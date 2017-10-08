export interface IDepotDetails {
        depotCode: string;
        addressesTown: string;
        addressesPostcode: string;
        addressesStreet: string;
        internationalPackageHoursInfo: string;
        domesticPackageHoursInfo: string;
        saturdayPackageHoursInfo: string;
        passportPickupHoursInfo: string;
        weekPackageHoursInfo: string;
}

//export interface IDepotDetails {
//    id: number;
//    depotCode: string;
//    addressesTown: string;
//    addressesStreet: string;
//    exitCustomsOfficeOfficeNumber: string;
//    awkwInfoIsSystemOrDiplomat: string;

//    awkwInfoIsBHPCompliant: boolean;
//    awkwInfoSupportingLocation: string;
//    internationalPackageHoursInfo: string;
//    domesticPackageHoursInfo: string;
//    saturdayPackageHoursInfo: string;

//    saturdayOpsHoursInfo: string;
//    weekPackageHoursInfo: string;
//    customsOfficeOfficeNumber: string;
//    exitCustomsOfficeOfficeDesc: string;
//    customsOfficeOfficeDesc: string;

//    addressesPostcode: string;
//    contactInfo1Phone: string;
//    contactInfo1Extension: string;
//    contactInfo1Description: string;
//    contactInfo2Phone: string;

//    contactInfo2Extension: string;
//    contactInfo2Description: string;
//    contactInfo3Phone: string;
//    contactInfo3Extension: string;
//    contactInfo3Description: string;

//    afterHoursContactInfo1Phone: string;
//    afterHoursContactInfo1Extension: string;
//    afterHoursContactInfo1Description: string;
//    afterHoursContactInfo2Phone: string;
//    afterHoursContactInfo2Extension: string;

//    afterHoursContactInfo2Description: string;
//    afterHoursContactInfo3Phone: string;
//    afterHoursContactInfo3Extension: string;
//    afterHoursContactInfo3Description: string;
//    name: string;

//    samedayUndelCutoffTimeInfo: string;
//}

export interface IDepotDetailsResult {
    depotDetails: IDepotDetails[];
}

export interface IDepotDetailsFilter {
    depotCode: string;
}