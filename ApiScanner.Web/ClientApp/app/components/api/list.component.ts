import { Component, OnInit } from '@angular/core';
import { MatTableDataSource } from '@angular/material';

import { ApiModel } from './api.model';
import { ApiIntervalModel } from '../enums/api-interval.model';

import { ApiService } from './api.service';

@Component({
    templateUrl: './list.component.html'
})
export class ApiListComponent implements OnInit {
    public ApiIntervalModel: typeof ApiIntervalModel = ApiIntervalModel;
    public displayedColumns: string[] = ['apiName', 'apiUrl', 'apiInterval', 'apiEnabled', 'apiEdit'];
    public apiDataSource: MatTableDataSource<ApiModel> = new MatTableDataSource<ApiModel>();

    constructor(
        private _apiService: ApiService) { }

    ngOnInit() {
        this._apiService.getApis()
            .subscribe(
            data => {
                this.apiDataSource.data = data;
            });
    }
}