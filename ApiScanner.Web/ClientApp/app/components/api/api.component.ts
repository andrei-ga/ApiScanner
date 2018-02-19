import { Component } from '@angular/core';
import { Subscription } from 'rxjs/Subscription';
import { ActivatedRoute } from '@angular/router';
import { NgxChartsModule } from '@swimlane/ngx-charts';
import { forkJoin } from "rxjs/observable/forkJoin";
import * as moment from 'moment';
import { TranslateService } from '@ngx-translate/core';

import { ApiLogModel } from '../api-log/api-log.model';
import { ApiModel } from '../api/api.model';
import { LineChartDataModel, LineChartDataSeriesModel } from '../chart/line-chart.model';
import { SimpleStringString } from '../shared/models/simple-pairs.model';

import { ApiLogService } from '../api-log/api-log.service';
import { ApiService } from '../api/api.service';

interface DataValuesLog {
    location: string,
    logDate: string,
    logValues: number[]
}

@Component({
    templateUrl: './api.component.html'
})
export class ApiComponent {    
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

    public apiLogs: ApiLogModel[] = new Array();
    public api: ApiModel;
    public chartFilterDates: SimpleStringString[] = new Array();
    public filterDateValue: string = '7';
    public hideIntervals: boolean = false;

    private subParams: Subscription;
    private apiId: string;

    constructor(
        private _apiLogService: ApiLogService,
        private _apiService: ApiService,
        private _translate: TranslateService,
        private _route: ActivatedRoute) {
        this._translate.get(['Location', 'Date', 'ResponseTime'])
            .subscribe(data => {
                this.chartLegendTitle = data.Location;
                this.chartXAxisLabel = data.Date;
                this.chartYAxisLabel = data.ResponseTime;
            });
    }

    ngOnInit() {
        let cacheAutoScale = localStorage.getItem('ApiChart_AutoScale');
        this.chartAutoScale = cacheAutoScale == "true";
        let cacheHideIntervals = localStorage.getItem('ApiChart_HideIntervals');
        this.hideIntervals = cacheHideIntervals == "true";

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
                            max: this.hideIntervals ? undefined : Math.max(...dataLogs[dataLogIndex].logValues),
                            min: this.hideIntervals ? undefined : Math.min(...dataLogs[dataLogIndex].logValues),
                            value: Math.round(sum / dataLogs[dataLogIndex].logValues.length)
                        };
                    }
                }
            }
        }
        this.chartDataReady = true;
    }

    public updateAutoScale() {
        localStorage.setItem('ApiChart_AutoScale', String(this.chartAutoScale));
    }

    public updateHideIntervals() {
        localStorage.setItem('ApiChart_HideIntervals', String(this.hideIntervals));
        this.computeChart();
    }

    ngOnDestroy() {
        this.subParams.unsubscribe();
    }
}