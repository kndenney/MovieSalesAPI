import { Component, OnInit, NgZone } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';

import { AuthorizationService } from '../shared/authorization/services/authorization.service';
import { MatSnackBar } from '@angular/material';

@Component({
    templateUrl: 'login.component.html',
    styleUrls: ['login.component.css']
})

export class LoginComponent implements OnInit {
    model: any = {};
    loading = false;
    returnUrl: string;
    error = '';

    constructor(
        private route: ActivatedRoute,
        private router: Router,
        private authorizationService: AuthorizationService,
        private zone: NgZone,
        public snackbar: MatSnackBar
    ) { }

    ngOnInit() {
        // reset login status
        this.authorizationService.logout();

        // get return url from route parameters or default to '/'
        this.returnUrl = this.route.snapshot.queryParams['returnUrl'] || '/';
    }
 
    login() {
        //let index = 0; //This would be some index of some database call or something like that this.items.indexOf(item);
        this.loading = true;
        this.authorizationService.login(this.model.username, this.model.password)
            .subscribe(
                response => {

                    if (response.data[0].token == null) {
                        // Invalid login - let the user know
                        this.snackbar.open
                        (
                            'Invalid username or password. Please try again.', '',
                            {
                                duration: 2000,
                                panelClass: ['red-snackbar']
                            },
                        );
                    } else {
                        // Valid login - localStorage should have a currentUser
                        // and that should have the token for future request usage
                        this.snackbar.open
                        (
                            'Hello!', '',
                            {
                                duration: 500,
                                panelClass: ['green-snackbar']
                            },
                        );

                        this.zone.run(() => this.router.navigate(['/movies']));
                    }
                    console.log(response);

                    this.loading = false;
                });
    }
}
