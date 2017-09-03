import { Injectable, Inject } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/operator/map';

import { AccountModel } from './account.model';

@Injectable()
export class AccountService {
    constructor(private _http: HttpClient, @Inject('BASE_URL') private _baseUrl: string) { }

    public registerAccount(account: AccountModel) {
        const headers = new HttpHeaders().set('Content-Type', 'application/json');
        return this._http.post(`${this._baseUrl}/api/account/register`, JSON.stringify(account), { headers: headers });
    }

    public loginAccount(account: AccountModel) {
        const headers = new HttpHeaders().set('Content-Type', 'application/json');
        return this._http.post(`${this._baseUrl}/api/account/login`, JSON.stringify(account), { headers: headers });
    }

    public isLoggedIn() {
        return this._http.get(`${this._baseUrl}/api/account/logged`);
    }

    public logout() {
        const headers = new HttpHeaders().set('Content-Type', 'application/json');
        return this._http.post(`${this._baseUrl}/api/account/logout`, { headers: headers });
    }

    public getAccountData() {
        return this._http.get(`${this._baseUrl}/api/account/data`);
    }

    public resetPassword(account: AccountModel) {
        const headers = new HttpHeaders().set('Content-Type', 'application/json');
        return this._http.post(`${this._baseUrl}/api/account/reset`, JSON.stringify(account), { headers: headers });
    }
}