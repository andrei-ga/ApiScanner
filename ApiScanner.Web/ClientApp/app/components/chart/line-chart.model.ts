export interface LineChartDataModel {
    name: string,
    series: LineChartDataSeriesModel[]
}

export interface LineChartDataSeriesModel {
    name: string,
    value: any,
    min?: any,
    max?: any
}