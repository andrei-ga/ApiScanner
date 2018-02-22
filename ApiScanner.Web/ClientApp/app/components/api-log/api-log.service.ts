import { Injectable, Inject } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/operator/map';

import { ApiLogModel } from './api-log.model';

@Injectable()
export class ApiLogService {
    constructor(private _http: HttpClient, @Inject('BASE_URL') private _baseUrl: string) { }

    public getApiLogs(apiId: string, dateFrom?: Date): Observable<ApiLogModel[]> {
        let url = `${this._baseUrl}/api/apilog/${apiId}`;
        if (dateFrom)
            url += `?dateFrom=${dateFrom.toISOString()}`;
        return this._http.get<ApiLogModel[]>(url);
    }

    public getWidgetLogs(widgetId: string, includeFails: boolean, dateFrom?: Date): Observable<ApiLogModel[]> {
        let url = `${this._baseUrl}/api/apilog/widget/${widgetId}`;
        let hasQuery = false;
        if (dateFrom) {
            url += `?dateFrom=${dateFrom.toISOString()}`;
            hasQuery = true;
        }
        if (includeFails)
            url += (hasQuery ? '&' : '?') + 'includeFails=true';
        return this._http.get<ApiLogModel[]>(url);
    }
}