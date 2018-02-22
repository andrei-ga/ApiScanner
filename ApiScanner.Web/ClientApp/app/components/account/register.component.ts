import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { TranslateService } from '@ngx-translate/core';

import { AccountService } from './account.service';
import { NotificationDataService } from '../notification/notification-data.service';

import { AccountModel } from './account.model';
import { NotificationClassType } from '../notification/notification.model';

@Component({
    templateUrl: './register.component.html',
    styleUrls: ['./forms.component.css']
})
export class RegisterComponent {
    public account: AccountModel = {
        email: '',
        password: '',
        passwordRepeat: ''
    }
    public registering: boolean = false;

    private lastErrorNotifId: string = '';
    private wordingManyRetries: string = '';
    private wordingInvalidEmail: string = '';
    private wordingPasswordTooShort: string = '';
    private wordingPasswordRequiresDigit: string = '';
    private wordingPasswordRequiresLower: string = '';
    private wordingPasswordRequiresUpper: string = '';
    private wordingDuplicateEmail: string = '';
    private wordingPasswordMissmatch: string = '';
    private wordingRegisterFailed: string = '';

    constructor(
        private _accountService: AccountService,
        private _translate: TranslateService,
        private _notificationDataService: NotificationDataService,
        private _router: Router) { }

    ngOnInit() {
        this._translate.get(['ManyRetries', 'InvalidEmail', 'PasswordTooShort', 'PasswordRequiresDigit', 'PasswordRequiresLower', 'PasswordRequiresUpper', 'DuplicateEmail', 'PasswordMissmatch', 'RegisterFailed'])
            .subscribe(data => {
                this.wordingManyRetries = data.ManyRetries;
                this.wordingInvalidEmail = data.InvalidEmail;
                this.wordingPasswordTooShort = data.PasswordTooShort;
                this.wordingPasswordRequiresDigit = data.PasswordRequiresDigit;
                this.wordingPasswordRequiresLower = data.PasswordRequiresLower;
                this.wordingPasswordRequiresUpper = data.PasswordRequiresUpper;
                this.wordingDuplicateEmail = data.DuplicateEmail;
                this.wordingPasswordMissmatch = data.PasswordMissmatch;
                this.wordingRegisterFailed = data.RegisterFailed;
            });
    }

    public registerAccount() {
        if (!this.registering) {
            this.registering = true;
            if (this.lastErrorNotifId != '') {
                this._notificationDataService.removeNotification(this.lastErrorNotifId);
                this.lastErrorNotifId = '';
            }
            this._accountService.registerAccount(this.account)
                .subscribe(
                data => {
                    //this._notificationDataService.addNotification('Please confirm your email.', NotificationClassType.info, true); TODO: add setting in config
                    this._router.navigateByUrl('login');
                },
                error => {
                    let errorText = '';
                    if (error.status == 429) {
                        errorText = this.wordingManyRetries;
                    } else {
                        for (let err of error.error.value) {
                            switch (err) {
                                case 'InvalidEmail':
                                    errorText += `${this.wordingInvalidEmail}\n`;
                                    break;
                                case 'PasswordTooShort':
                                    errorText += `${this.wordingPasswordTooShort}\n`;
                                    break;
                                case 'PasswordRequiresDigit':
                                    errorText += `${this.wordingPasswordRequiresDigit}\n`;
                                    break;
                                case 'PasswordRequiresLower':
                                    errorText += `${this.wordingPasswordRequiresLower}\n`;
                                    break;
                                case 'PasswordRequiresUpper':
                                    errorText += `${this.wordingPasswordRequiresUpper}\n`;
                                    break;
                                case 'DuplicateEmail':
                                    errorText += `${this.wordingDuplicateEmail}\n`
                                    break;
                                case 'PasswordMissmatch':
                                    errorText += `${this.wordingPasswordMissmatch}\n`
                                    break;
                            }
                        }
                    }
                    if (errorText == '')
                        errorText = this.wordingRegisterFailed;

                    this.lastErrorNotifId = this._notificationDataService.addNotification(errorText, NotificationClassType.danger, false);
                    this.registering = false;
                });
        }
    }
}