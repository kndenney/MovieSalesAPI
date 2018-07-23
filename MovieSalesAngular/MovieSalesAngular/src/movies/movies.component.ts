import { Component, OnInit } from '@angular/core';

@Component({
    templateUrl: 'movies.component.html'
})

export class MoviesComponent implements OnInit {
   // users: User[] = [];

    constructor() { }

    ngOnInit() {
        // get users from secure api end point
        /*this.userService.getAll()
            .first()
            .subscribe(users => {
                this.users = users;
            });*/
    }
}
