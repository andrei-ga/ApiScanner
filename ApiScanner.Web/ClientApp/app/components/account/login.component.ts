import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { TranslateService } from '@ngx-translate/core';

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
    private wordingLoginFailed: string = '';
    private wordingLoginSuccess: string = '';
    private wordingManyRetries: string = '';
    private wordingLoginEmailNotConfirmed: string = '';
    private wordingLoginUserOrPassIncorrect: string = '';

    constructor(
        private _accountDataService: AccountDataService,
        private _accountService: AccountService,
        private _notificationDataService: NotificationDataService,
        private _translate: TranslateService,
        private router: Router) {
        this._translate.get(['LoginFailed', 'LoginSuccess', 'ManyRetries', 'LoginEmailNotConfirmed', 'LoginUserOrPassIncorrect'])
            .subscribe(data => {
                this.wordingLoginFailed = data.LoginFailed;
                this.wordingLoginSuccess = data.LoginSuccess;
                this.wordingManyRetries = data.ManyRetries;
                this.wordingLoginEmailNotConfirmed = data.LoginEmailNotConfirmed;
                this.wordingLoginUserOrPassIncorrect = data.LoginUserOrPassIncorrect;
            });
    }

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
                    this._notificationDataService.addNotification(this.wordingLoginSuccess, NotificationClassType.success, true);
                    this._accountDataService.refreshData();
                    this.router.navigateByUrl('');
                },
                error => {
                    let errorText = this.wordingLoginFailed;
                    if (error.status == 429) {
                        errorText = this.wordingManyRetries;
                    } else {
                        switch (error.error.value) {
                            case 'UserOrPassIncorrect':
                                errorText = this.wordingLoginUserOrPassIncorrect;
                                break;
                            case 'EmailNotConfirmed':
                                errorText = this.wordingLoginEmailNotConfirmed;
                                break;
                        }
                    }
                    this.lastErrorNotifId = this._notificationDataService.addNotification(errorText, NotificationClassType.danger, false);
                    this.signing = false;
                });
        }
    }
}