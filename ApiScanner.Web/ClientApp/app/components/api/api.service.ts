import { Injectable, Inject } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/operator/map'

import { ApiModel } from './api.model';

@Injectable()
export class ApiService {
    constructor(private _http: HttpClient, @Inject('BASE_URL') private _baseUrl: string) { }

    public createApi(api: ApiModel) {
        const headers = new HttpHeaders().set('Content-Type', 'application/json');
        return this._http.post(`${this._baseUrl}/api/api`, JSON.stringify(api), { headers: headers });
    }

    public getApis(): Observable<ApiModel[]> {
        return this._http.get<ApiModel[]>(`${this._baseUrl}/api/api`);
    }

    public getApi(apiId: string): Observable<ApiModel> {
        return this._http.get<ApiModel>(`${this._baseUrl}/api/api/${apiId}`);
    }

    public canSeeApi(apiId: string): Observable<boolean> {
        return this._http.get<boolean>(`${this._baseUrl}/api/api/${apiId}/access`);
    }

    public updateApi(api: ApiModel) {
        const headers = new HttpHeaders().set('Content-Type', 'application/json');
        return this._http.put(`${this._baseUrl}/api/api`, JSON.stringify(api), { headers: headers });
    }

    public deleteApi(apiId: string) {
        return this._http.delete(`${this._baseUrl}/api/api/${apiId}`);
    }
}