import {Injectable} from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { map } from 'rxjs/operators';
import { Observable, Subject, asapScheduler, pipe, of, from, interval, merge, fromEvent } from 'rxjs';
import { TokenRequest } from '../models/tokenrequest';
import { TokenResponse } from '../models/tokenresponse';

const httpOptions = {
    headers: new HttpHeaders({ 'Content-Type': 'application/json' })
};
 
@Injectable()
export class AuthorizationService {

    //https://medium.com/@ryanchenkie_40935/angular-authentication-using-the-http-client-and-http-interceptors-2f9d1540eb8
    //http://jasonwatmore.com/post/2016/08/16/angular-2-jwt-authentication-example-tutorial

    public _tokenRequest: TokenRequest;
    public _tokenResponse: TokenResponse;

    constructor(
        private http:HttpClient,
        private tokenRequest: TokenRequest,
        private tokenResponse: TokenResponse ) {
        this._tokenRequest = tokenRequest;
        this._tokenResponse = tokenResponse;
    }
 
    // Uses http.get() to load data from a single API endpoint
    createAuthToken(t: TokenRequest): any {

       let username: string =  t.Username;
       let password: string = t.Password;

        //Onc eyou have the result save it to localStorage
        //or figure out a way to 
        return this.http.post<any>('/api/authorize/create/token', { username: username, password: password }).subscribe((value) => {
            if (value && value.token) {
                localStorage.setItem('currentUser', JSON.stringify(value));
            }
            return value;
        });
    }

    login(username: string, password: string) {

        this._tokenRequest.Username = username;
        this._tokenRequest.Password = password;

        const requestOption = {
            params: new HttpParams(),
            headers: new HttpHeaders()
        }
  
       // requestOption.params = parameters;
        requestOption.headers = new HttpHeaders().set('Content-Type','application/x-www-form-urlencoded'); //('Content-Type', 'application-json');
        let baseUrl: string = 'https://localhost:44368';

        return this.http.post<any>(baseUrl + '/api/authorize/create/token', this._tokenRequest) //, requestOption) // JSON.stringify(this._tokenRequest), requestOption)
            .pipe(map(user => {

                let response = <TokenResponse> user;
                
                // login successful if there's a jwt token in the response
                if (response && response.Token) {
                    alert(response);
                    // store user details and jwt token in local storage to keep user logged in between page refreshes
                    localStorage.setItem('currentUser', JSON.stringify(user));
                }
 
                return response;
            }));
    }

    //If we were doing some sort of 'get' operation you could use RequestOptions
    //and have parameters in the URL and all of that like 
    //TODO: clean up this below example as we were coding out the login
    /*
     getData(username: string, token: string) {

        this._tokenRequest.Username = username;
        this._tokenRequest.Token = token;

        const requestOption = {
            params: new HttpParams(),
            headers: new HttpHeaders()
        }
  
        requestOption.params = parameters;
        requestOption.headers = new HttpHeaders().set('Content-Type','application/x-www-form-urlencoded'); //('Content-Type', 'application-json');
        let baseUrl: string = 'https://localhost:44368';

        return this.http.get<any>(baseUrl + '/api/movies/get', JSON.stringify(this._tokenRequest), requestOption)
            .pipe(map(user => {

                let response = <TokenResponse> user;

                if (response) {
                    alert(response);
                }
 
                return response;
            }));
    }
 
    */
 
    logout() {
        // remove user from local storage to log user out
        localStorage.removeItem('currentUser');
    }

    getToken() {
        return localStorage.getItem('UserToken');
    }
}