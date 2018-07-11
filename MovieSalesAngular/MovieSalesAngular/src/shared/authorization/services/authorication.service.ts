import {Injectable} from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable, Subject, asapScheduler, pipe, of, from, interval, merge, fromEvent } from 'rxjs';
import { TokenRequest } from '../models/tokenrequest';

const httpOptions = {
    headers: new HttpHeaders({ 'Content-Type': 'application/json' })
};
 
@Injectable()
export class AuthorizationService {

    //https://medium.com/@ryanchenkie_40935/angular-authentication-using-the-http-client-and-http-interceptors-2f9d1540eb8
    //http://jasonwatmore.com/post/2016/08/16/angular-2-jwt-authentication-example-tutorial

    constructor( private http:HttpClient ) {}
 
    // Uses http.get() to load data from a single API endpoint
    createAuthToken(t: TokenRequest): any {

       let username: string =  t.username;
       let password: string = t.password;

        //Onc eyou have the result save it to localStorage
        //or figure out a way to 
        return this.http.post<any>('/api/food', { username: username, password: password }).subscribe((value) => {
            if (value && value.token) {
                localStorage.setItem('currentUser', JSON.stringify(value));
            }
            return value;
        });
    }
 
    logout() {
        // remove user from local storage to log user out
        localStorage.removeItem('currentUser');
    }

    getToken() {
        return localStorage.getItem('UserToken');
    }
}