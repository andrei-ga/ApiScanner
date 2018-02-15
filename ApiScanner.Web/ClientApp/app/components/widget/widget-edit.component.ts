import { Component } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';

import { LocationService } from '../location/location.service';
import { ApiService } from '../api/api.service';
import { NotificationDataService } from '../notification/notification-data.service';
import { WidgetService } from './widget.service';

import { LocationModel } from '../location/location.model';
import { WidgetModel } from './widget.model';
import { ApiModel } from '../api/api.model';
import { NotificationClassType } from '../notification/notification.model';

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

    private subParams: any;
    private lastErrorNotifId: string = '';

    constructor(
        private _apiService: ApiService,
        private _locService: LocationService,
        private _widgetService: WidgetService,
        private _notificationDataService: NotificationDataService,
        private _route: ActivatedRoute,
        private _router: Router) { }

    ngOnInit() {
        this.subParams = this._route.params.subscribe(params => {
            let id = params['id'];
            if (id) {
                
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
                for (let i = 0; i < data.length; i++) {
                    if (this.widget.apiWidgets.findIndex(e => e.apiId == data[i].apiId) >= 0) {
                        this.includedApiIds.push(data[i].apiId!);
                    }
                }
            });
    }

    ngOnDestroy() {
        this.subParams.unsubscribe();
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
                    this.lastErrorNotifId = this._notificationDataService.addNotification("Could not create widget.", NotificationClassType.danger, false);
                    this.processing = false;
                });
        }
    }
}