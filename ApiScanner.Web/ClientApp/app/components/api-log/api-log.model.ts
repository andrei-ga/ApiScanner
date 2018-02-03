import { ApiModel } from '../api/api.model';
import { LocationModel } from '../location/location.model';

export interface ApiLogModel {
    locationName: string,
    statusCode: number,
    headers: string,
    content: string,
    responseTime: number,
    logDate: string,
    success: boolean
}