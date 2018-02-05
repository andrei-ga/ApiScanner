import { HttpMethodTypeModel } from '../enums/http-method-type.model';
import { ApiIntervalModel } from '../enums/api-interval.model';
import { ConditionModel } from './condition.model';
import { ApiLocationModel } from '../location/api-location.model';
import { AuthorizationTypeModel } from '../enums/authorization-type.model';

export interface ApiModel {
    apiId?: string,
    userId?: string,
    name?: string,
    url?: string,
    method?: HttpMethodTypeModel,
    headers?: string,
    body?: string,
    interval?: ApiIntervalModel,
    authorization?: AuthorizationTypeModel,
    enabled?: boolean,
    publicRead?: boolean,
    publicWrite?: boolean,
    conditions: ConditionModel[],
    apiLocations: ApiLocationModel[]
}