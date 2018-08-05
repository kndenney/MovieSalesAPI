import { Component, OnInit, NgZone } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { MatSnackBar } from '@angular/material';
import { CreateAccountService } from './services/create-account.services';

@Component({
    templateUrl: 'create-account.component.html',
    styleUrls: ['create-account.component.css']
})

export class CreateAccountComponent implements OnInit {
    model: any = {};
    loading = false;
    returnUrl: string;
    error = '';

    constructor(
        private route: ActivatedRoute,
        private router: Router,
        private createAccountService: CreateAccountService,
        private zone: NgZone,
        public snackbar: MatSnackBar
    ) { }

    ngOnInit() {
        // get return url from route parameters or default to '/'
        this.returnUrl = this.route.snapshot.queryParams['returnUrl'] || '/';
    }

    createAccount() {
        // let index = 0; //This would be some index of some database call or something like that this.items.indexOf(item);
        this.loading = true;

        localStorage.setItem('username', this.model.username);

        this.createAccountService.createUserAccount(this.model.username, this.model.password)
            .subscribe(
                response => {

                    if (response.data[0].token == null) {
                        // Invalid account creation - let the user know
                        this.snackbar.open
                        (
                            'Account not created. Please try again.', '',
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
                            'Account Created!', '',
                            {
                                duration: 500,
                                panelClass: ['green-snackbar']
                            },
                        );

                        this.zone.run(() => this.router.navigate(['/login']));
                    }
                    console.log(response);

                    this.loading = false;
                });

        this.loading = false;
    }
}
