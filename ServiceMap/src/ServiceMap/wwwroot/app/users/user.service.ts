import { Injectable } from '@angular/core';
import { Http, Response, Headers, RequestOptions, URLSearchParams } from '@angular/http';
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/operator/do';
import 'rxjs/add/operator/catch';
import 'rxjs/add/observable/throw';
import 'rxjs/add/operator/map';
import 'rxjs/add/observable/of';
import { IUser, IUserFilter, IUserResult } from './user';
import { IResult } from '../shared/common';

@Injectable()
export class UserService {
    private baseUrl = 'api/users';
    private getUsersUrl = 'api/users/GetUsers';

    constructor(private http: Http) { }

    getUsers(userFilter: IUserFilter): Observable<IUserResult> {
        let searchParams = new URLSearchParams();
        searchParams.set('email', userFilter.email);
        searchParams.set('showLockedOnly', String(userFilter.showLockedOnly));

        return this.http.get(this.getUsersUrl, { search: searchParams })
            .map(this._extractData)
            .do(data => console.log('getUsers' + JSON.stringify(data)))
            .catch(this._handleError);
    }

    getUser(user: IUser): Observable<IUser> {
        if (user._id === '0') {
            return Observable.of(this.initializeUser());
            // return Observable.create((observer: any) => {
            //     observer.next(this.initializeProduct());
            //     observer.complete();
            // });
        };
        return Observable.of(user);
    }

    deleteUser(_id: string): Observable<IResult> {
        let headers = new Headers({ 'Content-Type': 'application/json' });
        let options = new RequestOptions({ headers: headers });

        const url = `${this.baseUrl}/${_id}`;
        return this.http.delete(url, options)
            .map(this._extractData)
            .do(data => console.log('deleteUser: ' + JSON.stringify(data)))
            .catch(this._handleError);
    }

    saveUser(user: IUser): Observable<IResult> {
        let headers = new Headers({ 'Content-Type': 'application/json' });
        let options = new RequestOptions({ headers: headers });

        if (user._id === "0") {
            return this._addUser(user, options);
        }
        return this._updateUser(user, options);
    }

    private _addUser(user: IUser, options: RequestOptions): Observable<IResult> {
        return this.http.post(this.baseUrl, user, options)
            .map(this._extractData)
            .do(data => console.log('addUser: ' + JSON.stringify(data)))
            .catch(this._handleError);
    }

    private _updateUser(user: IUser, options: RequestOptions): Observable<IResult> {
        const url = `${this.baseUrl}/${user._id}`;
        return this.http.put(url, user, options)
            .map(this._extractData)
            .do(data => console.log('updateUser: ' + JSON.stringify(data)))
            .catch(this._handleError);
    }

    private _extractData(response: Response) {
        let body = response.json();
        return body || {};
    }

    private _handleError(error: Response | any) {
        // In a real world app, we might use a remote logging infrastructure
        let errMsg: string;
        let err: any;
        if (error instanceof Response) {
            errMsg = `${error.status} - ${error.statusText || ''}`;

            // Wylogowanie użytkownika poprzez odświeżenie strony- do dalszej analizy
            if (error.status === 401) {
                location.reload();
                return Observable.throw(errMsg);
            }

            try {
                const body = error.json() || '';
                err = body.error || JSON.stringify(body);
            }
            catch (e) {
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


    initializeUser(): IUser {
        // Return an initialized object
        return {
            _id: '0',
            tntUserName: null,
            email: null,
            password: null,
            limitOfRequestsPerDay: null,
            numberOfRequestsPerDay: null,
            isSuperUser: false,
            isLocked: false
        };
    }

   //private _handleError(error: Response): Observable<any> {
    //         if (error.status === 401) {
    //        location.reload();
    //    }
    //    console.error(error);
    //    return Observable.throw(error.json().error || 'Server error');
    //}
}
