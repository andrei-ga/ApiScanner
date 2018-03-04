import { Injectable } from '@angular/core';
import { Router, CanActivate, ActivatedRouteSnapshot } from '@angular/router';
import { Observable } from 'rxjs/Observable';

import { AccountService } from './account.service';
import { ApiService } from '../api/api.service';
import { WidgetService } from '../widget/widget.service';

@Injectable()
export class GuardLogin implements CanActivate {

    constructor(
        private _router: Router,
        private _accountService: AccountService) { }

    canActivate(): Observable<boolean> {
        return this._accountService.isLoggedIn().map(data => {
            if (!data)
                return true;
            else {
                this._router.navigateByUrl('/');
                return false;
            }
        });
    }
}

@Injectable()
export class GuardLoggedIn implements CanActivate {

    constructor(
        private _router: Router,        
        private _accountService: AccountService) { }

    canActivate(): Observable<boolean> {
        return this._accountService.isLoggedIn().map(data => {
            if (data)
                return true;
            else {
                this._router.navigateByUrl('/');
                return false;
            }
        });
    }
}

@Injectable()
export class GuardSeeApi implements CanActivate {

    constructor(
        private _router: Router,
        private _apiService: ApiService) { }

    canActivate(next: ActivatedRouteSnapshot): Observable<boolean> {

        return this._apiService.canSeeApi(next.params.id).map(data => {
            if (data)
                return true;
            else {
                this._router.navigateByUrl('/');
                return false;
            }
        });
    }
}

@Injectable()
export class GuardSeeWidget implements CanActivate {

    constructor(
        private _router: Router,
        private _widgetService: WidgetService) { }

    canActivate(next: ActivatedRouteSnapshot): Observable<boolean> {

        return this._widgetService.canSeeWidget(next.params.id).map(data => {
            if (data)
                return true;
            else {
                this._router.navigateByUrl('/');
                return false;
            }
        });
    }
}

@Injectable()
export class GuardAdmin implements CanActivate {

    constructor(
        private _router: Router,
        private _accountService: AccountService) { }

    canActivate(): Observable<boolean> {
        return this._accountService.isAdmin().map(data => {
            if (data)
                return true;
            else {
                this._router.navigateByUrl('/');
                return false;
            }
        });
    }
}