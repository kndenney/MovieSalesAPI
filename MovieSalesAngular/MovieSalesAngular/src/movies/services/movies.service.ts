import {Injectable} from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { map } from 'rxjs/operators';
import { Observable, Subject, asapScheduler, pipe, of, from, interval, merge, fromEvent } from 'rxjs';
import { UsersMovies } from '../models/users-movies';
import { TokenRequest } from '../../shared/authorization/models/tokenrequest';
import { Movies } from '../models/movies';
import { environment } from '../../environments/environment.qa';

const httpOptions = {
    headers: new HttpHeaders({ 'Content-Type': 'application/json' })
};

@Injectable()
export class MoviesService {

    constructor(
        private http: HttpClient
    ) {}

    public userMoviesSubject: Subject<any> = new Subject<any>();

    public retrieveAllMoviesIncludingUsers(
        username: string
    ): Observable<any> {

        const parameters = new HttpParams();
        parameters.set('@username', username);

        const requestOption = {
            params: new HttpParams(),
            headers: new HttpHeaders()
        };

        requestOption.headers = new HttpHeaders().set('Content-Type', 'application/json');
        const baseUrl = 'http://localhost:5000/';

        requestOption.params = parameters;

        return this.http.get<UsersMovies>(baseUrl + 'movies/all', requestOption)
            .pipe(map(usersMovies => {
                const response = <UsersMovies> usersMovies;
               // this.userMoviesSubject.next(response.moviename);
                return response;
            }));
    }

    public retrieveAllMovies(): Observable<any> {

        const requestOption = {
            params: new HttpParams(),
            headers: new HttpHeaders()
        };

        requestOption.headers = new HttpHeaders().set('Content-Type', 'application/json');
        const baseUrl = 'http://localhost:5000/api/';

        return this.http.get<Movies>(baseUrl + 'movies', requestOption)
            .pipe(map(movies => {
                return <Movies> movies;
            }));
    }
}
