import { Component, OnInit } from '@angular/core';
import { Observable } from '../../node_modules/rxjs';
import { HttpParams, HttpHeaders, HttpClient } from '../../node_modules/@angular/common/http';
import { map } from '../../node_modules/rxjs/operators';
import { MoviesService } from './services/movies.service';
import { UsersMovies } from './models/users-movies';

@Component({
    templateUrl: 'movies.component.html'
})

export class MoviesComponent implements OnInit {
   // users: User[] = [];

   constructor(
    private http: HttpClient,
    private moviesService: MoviesService
   ) {}

   public usersMovies: UsersMovies[];

    ngOnInit() {

        this.moviesService.retrieveAllMoviesIncludingUsers(
            localStorage.getItem('username')
        ).subscribe(results => {
            this.usersMovies = results;
        });
    }


}
