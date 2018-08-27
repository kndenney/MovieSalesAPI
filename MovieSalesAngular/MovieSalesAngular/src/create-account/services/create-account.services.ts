import {Injectable} from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { map } from 'rxjs/operators';
import { Observable, Subject, asapScheduler, pipe, of, from, interval, merge, fromEvent } from 'rxjs';
import { TokenRequest } from '../../shared/authorization/models/tokenrequest';
import { environment } from '../../environments/environment.qa';
import { User } from '../models/user';

const httpOptions = {
    headers: new HttpHeaders({ 'Content-Type': 'application/json' })
};

@Injectable()
export class CreateAccountService {

    public _user: User;
    
    constructor(
        private http: HttpClient,
        private user: User
    ) {
        this._user = user;
    }

    public createAccountSubject: Subject<any> = new Subject<any>();

    public createUserAccount(
        username: string,
        password: string
    ): Observable<any> {

        this._user.Username = username;
        this._user.Password = password;

        const parameters = new HttpParams();
        parameters.set('@username', username);
        parameters.set('@password', password);


        const requestOption = {
            params: new HttpParams(),
            headers: new HttpHeaders()
        };

        requestOption.headers = new HttpHeaders().set('Content-Type', 'application/json');
        const baseUrl = 'http://localhost:5000/';

        requestOption.params = parameters;

        // The create-account controller does NOTHING to create
        // the JWT because after they create their account
        // Then are then sent to the login screen to login

        return this.http.post<User>(baseUrl + 'users', this._user)
            .pipe(map(userAccount => {
                const response = <User> userAccount;
                this.createAccountSubject.next(response.Username);
                return response;
            }));
    }
}
