import { Component } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { Subscription } from 'rxjs/Subscription';
import { TranslateService } from '@ngx-translate/core';

import { LocationService } from '../location/location.service';
import { ApiService } from '../api/api.service';
import { NotificationDataService } from '../notification/notification-data.service';
import { WidgetService } from './widget.service';
import { AccountDataService } from '../account/account-data.service';

import { LocationModel } from '../location/location.model';
import { WidgetModel } from './widget.model';
import { ApiModel } from '../api/api.model';
import { NotificationClassType } from '../notification/notification.model';
import { AccountModel } from '../account/account.model';

@Component({
    templateUrl: './widget-edit.component.html'
})
export class WidgetEditComponent {
    public locations: LocationModel[] = new Array();
    public widget: WidgetModel = {
        apiWidgets: new Array(),
        publicRead: false
    }
    public apis: ApiModel[] = new Array();
    public includedApiIds: string[] = new Array();
    public processing: boolean = false;
    public apisLoaded: boolean = false;
    public widgetLoaded: boolean = false;

    private subParams: any;
    private lastErrorNotifId: string = '';
    private myUser?: AccountModel = undefined;
    private wordingConfirmDeleteWidget: string = '';
    private wordingWidgetCannotCreate: string = '';
    private wordingWidgetCannotDelete: string = '';
    private wordingWidgetCannotUpdate: string = '';

    private subscribeAccount: Subscription;

    constructor(
        private _apiService: ApiService,
        private _locService: LocationService,
        private _accountDataService: AccountDataService,
        private _widgetService: WidgetService,
        private _notificationDataService: NotificationDataService,
        private _route: ActivatedRoute,
        private _translate: TranslateService,
        private _router: Router) {
        this.subscribeAccount = this._accountDataService.account
            .subscribe(
            data => {
                if (data.email) {
                    this.myUser = data;
                }
            });
        this._translate.get(['ConfirmDeleteWidget', 'WidgetCannotCreate', 'WidgetCannotDelete', 'WidgetCannotUpdate'])
            .subscribe(data => {
                this.wordingConfirmDeleteWidget = data.ConfirmDeleteWidget;
                this.wordingWidgetCannotCreate = data.WidgetCannotCreate;
                this.wordingWidgetCannotDelete = data.WidgetCannotDelete;
                this.wordingWidgetCannotUpdate = data.WidgetCannotUpdate;
            });
    }

    ngOnInit() {
        this.subParams = this._route.params.subscribe(params => {
            let id = params['id'];
            if (id) {
                this._widgetService.getWidget(id)
                    .subscribe(
                    data => {
                        if (!data)
                            this._router.navigateByUrl('/widgets/list');
                        this.widget = data;

                        // check enabled apis on current widget
                        for (let i = 0; i < this.widget.apiWidgets.length; i++) {
                            this.includedApiIds.push(this.widget.apiWidgets[i].apiId!);
                        }
                        this.widgetLoaded = true;
                    },
                    error => {
                        this._router.navigateByUrl('/widgets/list');
                    });
            } else {
                this.widgetLoaded = true;
            }
        });
        this._locService.getLocations()
            .subscribe(
            data => {
                if (this.widget.locationId == undefined && data.length > 0) {
                    this.widget.locationId = data[0].locationId;
                }
                this.locations = data;
            });
        this._apiService.getApis()
            .subscribe(
            data => {
                this.apis = data;
                this.apisLoaded = true;
            });
    }

    ngOnDestroy() {
        this.subParams.unsubscribe();
        this.subscribeAccount.unsubscribe();
    }

    private mapApiWidgets() {
        this.widget.apiWidgets = new Array();
        for (let i = 0; i < this.includedApiIds.length; i++) {
            this.widget.apiWidgets.push({
                widgetId: this.widget.widgetId,
                apiId: this.includedApiIds[i]
            });
        }
    }

    public createWidget() {
        if (!this.processing) {
            this.processing = true;
            if (this.lastErrorNotifId != '') {
                this._notificationDataService.removeNotification(this.lastErrorNotifId);
                this.lastErrorNotifId = '';
            }

            // map api widgets
            this.mapApiWidgets();

            this._widgetService.createWidget(this.widget)
                .subscribe(
                data => {
                    this._router.navigateByUrl('/widgets/list');
                },
                error => {
                    this.lastErrorNotifId = this._notificationDataService.addNotification(this.wordingWidgetCannotCreate, NotificationClassType.danger, false);
                    this.processing = false;
                });
        }
    }

    public updateWidget() {
        if (!this.processing) {
            this.processing = true;
            if (this.lastErrorNotifId != '') {
                this._notificationDataService.removeNotification(this.lastErrorNotifId);
                this.lastErrorNotifId = '';
            }

            // map locations
            this.mapApiWidgets();

            this._widgetService.updateWidget(this.widget)
                .subscribe(
                data => {
                    this._router.navigateByUrl('/widgets/list');
                },
                error => {
                    this.lastErrorNotifId = this._notificationDataService.addNotification(this.wordingWidgetCannotUpdate, NotificationClassType.danger, false);
                    this.processing = false;
                });
        }
    }

    public deleteWidget() {
        if (!this.processing && confirm(this.wordingConfirmDeleteWidget)) {
            this.processing = true;
            if (this.lastErrorNotifId != '') {
                this._notificationDataService.removeNotification(this.lastErrorNotifId);
                this.lastErrorNotifId = '';
            }
            this._widgetService.deleteWidget(this.widget.widgetId!)
                .subscribe(
                data => {
                    this._router.navigateByUrl('/widgets/list');
                },
                error => {
                    this.lastErrorNotifId = this._notificationDataService.addNotification(this.wordingWidgetCannotDelete, NotificationClassType.danger, false);
                    this.processing = false;
                });
        }
    }

    public canEditWidget(): boolean {
        return (this.widget.widgetId != undefined && this.myUser != undefined && (this.myUser.id == this.widget.userId)) == true;
    }
}