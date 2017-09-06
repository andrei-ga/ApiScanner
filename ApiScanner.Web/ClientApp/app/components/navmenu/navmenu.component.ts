import { Component, OnDestroy, Input } from '@angular/core';
import { Subscription } from 'rxjs/Subscription';
import { Router } from '@angular/router';

import { AccountDataService } from '../account/account-data.service';
import { AccountService } from '../account/account.service';
import { NotificationDataService } from '../notification/notification-data.service';

import { NotificationClassType } from '../notification/notification.model';
import { AccountModel } from '../account/account.model';

import { TranslationService } from '../shared/services/translation.service';

@Component({
    selector: 'nav-menu',
    templateUrl: './navmenu.component.html',
    styleUrls: ['./navmenu.component.css']
})
export class NavMenuComponent implements OnDestroy {
    public myUser?: AccountModel = undefined;
    public loggedIn: boolean = false;
    public myLanguage: string;

    @Input() pageHeader: string;

    private subscribeAccount: Subscription;
    private subscribeLanguage: Subscription;

    constructor(
        private _accountDataService: AccountDataService,
        private _router: Router,
        private _notificationDataService: NotificationDataService,
        private _accountService: AccountService,
        private _translation: TranslationService) {
        this.subscribeAccount = this._accountDataService.account
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
        this.subscribeLanguage = this._translation.language
            .subscribe(
            data => {
                this.myLanguage = data ? data : 'en';
            });
    }

    ngOnDestroy() {
        this.subscribeAccount.unsubscribe();
        this.subscribeLanguage.unsubscribe();
    }

    public logout(): void {
        this._accountService.logout()
            .subscribe(
            data => {
                if (data) {
                    this._notificationDataService.addNotification('Successfully logged out.', NotificationClassType.success, true);
                    this._accountDataService.refreshData();
                    this._router.navigate(['']);
                }
            });
    }

    public switchLanguage(language: string) {
        this._translation.changeLanguage(language);
    }
}
