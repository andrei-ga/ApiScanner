import { Injectable, Inject } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/operator/map';

import { ConfigurationModel } from './configuration.model';

@Injectable()
export class ConfigurationService {
    constructor(private _http: HttpClient, @Inject('BASE_URL') private _baseUrl: string) { }

    public getConfigs(): Observable<ConfigurationModel[]> {
        return this._http.get<ConfigurationModel[]>(`${this._baseUrl}/api/configuration`);
    }

    public saveConfigs(configs: ConfigurationModel[]) {
        const headers = new HttpHeaders().set('Content-Type', 'application/json');
        return this._http.put(`${this._baseUrl}/api/configuration`, JSON.stringify(configs), { headers: headers });
    }
}