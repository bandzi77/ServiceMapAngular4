import { Component, OnInit, ViewChild, ViewContainerRef } from '@angular/core';
import { IServiceTnt, IServiceFilter, IRequestsPerDay } from './serviceTnt';
import { IDepotDetails, IDepotDetailsFilter } from './depotDetails';
import { IPage, IPageInfo } from '../pagination/page';
import { ActivatedRoute, Router } from '@angular/router';
import { ServicesTntService } from './serviceTnt.service';
import { Subscription } from 'rxjs';
import { LgModalComponent } from '../shared/lgModal.component';
import { IResult } from '../shared/common';
import { LazyLoadEvent, SelectItem, MultiSelect, DataTable } from 'primeng/primeng'
import { ToastrService,IToastrSm } from '../shared/toastr.service';

@Component({
    templateUrl: './serviceTnt-list.component.html',
    styleUrls: ['./serviceTnt-list.component.css']
})

export class ServiceTntListComponent implements OnInit {

    busyIndicator: Subscription;
    pageTitle: string = "Mapa Serwisowa";
    imageWidth: number = 50;
    imageMargin: number = 2;
    listFilter: string;
    postCode: string = '';
    cityName: string = '';
    servicesTnt: IServiceTnt[];
    depotTnt: IDepotDetails[];
    paging: IPage;
    pageInfo: IPageInfo;
    requestsPerDay: IRequestsPerDay;
    errorMessage: string;
    private serviceFilter: IServiceFilter;
    columnOptions: SelectItem[];
    cols: any;
    isInitWindow: boolean = false;
    @ViewChild('lgModal') lgModalRef: LgModalComponent;
    @ViewChild('dataTable') public dataTable: DataTable;
    //TODO -usunąć
    @ViewChild('multiselect') multi: MultiSelect;
    constructor(private _serviceTntService: ServicesTntService,
        private _route: ActivatedRoute,
        private toastService: ToastrService) { }

    ngOnInit(): void {

        let id = +this._route.snapshot.params['currentPage'];
        this.paging = this._getPage();
        this.pageInfo = this._getPageInfo();
        this.requestsPerDay = this._getRequestsPerDay();

        this.cols = [
            { field: 'depotCode', header: 'Kod Depotu' },
            { field: 'town', header: 'Miasto' },
            { field: 'fromPostcode', header: 'Kod pocztowy od' },
            { field: 'toPostcode', header: 'Kod pocztowy do' },

            { field: 'sobota', header: 'Doręczenie sobotnie' },
            { field: 'eX9', header: '9 Express' },
            { field: 'eX10', header: '10 Express' },
            { field: 'eX12', header: '12 Express' },


            { field: 'priority', header: 'Przesyłka priorytetowa' },
            //{ field: 'wieczorneDostarczenie', header: 'Wieczorne dostarczenie' },
            //{ field: 'standardDeliveryOd', header: 'Doręcznia < br > od' },
            //{ field: 'standardDeliveryDo', header: 'Doręcznia < br >do' }

            { field: 'pickUpDomesticZgl', header: 'Zamówienia kuriera krajowego do' },
            { field: 'dateTimePickUpEksportSmZgl', header: 'Zamówienie kuriera międzynarodowego do' },
            //{ field: 'samochodZwindaDostepnyWstandardzie', header: 'ISamochod z winda< br > dostepny w standardzie' },
            { field: 'diplomatNextDay', header: 'Najwcześniejsza dostawa przesyłki pozasystemowej' },
            { field: 'serwisPodmiejski', header: 'Serwis Podmiejski' },
            { field: 'serwisMiejski', header: 'Serwis Miejski ' },
            { field: 'pickUpDomesticCzas', header: 'Minimalny czas na odbiór przesyłki drogowej' },
            { field: 'pickUpEksportSmCzas', header: 'Minimalny czas na odbiór przesyłki lotniczej' }
        ];

        this.columnOptions = [];
        for (let i = 0; i < this.cols.length; i++) {
            this.columnOptions.push({ label: this.cols[i].header, value: this.cols[i] });
        }
    }

    //TODO - usunąć
    private _getPage(): IPage {
        return {
            totalCount: 0,
            pageSize: 25
        };
    }

    private _getPageInfo(): IPageInfo {
        return {
            order_by: null,
            current_page: 3,
            page_size: 25
        };
    }

    private _getRequestsPerDay(): IRequestsPerDay {
        return {
            numberOfRequestsPerDay: null,
            limitOfRequestsPerDay: null
        };
    }

    private _getData(filtr: IServiceFilter) {

        this.busyIndicator = this._serviceTntService.searchServicesTnt(filtr, this.pageInfo)
            .subscribe(result => {
                this.servicesTnt = result.serviceTnt;
                this.paging = result.paging;
                this.requestsPerDay = result.requestsPerDay;
                this.onGetComplete(result.result);
            },
            error => this.errorMessage = <any>error);
    }

    onSearchService() {
        console.log('testowanie init');
        this.dataTable.rows;
        let filtr = this._createServiceFilter();
        this.pageInfo.current_page = 0;
        this._getData(filtr);
        this.dataTable.first = 0;
        this.isInitWindow = true;
    }

    onLazyLoading() {
        let filtr = this._createServiceFilter();
        this._getData(filtr);
    }

    _createServiceFilter(): IServiceFilter {
        let _serviceFilter: IServiceFilter =
            {
                postCode: this.postCode,
                cityName: this.cityName,
            };

        return _serviceFilter;
    }

    onClick(item: any) {
        let filtr: IDepotDetailsFilter = { depotCode: item };
        this.busyIndicator = this._serviceTntService.getDepotDetails(filtr).subscribe(result => {
            this.depotTnt = result.depotDetails;
            this.lgModalRef.show();
        },
            error => this.errorMessage = <any>error);
    }

    loadPageLazy(event: LazyLoadEvent) {
        //in a real application, make a remote request to load data using state metadata from event
        //event.first = First row offset
        //event.rows = Number of rows per page
        //event.sortField = Field name to sort with
        //event.sortOrder = Sort order as number, 1 for asc and -1 for dec
        //filters: FilterMetadata object having field as key and filter value, filter matchMode as value

        //imitate db connection over a network
        //setTimeout(() => {
        //    if (this.datasource) {
        //        this.cars = this.datasource.slice(event.first, (event.first + event.rows));
        //    }
        //}, 250);

        if (this.isInitWindow) {
            this.pageInfo.order_by = this._setOrderBy(event.sortField, event.sortOrder);
            this.pageInfo.current_page = (event.first / event.rows);
            this.pageInfo.page_size = event.rows;
            this.onLazyLoading();
        }
    }

    _setOrderBy(sortField?: string, sortOrder?: number): string {
        let result = null;
        if (sortField == undefined || sortField == null) {
            return result;
        }

        result = sortField;

        if (sortOrder == undefined || sortOrder == null) {
            return result;
        }

        return result = result + " " + ((sortOrder === -1) ? 'desc' : 'asc');
    }

    private onGetComplete(res: IResult): void {
        if (!res.success) {
            this.toastService.error(<IToastrSm>{
                message: res.message
            });
        }
    }
}