import { Component } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { NgxChartsModule } from '@swimlane/ngx-charts';
import { forkJoin } from "rxjs/observable/forkJoin";

import { ApiLogModel } from '../api-log/api-log.model';
import { ApiModel } from '../api/api.model';
import { LineChartDataModel, LineChartDataSeriesModel } from '../chart/line-chart.model';
import { SimpleStringString } from '../shared/models/simple-pairs.model';

import { ApiLogService } from '../api-log/api-log.service';
import { ApiService } from '../api/api.service';

import * as moment from 'moment';

interface DataValuesLog {
    location: string,
    logDate: string,
    logValues: number[]
}

@Component({
    templateUrl: './api.component.html'
})
export class ApiComponent {
    public apiLogs: ApiLogModel[] = new Array();
    // chart attributes
    public chartShowXAxis: boolean = true;
    public chartShowYAxis: boolean = true;
    public chartGradient: boolean = false;
    public chartShowLegend: boolean = true;
    public chartLegendTitle: string = 'Location';
    public chartShowXAxisLabel: boolean = true;
    public chartXAxisLabel: string = 'Date';
    public chartShowYAxisLabel: boolean = true;
    public chartYAxisLabel: string = 'Response time (ms)';
    public chartAutoScale: boolean = false;
    public chartResults: LineChartDataModel[] = new Array();
    public chartDataReady: boolean = false;

    public api: ApiModel;
    public chartFilterDates: SimpleStringString[] = new Array();
    public filterDateValue: string = '7';

    private subParams: any;
    private apiId: string;

    constructor(
        private _apiLogService: ApiLogService,
        private _apiService: ApiService,
        private _route: ActivatedRoute) { }

    ngOnInit() {
        let cacheAutoScale = localStorage.getItem('Chart_AutoScale');
        this.chartAutoScale = cacheAutoScale == "true";
        // set date filter values
        let dateNow = moment();
        this.chartFilterDates = [{
            name: '1d',
            value: '1'
        }, {
            name: '1w',
            value: '7'
        }, {
            name: '1m',
            value: dateNow.diff(moment().subtract('months', 1), 'days').toString()
        }, {
            name: '3m',
            value: dateNow.diff(moment().subtract('months', 3), 'days').toString()
        }, {
            name: '6m',
            value: dateNow.diff(moment().subtract('months', 6), 'days').toString()
        }, {
            name: '1y',
            value: dateNow.diff(moment().subtract('years', 1), 'days').toString()
        }, {
            name: 'all',
            value: '-1'
        }];

        this.subParams = this._route.params.subscribe(params => {
            let id = params['id'];
            if (id) {
                this.apiId = id;
                // get api data
                this._apiService.getApi(id)
                    .subscribe(
                    data => {
                        this.api = data;
                    });

                // get api logs data
                this.getApiLogsData();
            }
        });
    }

    public getChartFilterName(): string {
        let selectedFilter = this.chartFilterDates.find(e => e.value == this.filterDateValue);
        if (selectedFilter)
            return selectedFilter.name;
        return '1w';
    }

    private getApiLogsData() {
        this._apiLogService.getApiLogs(this.apiId, this.filterDateValue == '-1' ? undefined : new Date(moment().subtract('days', parseInt(this.filterDateValue)).toDate()))
            .subscribe(
            data => {
                this.apiLogs = data;
                this.computeChart();
            });
    }

    private computeChart() {
        this.chartDataReady = false;
        this.chartResults = new Array();
        let dataLogs: DataValuesLog[] = new Array();

        for (let i = 0; i < this.apiLogs.length; i++) {
            let locationName = this.apiLogs[i].locationName;
            let logDate = new Date(this.apiLogs[i].logDate).toDateString();

            // group logs by location and date
            let dataLogIndex = dataLogs.findIndex(e => e.location == locationName && e.logDate == logDate);
            if (dataLogIndex == -1) {
                dataLogs.push({
                    location: locationName,
                    logDate: logDate,
                    logValues: [this.apiLogs[i].responseTime]
                });

                let chartSeries: LineChartDataSeriesModel = {
                    name: logDate,
                    value: this.apiLogs[i].responseTime
                };

                let locationIndex = this.chartResults.findIndex(e => e.name == locationName);
                if (locationIndex == -1) {
                    this.chartResults.push({
                        name: this.apiLogs[i].locationName,
                        series: [chartSeries]
                    });
                } else {
                    this.chartResults[locationIndex].series.push(chartSeries);
                }
            } else {
                dataLogs[dataLogIndex].logValues.push(this.apiLogs[i].responseTime);
                let locationIndex = this.chartResults.findIndex(e => e.name == locationName);
                if (locationIndex != -1) {
                    let dateIndex = this.chartResults[locationIndex].series.findIndex(e => e.name == logDate);
                    if (dateIndex != -1) {
                        let oldValues = this.chartResults[locationIndex].series[dateIndex];
                        let sum = 0;
                        for (let j = 0; j < dataLogs[dataLogIndex].logValues.length; j++) {
                            sum += dataLogs[dataLogIndex].logValues[j];
                        }

                        this.chartResults[locationIndex].series[dateIndex] = {
                            name: oldValues.name,
                            max: Math.max(...dataLogs[dataLogIndex].logValues),
                            min: Math.min(...dataLogs[dataLogIndex].logValues),
                            value: Math.round(sum / dataLogs[dataLogIndex].logValues.length)
                        };
                    }
                }
            }
        }
        this.chartDataReady = true;
    }

    public updateAutoScale() {
        localStorage.setItem('Chart_AutoScale', String(this.chartAutoScale));
    }

    ngOnDestroy() {
        this.subParams.unsubscribe();
    }
}