import { ApiWidgetModel } from './api-widget.model';

export interface WidgetModel {
    widgetId?: string,
    userId?: string,
    locationId?: string,
    name?: string,
    publicRead: boolean,
    apiWidgets: ApiWidgetModel[]
}