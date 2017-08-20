import { Injectable, Inject } from '@angular/core';
import { Http, Response, URLSearchParams, Headers } from '@angular/http';
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/operator/map'

import { AccountModel } from './account.model';

@Injectable()
export class AccountService {
    constructor(private _http: Http, @Inject('BASE_URL') private _baseUrl: string) { }

    registerAccount(account: AccountModel): Observable<Response> {
        let headers = new Headers();
        headers.append('Content-Type', 'application/json');

        return this._http.post(`${this._baseUrl}/api/Account/Register`, JSON.stringify(account), { headers: headers })
            .map((res: Response) => res.json());
    }

    loginAccount(account: AccountModel): Observable<Response> {
        let headers = new Headers();
        headers.append('Content-Type', 'application/json');

        return this._http.post(`${this._baseUrl}/api/Account/Login`, JSON.stringify(account), { headers: headers })
            .map((res: Response) => res.json());
    }

    isLoggedIn(): Observable<boolean> {
        return this._http.get(`${this._baseUrl}/api/Account/LoggedIn`)
            .map((res: Response) => res.json());
    }

    logout(): Observable<boolean> {
        let headers = new Headers();
        headers.append('Content-Type', 'application/json');

        return this._http.post(`${this._baseUrl}/api/Account/Logout`, { headers: headers })
            .map((res: Response) => res.json());
    }

    getAccountData(): Observable<AccountModel> {
        return this._http.get(`${this._baseUrl}/api/Account/AccountData`)
            .map((res: Response) => res.json());
    }

    forgotPassword(email: string): Observable<boolean> {
        let headers = new Headers();
        headers.append('Content-Type', 'application/json');

        return this._http.post(`${this._baseUrl}/api/Account/ForgotPassword`, JSON.stringify({ value: email }), { headers: headers })
            .map((res: Response) => res.json());
    }

    resetPassword(account: AccountModel): Observable<boolean> {
        let headers = new Headers();
        headers.append('Content-Type', 'application/json');

        return this._http.post(`${this._baseUrl}/api/Account/ResetPassword`, JSON.stringify(account), { headers: headers })
            .map((res: Response) => res.json());
    }
}