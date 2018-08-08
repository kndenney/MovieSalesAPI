import {Injectable} from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { map } from 'rxjs/operators';
import { Observable, Subject, asapScheduler, pipe, of, from, interval, merge, fromEvent } from 'rxjs';
import { TokenRequest } from '../../shared/authorization/models/tokenrequest';
import { environment } from '../../environments/environment.qa';

const httpOptions = {
    headers: new HttpHeaders({ 'Content-Type': 'application/json' })
};

@Injectable()
export class CreateAccountService {

    constructor(
        private http: HttpClient
    ) {}

    public createAccountSubject: Subject<any> = new Subject<any>();

    public createUserAccount(
        username: string,
        password: string
    ): Observable<any> {

        const parameters = new HttpParams();
        parameters.set('@username', username);
        parameters.set('@password', password);


        const requestOption = {
            params: new HttpParams(),
            headers: new HttpHeaders()
        };

        requestOption.headers = new HttpHeaders().set('Content-Type', 'application/json');
        const baseUrl = 'https://localhost:44368/';

        requestOption.params = parameters;

        // The create-account controller does NOTHING to create
        // the JWT because after they create their account
        // Then are then sent to the login screen to login

        alert(username);

        return this.http.post<any>(baseUrl + 'users', requestOption)
            .pipe(map(userAccount => {
                const response = <any> userAccount;
                this.createAccountSubject.next(response.moviename);
                return response;
            }));
    }
}
