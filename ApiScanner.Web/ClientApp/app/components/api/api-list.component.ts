import { Component, OnInit, ViewChild } from '@angular/core';
import { MatTableDataSource, MatPaginator, PageEvent } from '@angular/material';

import { ApiModel } from './api.model';
import { ApiIntervalModel } from '../enums/api-interval.model';

import { ApiService } from './api.service';

@Component({
    templateUrl: './api-list.component.html',
    styleUrls: ['./api-list.component.css']
})
export class ApiListComponent implements OnInit {
    @ViewChild(MatPaginator) paginator: MatPaginator;

    public ApiIntervalModel: typeof ApiIntervalModel = ApiIntervalModel;
    public displayedColumns: string[] = ['name', 'url', 'interval', 'enabled', 'edit'];
    public apiDataSource: MatTableDataSource<ApiModel> = new MatTableDataSource<ApiModel>();
    public selectedIndex = -1;
    public paginatorPageSize = 25;

    constructor(
        private _apiService: ApiService) {
    }

    ngOnInit() {
        let cachePageSize = localStorage.getItem('ApiList_PageSize');
        if (cachePageSize)
            this.paginatorPageSize = parseInt(cachePageSize);

        this._apiService.getApis()
            .subscribe(
            data => {
                this.apiDataSource.data = data;
            });
    }

    ngAfterViewInit() {
        this.apiDataSource.paginator = this.paginator;
    }

    public highlight(index: number) {
        this.selectedIndex = this.selectedIndex == index ? -1 : index;
    }

    public paginatorChangePage(event: PageEvent) {
        localStorage.setItem('ApiList_PageSize', event.pageSize.toString());
        this.selectedIndex = -1;
    }
}