import { Component, OnInit } from '@angular/core';
import { MatTableDataSource } from '@angular/material';

import { ApiModel } from './api.model';
import { ApiIntervalModel } from '../enums/api-interval.model';

import { ApiService } from './api.service';

@Component({
    templateUrl: './api-list.component.html',
    styleUrls: ['./api-list.component.css']
})
export class ApiListComponent implements OnInit {
    public ApiIntervalModel: typeof ApiIntervalModel = ApiIntervalModel;
    public displayedColumns: string[] = ['name', 'url', 'interval', 'enabled', 'edit'];
    public apiDataSource: MatTableDataSource<ApiModel> = new MatTableDataSource<ApiModel>();
    public selectedIndex = -1;

    constructor(
        private _apiService: ApiService) { }

    ngOnInit() {
        this._apiService.getApis()
            .subscribe(
            data => {
                this.apiDataSource.data = data;
            });
    }

    public highlight(index: number) {
        this.selectedIndex = this.selectedIndex == index ? -1 : index;
    }
}