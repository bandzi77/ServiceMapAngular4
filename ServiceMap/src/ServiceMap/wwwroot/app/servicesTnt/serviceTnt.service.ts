import { Injectable } from '@angular/core';
import { IServiceFilter, IServiceTntResult } from './serviceTnt';
import { IPageInfo } from '../pagination/page';
import { IDepotDetailsFilter, IDepotDetailsResult } from './depotDetails';
import { Http, Response, URLSearchParams } from '@angular/http';
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/do';
import 'rxjs/add/operator/catch';
import 'rxjs/add/observable/throw';


@Injectable()
export class ServicesTntService {
    private _serchServicesUrl = 'api/servicesTnt/GetServices';
    private _getDepotDetails = 'api/servicesTnt/GetDepotDetails';
    constructor(private _http: Http) {
    };

    getDepotDetails(depotFilter: IDepotDetailsFilter): Observable<IDepotDetailsResult> {
        let searchParams = new URLSearchParams();

        searchParams.set('depotCode', depotFilter.depotCode);

        return this._http.get(this._getDepotDetails, { search: searchParams })
            .map(this.extractData)
            .do(data => console.log('All' + JSON.stringify(data)))
            .catch(this.handleError);
    }

    searchServicesTnt(serviceFilter: IServiceFilter, pageInfo: IPageInfo): Observable<IServiceTntResult> {
        let searchParams = new URLSearchParams();

        searchParams.set('postCode', serviceFilter.postCode);
        searchParams.set('cityName', serviceFilter.cityName);
        searchParams.set('orderBy', pageInfo.order_by);
        searchParams.set('currentPage', '' + pageInfo.current_page);
        searchParams.set('pageSize', '' + pageInfo.page_size);

        return this._http.get(this._serchServicesUrl, { search: searchParams })
            .map(this.extractData)
            .do(data => console.log('All' + JSON.stringify(data)))
            .catch(this.handleError);
    }

    // Działa
    // private handleError(error: Response) {
    //    console.error(error);
    //    return Observable.throw(error.json().error || 'Server error');
    // } 

    private extractData(response: Response) {
        let data = response.json();
        return data || {};
    }

    private handleError(error: Response | any) {
        // In a real world app, we might use a remote logging infrastructure
        let errMsg: string;
        let err: any;
        if (error instanceof Response) {
            errMsg = `${error.status} - ${error.statusText || ''}`;

            if (error.status === 401) {
                location.reload();
                return Observable.throw(errMsg);
            }

            try {
                const body = error.json() || '';
                err = body.error || JSON.stringify(body);
            }
            catch (e){
                err = ' More info: ' + e.stack;
            }
            finally {
                errMsg = errMsg + `${err || 'brak'}`;
            }
        } else {
            errMsg = error.message ? error.message : error.toString();
        }

        console.error(errMsg);
        return Observable.throw(errMsg);
    }
}
