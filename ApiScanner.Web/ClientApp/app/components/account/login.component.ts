import { Component } from '@angular/core';
import { Router } from '@angular/router';

import { AccountService } from './account.service';
import { AccountDataService } from './account-data.service';
import { NotificationDataService } from '../notification/notification-data.service';

import { AccountModel } from './account.model';
import { NotificationClassType } from '../notification/notification.model';

@Component({
    templateUrl: './login.component.html',
    styleUrls: ['./forms.component.css']
})
export class LoginComponent {
    public account: AccountModel = {
        email: '',
        password: '',
        rememberLogin: false
    }
    public signing: boolean = false;

    private lastErrorNotifId: string = '';

    constructor(
        private _accountDataService: AccountDataService,
        private _accountService: AccountService,
        private _notificationDataService: NotificationDataService,
        private router: Router) { }

    public loginAccount() {
        if (!this.signing) {
            this.signing = true;
            if (this.lastErrorNotifId != '') {
                this._notificationDataService.removeNotification(this.lastErrorNotifId);
                this.lastErrorNotifId = '';
            }
            this._accountService.loginAccount(this.account)
                .subscribe(
                data => {
                    this._notificationDataService.addNotification('Successfully logged in.', NotificationClassType.success, true);
                    this._accountDataService.refreshData();
                    this.router.navigateByUrl('');
                },
                error => {
                    let errorText = 'Login failed.';
                    if (error.status == 429) {
                        errorText = 'Too many retries per minute.';
                    } else {
                        switch (error.error.value) {
                            case 'UserOrPassIncorrect':
                                errorText = 'Incorrect username or password.';
                                break;
                            case 'EmailNotConfirmed':
                                errorText = 'Please confirm your email before logging in.';
                                break;
                        }
                    }
                    this.lastErrorNotifId = this._notificationDataService.addNotification(errorText, NotificationClassType.danger, false);
                    this.signing = false;
                });
        }
    }
}