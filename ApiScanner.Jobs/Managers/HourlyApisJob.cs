using ApiScanner.DataAccess.Repositories;
using ApiScanner.Entities.Enums;
using ApiScanner.Entities.Models;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace ApiScanner.Jobs.Managers
{
    public class HourlyApisJob
    {
        private readonly IApiRepository _apiRepo;
        private readonly IApiLogRepository _apiLogRepo;
        private readonly IConfiguration _config;

        private static Guid _locationId;
        private static string _testApi;
        private static HttpClient _client = new HttpClient();

        public HourlyApisJob(IApiRepository apiRepo, IApiLogRepository apiLogRepo, IConfiguration configuration)
        {
            _apiRepo = apiRepo;
            _apiLogRepo = apiLogRepo;
            _config = configuration;
            _locationId = _config.GetValue<Guid>("LocationId");
            _testApi = _config.GetValue<string>("TestApi");

            CacheControlHeaderValue cacheControl = new CacheControlHeaderValue
            {
                NoCache = true
            };
            _client.DefaultRequestHeaders.CacheControl = cacheControl;
        }

        public async Task ExecuteJobAsync(ApiInterval interval)
        {
            await RunApiTest();
            // get api list of current location
            var apis = await _apiRepo.GetEnabledApisAsync(_locationId, interval);
            foreach (var api in apis)
            {
                // do a http request and check each api
                try
                {
                    await RunApiAsync(api);
                }
                catch(Exception)
                {
                    // log error
                }
            }
        }

        private async Task RunApiTest()
        {
            await _client.GetAsync(_testApi);
        }

        private async Task RunApiAsync(ApiModel api)
        {
            var request = new HttpRequestMessage
            {
                RequestUri = new Uri(api.Url)
            };
            HttpContent content;

            // check api method
            switch (api.Method)
            {
                case HttpMethodType.Get:
                    request.Method = HttpMethod.Get;
                    break;
                case HttpMethodType.Head:
                    request.Method = HttpMethod.Head;
                    break;
                case HttpMethodType.Post:
                    request.Method = HttpMethod.Post;
                    content = new StringContent(api.Body);
                    request.Content = content;
                    break;
                case HttpMethodType.Put:                
                    request.Method = HttpMethod.Put;
                    content = new StringContent(api.Body);
                    request.Content = content;
                    break;
                case HttpMethodType.Patch:
                    var method = new HttpMethod("PATCH");
                    request.Method = method;
                    content = new StringContent(api.Body);
                    request.Content = content;
                    break;
                case HttpMethodType.Delete:
                    request.Method = HttpMethod.Delete;
                    break;
                case HttpMethodType.Options:
                    request.Method = HttpMethod.Options;
                    break;
            }

            // check api headers
            if (!string.IsNullOrWhiteSpace(api.Headers))
            {
                var headerValues = api.Headers.Split('\n').Where(e => !string.IsNullOrWhiteSpace(e)).Select(e => e.Split(":")).ToDictionary(e => e[0].Trim().TrimStart('"').TrimEnd('"'), e => e[1].Trim().TrimStart('"').TrimEnd('"'));
                foreach(var header in headerValues)
                {
                    switch(header.Key)
                    {
                        case "Content-Type":
                            var headerValueSplit = header.Value.Split(';');
                            var contentType = headerValueSplit.FirstOrDefault();
                            var charSet = "utf-8";
                            if (headerValueSplit.Length > 1)
                                charSet = headerValueSplit[1].Trim().Replace("charset=", "");
                            content = new StringContent(api.Body, Encoding.GetEncoding(charSet), contentType);
                            request.Content = content;
                            break;
                        default:
                            request.Headers.TryAddWithoutValidation(header.Key, header.Value);
                            break;
                    }
                }
            }

            var stopWatch = Stopwatch.StartNew();
            var response = await _client.SendAsync(request);
            stopWatch.Stop();

            // do not download content bigger than 1mb
            var bytesLength = response.Content.Headers.ContentLength;
            ApiLogModel log = new ApiLogModel
            {
                ApiId = api.ApiId,
                StatusCode = (int)response.StatusCode,
                Headers = JsonConvert.SerializeObject(response.Headers.Concat(response.Content.Headers).ToDictionary(e => e.Key, e => string.Join(", ", e.Value))),
                Content = bytesLength > 1024 * 1024 ? string.Empty : await response.Content.ReadAsStringAsync(),
                ResponseTime = stopWatch.ElapsedMilliseconds,
                Success = response.IsSuccessStatusCode,
                LogDate = DateTime.UtcNow
            };

            // check if the response content is json
            bool isJson = response.Content.Headers.ContentType?.MediaType == "application/json";
            bool compiledValue;

            // check conditions
            foreach (var condition in api.Conditions)
            {
                var compareExp = GetCompareExpression(condition.CompareType);
                var compareType = GetCompareType(condition.MatchType);
                var logValue = GetMatchValue(condition, log, isJson);
                var compareValue = condition.CompareValue;
                if (compareExp == ExpressionType.Dynamic)
                {
                    // contains, not contains, exists and not exists conditions
                    switch (condition.CompareType)
                    {
                        case CompareType.Contains:
                            compiledValue = logValue.Contains(compareValue);
                            break;
                        case CompareType.NotContains:
                            compiledValue = !logValue.Contains(compareValue);
                            break;
                        case CompareType.Exists:
                            compiledValue = HasMatchValue(condition, log);
                            break;
                        case CompareType.NotExists:
                            compiledValue = !HasMatchValue(condition, log);
                            break;
                        default:
                            compiledValue = false;
                            break;
                    }
                }
                else
                {
                    // the rest type of conditions                    
                    try
                    {
                        if (compareType == typeof(int))
                        {
                            int.TryParse(logValue, out int logValueInt);
                            int.TryParse(compareValue, out int compareValueInt);

                            Expression argLeft = Expression.Constant(logValueInt, compareType);
                            Expression argRight = Expression.Constant(compareValueInt, compareType);
                            Expression exp = Expression.MakeBinary(compareExp, argLeft, argRight);
                            var lambda = Expression.Lambda<Func<bool>>(exp);
                            var compiled = lambda.Compile().DynamicInvoke();
                            compiledValue = (bool)compiled;
                        }
                        else
                        {
                            Expression argLeft = Expression.Constant(logValue, compareType);
                            Expression argRight = Expression.Constant(compareValue, compareType);
                            Expression exp = Expression.MakeBinary(compareExp, argLeft, argRight);
                            var lambda = Expression.Lambda<Func<bool>>(exp);
                            var compiled = lambda.Compile().DynamicInvoke();
                            compiledValue = (bool)compiled;
                        }
                    }
                    catch(Exception)
                    {
                        compiledValue = false;
                    }
                }

                if (condition.ShouldPass)
                {
                    if (compiledValue == false)
                    {
                        log.Success = false;
                        break;
                    }
                    else
                    {
                        log.Success = true;
                    }
                }
                else
                {
                    if (compiledValue == false)
                    {
                        log.Success = true;
                    }
                    else
                    {
                        log.Success = false;
                        break;
                    }
                }
            }

            // delete content if more than 1kb length
            if (bytesLength > 1024)
                log.Content = string.Empty;

            await _apiLogRepo.LogAsync(log);
        }

        /// <summary>
        /// Get the math operation expression.
        /// </summary>
        /// <param name="comp"></param>
        /// <returns></returns>
        private ExpressionType GetCompareExpression(CompareType comp)
        {
            switch (comp)
            {
                case CompareType.Equals:
                    return ExpressionType.Equal;
                case CompareType.Different:
                    return ExpressionType.NotEqual;
                case CompareType.GreaterThan:
                    return ExpressionType.GreaterThan;
                case CompareType.GreaterThanOrEqual:
                    return ExpressionType.GreaterThanOrEqual;
                case CompareType.LessThan:
                    return ExpressionType.LessThan;
                case CompareType.LessThanOrEqual:
                    return ExpressionType.LessThanOrEqual;
                default:
                    return ExpressionType.Dynamic;
            }
        }

        /// <summary>
        /// Get the type of variables that should be used.
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        private Type GetCompareType(ConditionType condition)
        {
            switch (condition)
            {
                case ConditionType.ResponseTime:
                case ConditionType.StatusCode:
                    return typeof(int);
                default:
                    return typeof(string);
            }
        }

        /// <summary>
        /// Get the value of variables that should be used.
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="log"></param>
        /// <returns></returns>
        private string GetMatchValue(ConditionModel condition, ApiLogModel log, bool isJson)
        {
            switch (condition.MatchType)
            {
                case ConditionType.Body:
                    if (isJson && !string.IsNullOrEmpty(condition.MatchVar))
                    {
                        try
                        {
                            var content = (JObject)JsonConvert.DeserializeObject(log.Content);
                            return content.GetValue(condition.MatchVar)?.Value<string>();
                        }
                        catch (Exception)
                        {
                            return null;
                        }
                    }
                    return log.Content;
                case ConditionType.Header:
                    if (!string.IsNullOrEmpty(condition.MatchVar))
                    {
                        try
                        {
                            var content = (JObject)JsonConvert.DeserializeObject(log.Headers);
                            return content.GetValue(condition.MatchVar)?.Value<string>();
                        }
                        catch (Exception)
                        {
                            return null;
                        }
                    }
                    return log.Headers;
                case ConditionType.ResponseTime:
                    return log.ResponseTime.ToString();
                case ConditionType.StatusCode:
                    return log.StatusCode.ToString();
                default:
                    return null;
            }
        }

        /// <summary>
        /// Check if log has the specified variable.
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="log"></param>
        /// <returns></returns>
        private bool HasMatchValue(ConditionModel condition, ApiLogModel log)
        {
            if (string.IsNullOrEmpty(condition.MatchVar))
                return false;

            switch (condition.MatchType)
            {
                case ConditionType.Body:
                    return log.Content.Contains(condition.MatchVar);
                case ConditionType.Header:
                    return log.Headers.Contains(condition.MatchVar);
                default:
                    return false;
            }
        }
    }
}
