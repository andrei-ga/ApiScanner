import { ApiWidgetModel } from './api-widget.model';
import { LocationModel } from '../location/location.model';

export interface WidgetModel {
    widgetId?: string,
    userId?: string,
    locationId?: string,
    name?: string,
    publicRead: boolean,
    location?: LocationModel,
    apiWidgets: ApiWidgetModel[]
}