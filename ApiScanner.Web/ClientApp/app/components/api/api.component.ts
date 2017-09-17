import { Component } from '@angular/core';
import { Subscription } from 'rxjs/Subscription';
import { Router, ActivatedRoute } from '@angular/router';
import { TranslateService } from '@ngx-translate/core';

import { ApiModel } from './api.model';
import { HttpMethodTypeModel } from '../enums/http.method.type.model';
import { ApiIntervalModel } from '../enums/api.interval.model';
import { ConditionTypeModel } from '../enums/condition.type.model';
import { CompareTypeModel } from '../enums/compare.type.model';
import { AccountModel } from '../account/account.model';

import { ApiService } from './api.service';
import { NotificationDataService } from '../notification/notification-data.service';
import { NotificationClassType } from '../notification/notification.model';
import { AccountDataService } from '../account/account-data.service';

@Component({
    templateUrl: './api.component.html'
})
export class ApiCreateComponent {
    public HttpMethodTypeModel: typeof HttpMethodTypeModel = HttpMethodTypeModel;
    public ApiIntervalModel: typeof ApiIntervalModel = ApiIntervalModel;
    public ConditionTypeModel: typeof ConditionTypeModel = ConditionTypeModel;
    public CompareTypeModel: typeof CompareTypeModel = CompareTypeModel;
    public processing: boolean = false;
    public myUser?: AccountModel = undefined;

    private confirmDeleteApi: string = '';
    private lastErrorNotifId: string = '';
    private subParams: any;

    private subscribeAccount: Subscription;
    private subscribeConfirmDeleteApi: Subscription;

    constructor(
        private _apiService: ApiService,
        private _notificationDataService: NotificationDataService,
        private _route: ActivatedRoute,
        private _accountDataService: AccountDataService,
        private _translate: TranslateService,
        private _router: Router) {
        this.subscribeAccount = this._accountDataService.account
            .subscribe(
            data => {
                if (data.email) {
                    this.myUser = data;
                }
            });
        this.subscribeConfirmDeleteApi = this._translate.stream('ConfirmDeleteApi')
            .subscribe(
            data => {
                this.confirmDeleteApi = data;
            });
    }

    public api: ApiModel = {
        method: HttpMethodTypeModel.Get,
        interval: ApiIntervalModel.Daily,
        conditions: new Array(),
        enabled: true
    };

    ngOnInit() {
        this.subParams = this._route.params.subscribe(params => {
            let id = params['id'];
            if (id) {
                this._apiService.getApi(id)
                    .subscribe(
                    data => {
                        if (!data)
                            this._router.navigateByUrl('/apis/list');
                        this.api = data;
                    },
                    error => {
                        this._router.navigateByUrl('/apis/list');
                    });
            }
        });
    }

    ngOnDestroy() {
        this.subParams.unsubscribe();
        this.subscribeAccount.unsubscribe();
        this.subscribeConfirmDeleteApi.unsubscribe();
    }

    public splitEnum(myEnum: any): Array<string> {
        let keys = Object.keys(myEnum);
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
        if (!this.processing) {
            this.processing = true;
            if (this.lastErrorNotifId != '') {
                this._notificationDataService.removeNotification(this.lastErrorNotifId);
                this.lastErrorNotifId = '';
            }
            this._apiService.createApi(this.api)
                .subscribe(
                data => {
                    this._router.navigateByUrl('/apis/list');
                },
                error => {
                    this.lastErrorNotifId = this._notificationDataService.addNotification("Could not create api.", NotificationClassType.danger, false);
                    this.processing = false;
                });
        }
    }

    public updateApi() {
        if (!this.processing) {
            this.processing = true;
            if (this.lastErrorNotifId != '') {
                this._notificationDataService.removeNotification(this.lastErrorNotifId);
                this.lastErrorNotifId = '';
            }
            this._apiService.updateApi(this.api)
                .subscribe(
                data => {
                    this._router.navigateByUrl('/apis/list');
                },
                error => {
                    this.lastErrorNotifId = this._notificationDataService.addNotification("Could not update api.", NotificationClassType.danger, false);
                    this.processing = false;
                });
        }
    }    

    public deleteApi() {
        if (!this.processing && confirm(this.confirmDeleteApi)) {
            this.processing = true;
            if (this.lastErrorNotifId != '') {
                this._notificationDataService.removeNotification(this.lastErrorNotifId);
                this.lastErrorNotifId = '';
            }
            this._apiService.deleteApi(this.api.apiId ? this.api.apiId : '')
                .subscribe(
                data => {
                    this._router.navigateByUrl('/apis/list');
                },
                error => {
                    this.lastErrorNotifId = this._notificationDataService.addNotification("Could delete api.", NotificationClassType.danger, false);
                    this.processing = false;
                });
        }
    }

    public canEditApi(): boolean {
        return this.api.apiId != undefined && this.myUser != undefined && (this.myUser.id == this.api.userId || this.api.publicWrite) || false;
    }
}