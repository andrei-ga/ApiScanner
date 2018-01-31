import { Component } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

import { ApiLogModel } from '../api-log/api-log.model';

import { ApiLogService } from '../api-log/api-log.service';

@Component({
    templateUrl: './api.component.html'
})
export class ApiComponent {
    public apiLogs: ApiLogModel[] = new Array();

    private subParams: any;

    constructor(
        private _apiLogService: ApiLogService,
        private _route: ActivatedRoute) { }

    ngOnInit() {
        this.subParams = this._route.params.subscribe(params => {
            let id = params['id'];
            if (id) {
                let dateFrom = new Date();
                dateFrom.setUTCDate(dateFrom.getUTCDate() - 7);
                this._apiLogService.getApiLogs(id, dateFrom)
                    .subscribe(
                    data => {
                        this.apiLogs = data;
                    });
            }
        });
    }

    ngOnDestroy() {
        this.subParams.unsubscribe();
    }
}