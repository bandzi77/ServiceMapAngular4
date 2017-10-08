import { IPage } from '../pagination/page';
import { IResult } from '../shared/common';

export interface IServiceTnt {
    depotCode: string;
    town: string;
    fromPostcode: string;
    toPostcode: string;
    sobota: boolean;
    eX9: boolean;
    eX10: boolean;
    eX12: boolean;
    priority: string;
    wieczorneDostarczenie?: boolean;
    standardDeliveryOd: string;
    standardDeliveryDo: string;
    pickUpDomesticZgl: string;
    dateTimePickUpEksportSmZgl: string;
    samochodZwindaDostepnyWstandardzie?: boolean;
    diplomatNextDay: string;
    serwisMiejski?: boolean;
    serwisPodmiejski?: boolean;
    pickUpDomesticCzas: string;
    pickUpEksportSmCzas: string;
}
export interface IServiceFilter {
    postCode: string;
    cityName: string;
}

export interface IServiceTntResult {
    serviceTnt: IServiceTnt[];
    paging: IPage;
    requestsPerDay: IRequestsPerDay;
    result: IResult;
}

export interface IRequestsPerDay {
    limitOfRequestsPerDay?: number;
    numberOfRequestsPerDay?: number;
}
