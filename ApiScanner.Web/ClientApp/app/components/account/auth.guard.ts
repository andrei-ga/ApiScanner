import { Injectable } from '@angular/core';
import { Router, CanActivate } from '@angular/router';
import { Observable } from 'rxjs/Observable';

import { AccountService } from './account.service';

@Injectable()
export class GuardLogin implements CanActivate {

    constructor(
        private router: Router,
        private _accountService: AccountService) { }

    canActivate(): Observable<boolean> {
        return this._accountService.isLoggedIn().map(data => {
            if (!data)
                return true;
            else {
                this.router.navigate(['']);
                return false;
            }
        });
    }
}
@Injectable()
export class GuardLoggedIn implements CanActivate {

    constructor(
        private router: Router,
        private _accountService: AccountService) { }

    canActivate(): Observable<boolean> {
        return this._accountService.isLoggedIn().map(data => {
            if (data)
                return true;
            else {
                this.router.navigate(['']);
                return false;
            }
        });
    }
}