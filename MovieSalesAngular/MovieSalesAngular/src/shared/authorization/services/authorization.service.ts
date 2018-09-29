import {Injectable} from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { map } from 'rxjs/operators';
import { Observable, Subject, asapScheduler, pipe, of, from, interval, merge, fromEvent } from 'rxjs';
import { TokenRequest } from '../models/tokenrequest';
import { TokenResponse, TokenResponses } from '../models/tokenresponse';

const httpOptions = {
    headers: new HttpHeaders({ 'Content-Type': 'application/json' })
};
 
@Injectable()
export class AuthorizationService {

    // https://medium.com/@ryanchenkie_40935/angular-authentication-using-the-http-client-and-http-interceptors-2f9d1540eb8
    // http://jasonwatmore.com/post/2016/08/16/angular-2-jwt-authentication-example-tutorial
    // https://medium.com/@amcdnl/authentication-in-angular-jwt-c1067495c5e0

    public _tokenRequest: TokenRequest;
    public _tokenResponse: TokenResponses;

    constructor(
        private http: HttpClient,
        private tokenRequest: TokenRequest,
        private tokenResponse: TokenResponses ) {
        this._tokenRequest = tokenRequest;
        this._tokenResponse = tokenResponse;
    }

    login(username: string, password: string) {

        this._tokenRequest.Username = username;
        this._tokenRequest.Password = password;

        const requestOption = {
            params: new HttpParams(),
            headers: new HttpHeaders()
        };

        requestOption.headers = new HttpHeaders().set('Content-Type', 'application/x-www-form-urlencoded');
        const baseUrl = 'https://localhost:44368/';

        return this.http.post<any>(baseUrl + 'users/token', this._tokenRequest)
            .pipe(map(user => {

                const response = <TokenResponses> user;

                // login successful if there's a jwt token in the response
                if (response && (response.data[0].token != null)) {
                    // store user details and jwt token in local storage to keep user logged in between page refreshes
                    localStorage.setItem('currentUserToken', JSON.stringify(response.data[0].token));
                    localStorage.setItem('currentUserExpiration', JSON.stringify(response.data[0].expiration));
                }

                return response;
            }));
    }

    logout() {
        // remove user from local storage to log user out
        localStorage.removeItem('currentUserToken');
    }

    getToken() {
        return localStorage.getItem('currentUserToken');
    }

    isTokenExpired(token?: string): boolean {

        if (!token) {
            token = this.getToken();
        }

        if (!token) {
            return true;
        }

        // Retrieve the users JWT token expiration time
        // and check to see if the date JWT expires is greater
        // than the current time
        const date = new Date(localStorage.getItem('currentUserExpiration')); // this.getTokenExpirationDate(token);
        if (date === undefined) {
            return false;
        }
        return !(date.valueOf() > new Date().valueOf());
    }
}
