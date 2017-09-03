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
}