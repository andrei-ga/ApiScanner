import { HttpMethodTypeModel } from '../enums/http.method.type.model';
import { ApiIntervalModel } from '../enums/api.interval.model';
import { ConditionModel } from './condition.model';

export interface ApiModel {
    apiId?: string,
    userId?: string,
    name?: string,
    url?: string,
    method?: HttpMethodTypeModel,
    headers?: string,
    body?: string,
    interval?: ApiIntervalModel,
    enabled?: boolean,
    publicRead?: boolean,
    publicWrite?: boolean,
    conditions: ConditionModel[]
}