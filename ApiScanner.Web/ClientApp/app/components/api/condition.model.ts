import { ConditionTypeModel } from '../enums/condition-type.model';
import { CompareTypeModel } from '../enums/compare-type.model';

export interface ConditionModel {
    conditionId?: string,
    apiId?: string,
    matchType?: ConditionTypeModel,
    matchVar?: string,
    compareType?: CompareTypeModel,
    compareValue?: string,
    shouldPass?: boolean
}