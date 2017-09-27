using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SpaServices.Webpack;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ApiScanner.DataAccess;
using ApiScanner.Entities.Models;
using Microsoft.AspNetCore.Identity;
using ApiScanner.Business.Identity;
using Microsoft.AspNetCore.Http;
using ApiScanner.Business.Managers;
using ApiScanner.DataAccess.Repositories;

namespace ApiScanner.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Configure DataBase contexts
            CoreContext.ConnectionString = Configuration.GetConnectionString("DataAccessPostgreSqlProvider");
            services.AddDbContext<CoreContext>();

            // Identity framework
            services.AddIdentity<ApplicationUser, ApplicationRole>().AddEntityFrameworkStores<CoreContext>().AddDefaultTokenProviders();

            // Business services and repositories
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IApiManager, ApiManager>();
            services.AddScoped<ILocationManager, LocationManager>();
            services.AddTransient<IApiRepository, ApiRepository>();
            services.AddTransient<IConditionRepository, ConditionRepository>();
            services.AddTransient<ILocationRepository, LocationRepository>();

            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 8;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireDigit = false;
                options.Password.RequireUppercase = true;
                options.User.RequireUniqueEmail = true;
                options.SignIn.RequireConfirmedEmail = false;
            });

            services.AddMvc().AddJsonOptions(options => options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseWebpackDevMiddleware(new WebpackDevMiddlewareOptions
                {
                    HotModuleReplacement = true
                });
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            AuthAppBuilderExtensions.UseAuthentication(app);

            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");

                routes.MapSpaFallbackRoute(
                    name: "spa-fallback",
                    defaults: new { controller = "Home", action = "Index" });
            });
        }
    }
}
