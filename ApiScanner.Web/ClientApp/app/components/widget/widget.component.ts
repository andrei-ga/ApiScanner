import { Component, Inject } from '@angular/core';
import { Subscription } from 'rxjs/Subscription';
import { ActivatedRoute } from '@angular/router';
import * as moment from 'moment';
import { MatTableDataSource } from '@angular/material';
import { Location } from '@angular/common';

import { WidgetModel } from './widget.model';
import { ApiLogModel } from '../api-log/api-log.model';
import { LineChartDataModel, LineChartDataSeriesModel } from '../chart/line-chart.model';
import { SimpleStringString } from '../shared/models/simple-pairs.model';

import { WidgetService } from './widget.service';
import { ApiLogService } from '../api-log/api-log.service';
import { PageHeaderService } from '../shared/services/page-header.service';

interface DataValuesLog {
    apiName: string,
    logDate: string,
    logValues: number[]
}

interface DataStats {
    name: string,
    lastDaySuccess: number,
    lastDayFail: number,
    lastWeekSuccess: number,
    lastWeekFail: number
}

@Component({
    templateUrl: './widget.component.html',
    styleUrls: ['./widget.component.css']
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
    public chartFilterDates: SimpleStringString[] = new Array();
    public hideIntervals: boolean = false;
    public statsDataSource: MatTableDataSource<DataStats> = new MatTableDataSource<DataStats>();
    public displayedColumns: string[] = ['name', 'lastDaySuccess', 'lastDayFail', 'lastWeekSuccess', 'lastWeekFail'];
    public embedCode: string;
    public embed: boolean = true;

    private chartApiLogs: ApiLogModel[] = new Array();
    private widgetId: string;
    private subHeader: Subscription;

    constructor(
        @Inject('BASE_URL') private _baseUrl: string,
        private _apiLogService: ApiLogService,
        private _widgetService: WidgetService,
        private _headerService: PageHeaderService,
        private _location: Location,
        private _route: ActivatedRoute) { }

    ngOnInit() {
        let cacheAutoScale = localStorage.getItem('WidgetChart_AutoScale');
        this.chartAutoScale = cacheAutoScale == "true";
        let cacheHideIntervals = localStorage.getItem('WidgetChart_HideIntervals');
        this.hideIntervals = cacheHideIntervals == "true";

        this.subHeader = this._headerService.embed.subscribe(data => {
            this.embed = data;
        });

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
            value: dateNow.diff(moment().subtract(1, 'months'), 'days').toString()
        }, {
            name: '3m',
            value: dateNow.diff(moment().subtract(3, 'months'), 'days').toString()
        }, {
            name: '6m',
            value: dateNow.diff(moment().subtract(6, 'months'), 'days').toString()
        }, {
            name: '1y',
            value: dateNow.diff(moment().subtract(1, 'years'), 'days').toString()
        }, {
            name: 'all',
            value: '-1'
        }];

        this._route.params.subscribe(params => {
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
                this.getChartLogsData();
                if (!this.embed)
                    this.getStatsLogsData();
            }
        });
        this.embedCode = `<iframe src="${this._baseUrl.slice(0, -1)}/widgets/${this.widgetId}/embed" width="1000px" height="580px"></iframe>`;
    }

    private getStatsLogsData() {
        this._apiLogService.getWidgetLogs(this.widgetId, true, new Date(moment().subtract(7, 'days').toDate()))
            .subscribe(
            data => {
                let statsData: DataStats[] = new Array();
                let dateNow = moment(new Date());

                for (let i = 0; i < data.length; i++) {
                    let statIndex = statsData.findIndex(e => e.name == data[i].apiName);
                    if (statIndex == -1) {
                        statsData.push({
                            name: data[i].apiName,
                            lastWeekSuccess: 0,
                            lastWeekFail: 0,
                            lastDaySuccess: 0,
                            lastDayFail: 0
                        });
                    }
                }

                for (let i = 0; i < statsData.length; i++) {
                    statsData[i].lastWeekSuccess = data.filter(e => e.success && e.apiName == statsData[i].name).length;
                    statsData[i].lastWeekFail = data.filter(e => !e.success && e.apiName == statsData[i].name).length;
                    statsData[i].lastDaySuccess = data.filter(e => e.success && e.apiName == statsData[i].name && moment.duration(dateNow.diff(moment(e.logDate))).asHours() <= 24).length;
                    statsData[i].lastDayFail = data.filter(e => !e.success && e.apiName == statsData[i].name && moment.duration(dateNow.diff(moment(e.logDate))).asHours() <= 24).length;
                }
                this.statsDataSource.data = statsData;
            });
    }

    private getChartLogsData() {
        this._apiLogService.getWidgetLogs(this.widgetId, false, this.filterDateValue == '-1' ? undefined : new Date(moment().subtract(parseInt(this.filterDateValue), 'days').toDate()))
            .subscribe(
            data => {
                this.chartApiLogs = data;
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

        for (let i = 0; i < this.chartApiLogs.length; i++) {
            let apiName = this.chartApiLogs[i].apiName;
            let logDate = new Date(this.chartApiLogs[i].logDate).toDateString();

            // group logs by api and date
            let dataLogIndex = dataLogs.findIndex(e => e.apiName == apiName && e.logDate == logDate);
            if (dataLogIndex == -1) {
                dataLogs.push({
                    apiName: apiName,
                    logDate: logDate,
                    logValues: [this.chartApiLogs[i].responseTime]
                });

                let chartSeries: LineChartDataSeriesModel = {
                    name: logDate,
                    value: this.chartApiLogs[i].responseTime
                };

                let apiIndex = this.chartResults.findIndex(e => e.name == apiName);
                if (apiIndex == -1) {
                    this.chartResults.push({
                        name: this.chartApiLogs[i].apiName,
                        series: [chartSeries]
                    });
                } else {
                    this.chartResults[apiIndex].series.push(chartSeries);
                }
            } else {
                dataLogs[dataLogIndex].logValues.push(this.chartApiLogs[i].responseTime);
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
        this.subHeader.unsubscribe();
    }
}