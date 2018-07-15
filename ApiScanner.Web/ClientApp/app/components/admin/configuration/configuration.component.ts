import { Component } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';

import { ConfigurationService } from './configuration.service';
import { NotificationDataService } from '../../notification/notification-data.service';
import { NotificationClassType } from '../../notification/notification.model';

import { ConfigurationModel } from './configuration.model';

@Component({
    templateUrl: './configuration.component.html'
})
export class AdminConfigurationComponent {
    public configs: ConfigurationModel[] = new Array();
    public processing: boolean = false;

    private lastErrorNotifId: string = '';
    private wordingConfigSaved: string = '';
    private wordingConfigCannotSave: string = '';

    constructor(
        private _translate: TranslateService,
        private _notificationDataService: NotificationDataService,
        private _configService: ConfigurationService) { }

    ngOnInit() {
        this._configService.getConfigs()
            .subscribe(
            data => {
                this.configs = data;
            });

        this._translate.get(['ConfigSaved', 'ConfigCannotSave'])
            .subscribe(data => {
                this.wordingConfigSaved = data.ConfigSaved;
                this.wordingConfigCannotSave = data.ConfigCannotSave;
            });
    }

    public saveConfigs() {
        if (!this.processing) {
            this.processing = true;
            if (this.lastErrorNotifId != '') {
                this._notificationDataService.removeNotification(this.lastErrorNotifId);
                this.lastErrorNotifId = '';
            }

            this._configService.saveConfigs(this.configs)
                .subscribe(
                data => {
                    this.processing = false;
                    this._notificationDataService.addNotification(this.wordingConfigSaved, NotificationClassType.success, true);
                },
                error => {
                    this.lastErrorNotifId = this._notificationDataService.addNotification(this.wordingConfigCannotSave, NotificationClassType.danger, false);
                    this.processing = false;
                });
        }
    }
}