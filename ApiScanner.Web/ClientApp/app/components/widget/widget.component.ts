import { Component } from '@angular/core';
import { Subscription } from 'rxjs/Subscription';
import { ActivatedRoute } from '@angular/router';
import * as moment from 'moment';
import { TranslateService } from '@ngx-translate/core';

import { WidgetModel } from './widget.model';
import { ApiLogModel } from '../api-log/api-log.model';
import { LineChartDataModel, LineChartDataSeriesModel } from '../chart/line-chart.model';
import { SimpleStringString } from '../shared/models/simple-pairs.model';

import { WidgetService } from './widget.service';
import { ApiLogService } from '../api-log/api-log.service';

interface DataValuesLog {
    apiName: string,
    logDate: string,
    logValues: number[]
}

@Component({
    templateUrl: './widget.component.html'
})
export class WidgetComponent {
    // chart attributes
    public chartShowXAxis: boolean = true;
    public chartShowYAxis: boolean = true;
    public chartGradient: boolean = false;
    public chartShowLegend: boolean = true;
    public chartLegendTitle: string = 'Api name';
    public chartShowXAxisLabel: boolean = true;
    public chartXAxisLabel: string = 'Date';
    public chartShowYAxisLabel: boolean = true;
    public chartYAxisLabel: string = 'Response time (ms)';
    public chartAutoScale: boolean = false;
    public chartResults: LineChartDataModel[] = new Array();
    public chartDataReady: boolean = false;

    public widget: WidgetModel;
    public filterDateValue: string = '7';
    public apiLogs: ApiLogModel[] = new Array();
    public chartFilterDates: SimpleStringString[] = new Array();
    public hideIntervals: boolean = false;

    private widgetId: string;
    private subParams: Subscription;    

    constructor(
        private _apiLogService: ApiLogService,
        private _widgetService: WidgetService,
        private _translate: TranslateService,
        private _route: ActivatedRoute) {
        this._translate.get(['ApiName', 'Date', 'ResponseTime'])
            .subscribe(data => {
                this.chartLegendTitle = data.ApiName;
                this.chartXAxisLabel = data.Date;
                this.chartYAxisLabel = data.ResponseTime;
            });
    }

    ngOnInit() {
        let cacheAutoScale = localStorage.getItem('WidgetChart_AutoScale');
        this.chartAutoScale = cacheAutoScale == "true";
        let cacheHideIntervals = localStorage.getItem('WidgetChart_HideIntervals');
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
                this.widgetId = id;
                // get api data
                this._widgetService.getWidget(id)
                    .subscribe(
                    data => {
                        this.widget = data;
                    },
                    error => { });

                // get api logs data
                this.getWidgetLogsData();
            }
        });
    }

    private getWidgetLogsData() {
        this._apiLogService.getWidgetLogs(this.widgetId, this.filterDateValue == '-1' ? undefined : new Date(moment().subtract('days', parseInt(this.filterDateValue)).toDate()))
            .subscribe(
            data => {
                this.apiLogs = data;
                this.computeChart();
            });
    }

    public getChartFilterName(): string {
        let selectedFilter = this.chartFilterDates.find(e => e.value == this.filterDateValue);
        if (selectedFilter)
            return selectedFilter.name;
        return '1w';
    }

    private computeChart() {
        this.chartDataReady = false;
        this.chartResults = new Array();
        let dataLogs: DataValuesLog[] = new Array();

        for (let i = 0; i < this.apiLogs.length; i++) {
            let apiName = this.apiLogs[i].apiName;
            let logDate = new Date(this.apiLogs[i].logDate).toDateString();

            // group logs by api and date
            let dataLogIndex = dataLogs.findIndex(e => e.apiName == apiName && e.logDate == logDate);
            if (dataLogIndex == -1) {
                dataLogs.push({
                    apiName: apiName,
                    logDate: logDate,
                    logValues: [this.apiLogs[i].responseTime]
                });

                let chartSeries: LineChartDataSeriesModel = {
                    name: logDate,
                    value: this.apiLogs[i].responseTime
                };

                let apiIndex = this.chartResults.findIndex(e => e.name == apiName);
                if (apiIndex == -1) {
                    this.chartResults.push({
                        name: this.apiLogs[i].apiName,
                        series: [chartSeries]
                    });
                } else {
                    this.chartResults[apiIndex].series.push(chartSeries);
                }
            } else {
                dataLogs[dataLogIndex].logValues.push(this.apiLogs[i].responseTime);
                let apiIndex = this.chartResults.findIndex(e => e.name == apiName);
                if (apiIndex != -1) {
                    let dateIndex = this.chartResults[apiIndex].series.findIndex(e => e.name == logDate);
                    if (dateIndex != -1) {
                        let oldValues = this.chartResults[apiIndex].series[dateIndex];
                        let sum = 0;
                        for (let j = 0; j < dataLogs[dataLogIndex].logValues.length; j++) {
                            sum += dataLogs[dataLogIndex].logValues[j];
                        }

                        this.chartResults[apiIndex].series[dateIndex] = {
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
        localStorage.setItem('WidgetChart_AutoScale', String(this.chartAutoScale));
    }

    public updateHideIntervals() {
        localStorage.setItem('WidgetChart_HideIntervals', String(this.hideIntervals));
        this.computeChart();
    }

    ngOnDestroy() {
        this.subParams.unsubscribe();
    }
}