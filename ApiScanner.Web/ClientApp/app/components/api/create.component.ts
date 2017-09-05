import { Component } from '@angular/core';
import { Router } from '@angular/router';

import { ApiModel } from './api.model';
import { HttpMethodTypeModel } from '../enums/http.method.type.model';
import { ApiIntervalModel } from '../enums/api.interval.model';
import { ConditionTypeModel } from '../enums/condition.type.model';
import { CompareTypeModel } from '../enums/compare.type.model';

import { ApiService } from './api.service';
import { NotificationDataService } from '../notification/notification-data.service';
import { NotificationClassType } from '../notification/notification.model';

@Component({
    templateUrl: './api.component.html'
})
export class ApiCreateComponent {
    public HttpMethodTypeModel: typeof HttpMethodTypeModel = HttpMethodTypeModel;
    public ApiIntervalModel: typeof ApiIntervalModel = ApiIntervalModel;
    public ConditionTypeModel: typeof ConditionTypeModel = ConditionTypeModel;
    public CompareTypeModel: typeof CompareTypeModel = CompareTypeModel;
    public creating: boolean = false;

    private lastErrorNotifId: string = '';

    constructor(
        private _apiService: ApiService,
        private _notificationDataService: NotificationDataService,
        private _router: Router) { }

    public api: ApiModel = {
        method: HttpMethodTypeModel.Get,
        interval: ApiIntervalModel.Daily,
        conditions: new Array(),
        enabled: true
    };

    public splitEnum(myEnum: any): Array<string> {
        var keys = Object.keys(myEnum);
        return keys.slice(keys.length / 2);
    }

    public addCondition() {
        this.api.conditions.push({
            matchType: ConditionTypeModel.Body,
            compareType: CompareTypeModel.Equals,
            shouldPass: true
        });
    }

    public removeCondition(index: number) {
        this.api.conditions.splice(index, 1);
    }

    public conditionTypeChanged(index: number) {
        if (this.api.conditions[index].matchType == ConditionTypeModel.ResponseTime || this.api.conditions[index].matchType == ConditionTypeModel.StatusCode)
            this.api.conditions[index].matchVar = '';
    }

    public createApi() {
        if (!this.creating) {
            this.creating = true;
            if (this.lastErrorNotifId != '') {
                this._notificationDataService.removeNotification(this.lastErrorNotifId);
                this.lastErrorNotifId = '';
            }
            this._apiService.createApi(this.api)
                .subscribe(
                data => {
                    this._router.navigateByUrl('');
                },
                error => {
                    this.lastErrorNotifId = this._notificationDataService.addNotification("Could not create api.", NotificationClassType.danger, false);
                    this.creating = false;
                });
        }
    }
}