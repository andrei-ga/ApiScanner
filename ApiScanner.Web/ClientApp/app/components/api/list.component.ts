﻿import { Component, OnInit } from '@angular/core';

import { ApiModel } from './api.model';
import { ApiIntervalModel } from '../enums/api.interval.model';

import { ApiService } from './api.service';

@Component({
    templateUrl: './list.component.html'
})
export class ApiListComponent implements OnInit {
    public apis: ApiModel[] = new Array();
    public ApiIntervalModel: typeof ApiIntervalModel = ApiIntervalModel;

    constructor(
        private _apiService: ApiService) { }

    ngOnInit() {
        this._apiService.getApis()
            .subscribe(
            data => {
                this.apis = data;
            });
    }
}