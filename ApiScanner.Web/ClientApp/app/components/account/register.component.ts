import { Component } from '@angular/core';
import { Router } from '@angular/router';

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

    constructor(
        private _accountService: AccountService,
        private _notificationDataService: NotificationDataService,
        private _router: Router) { }

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
                    //this._notificationDataService.addNotification('Please confirm your email.', NotificationClassType.info, true);
                    this._router.navigateByUrl('/login');
                },
                err => {
                    let errorText = '';
                    if (err.status == 429) {
                        errorText = 'Too many retries per minute.';
                    } else {
                        for (let error of err.json().value) {
                            switch (error) {
                                case 'InvalidEmail':
                                    errorText += 'Email format is not valid.\n';
                                    break;
                                case 'PasswordTooShort':
                                    errorText += 'Password should be at least 8 characters long.\n';
                                    break;
                                case 'PasswordRequiresDigit':
                                    errorText += 'Password should contain at least one number character (0-9).\n';
                                    break;
                                case 'PasswordRequiresLower':
                                    errorText += 'Password should contain at least one lowercase character (a-z).\n';
                                    break;
                                case 'PasswordRequiresUpper':
                                    errorText += 'Password should contain at least one uppercase character (A-Z).\n';
                                    break;
                                case 'DuplicateEmail':
                                    errorText += 'This email is already registered.\n';
                                    break;
                                case 'PasswordMissmatch':
                                    errorText += 'Password does not match.\n';
                                    break;
                            }
                        }
                    }
                    if (errorText == '')
                        errorText = 'Register failed.';

                    this.lastErrorNotifId = this._notificationDataService.addNotification(errorText, NotificationClassType.danger, false);
                    this.registering = false;
                });
        }
    }
}