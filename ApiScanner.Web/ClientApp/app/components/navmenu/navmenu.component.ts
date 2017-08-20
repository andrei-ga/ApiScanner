import { Component, OnDestroy } from '@angular/core';
import { Subscription } from 'rxjs/Subscription';
import { Router } from '@angular/router';

import { AccountDataService } from '../account/account-data.service';
import { AccountService } from '../account/account.service';
import { NotificationDataService } from '../notification/notification-data.service';

import { NotificationClassType } from '../notification/notification.model';
import { AccountModel } from '../account/account.model';

@Component({
    selector: 'nav-menu',
    templateUrl: './navmenu.component.html',
    styleUrls: ['./navmenu.component.css']
})
export class NavMenuComponent implements OnDestroy {
    public myUser?: AccountModel = undefined;
    public loggedIn: boolean = false;

    private _subscribeAccount: Subscription;

    constructor(
        private _accountDataService: AccountDataService,
        private router: Router,
        private _notificationDataService: NotificationDataService,
        private _accountService: AccountService) {
        this._subscribeAccount = this._accountDataService.account
            .subscribe(
            data => {
                if (data) {
                    this.myUser = data;
                    this.loggedIn = true;
                } else {
                    this.myUser = undefined;
                    this.loggedIn = false;
                }
            });
    }

    ngOnDestroy() {
        this._subscribeAccount.unsubscribe();
    }

    public logout(): void {
        this._accountService.logout()
            .subscribe(
            data => {
                if (data) {
                    this._notificationDataService.addNotification('Successfully logged out.', NotificationClassType.success, true);
                    this._accountDataService.refreshData();
                    this.router.navigateByUrl('/login');
                }
            });
    }
}
