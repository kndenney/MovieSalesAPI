import { Component, OnInit, NgZone } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { MatSnackBar } from '@angular/material';
import { CreateAccountService } from './services/create-account.services';
import { FormBuilder, FormGroup, Validators, FormsModule, NgForm, ReactiveFormsModule } from '@angular/forms';

@Component({
    templateUrl: 'create-account.component.html',
    styleUrls: ['create-account.component.css']
})

export class CreateAccountComponent implements OnInit {

    // This is a guide: https://codecraft.tv/courses/angular/forms/model-driven-validation/
    // Material Design Icons: https://material.io/tools/icons/?style=baseline
    // Form options: // https://loiane.com/2017/08/angular-reactive-forms-trigger-validation-on-submit/

    createAccountForm: FormGroup;
    private _formBuilder: FormBuilder;
    submitted = false;
    model: any = {};
    loading = false;
    returnUrl: string;
    error = '';
    successfulSave: boolean;
    errors: string[];

    constructor(
        private route: ActivatedRoute,
        private router: Router,
        private createAccountService: CreateAccountService,
        private zone: NgZone,
        public snackbar: MatSnackBar,
        private formBuilder: FormBuilder
    ) { 
        this._formBuilder = formBuilder;
    }

    ngOnInit() {
        // get return url from route parameters or default to '/'
        this.returnUrl = this.route.snapshot.queryParams['returnUrl'] || '/';
        this.errors = [];

        // We are using reactive forms in Angular
        // This builds out our form validation - you can also build
        // custom validators if the default ones do not work for you
        this.createAccountForm = this._formBuilder.group({
            username: ['', Validators.required],
            password: ['', [Validators.required, Validators.minLength(4)]],
            firstname: ['', [Validators.required, Validators.maxLength(50)]],
            lastname: ['', [Validators.required, Validators.maxLength(50)]],
            address: this._formBuilder.group({
                street: ['', [Validators.required, Validators.maxLength(255)]],
                city: ['', [Validators.required, Validators.maxLength(255)]],
                state: ['', [Validators.required, Validators.maxLength(2)]],
                zip: ['', [Validators.required, Validators.maxLength(10)]]
              }),
        });
    }

    createAccount() {
        // let index = 0; //This would be some index of some database call or something like that this.items.indexOf(item);
        this.loading = true;
        this.submitted = true;
        console.log(this.createAccountForm.value);

        localStorage.setItem('username', this.model.username);

        this.errors = [];

           // stop here if form is invalid
        if (this.createAccountForm.invalid) {
            return;
        }

        this.createAccountService.createUserAccount(this.model.username, this.model.password)
            .subscribe(
                response => {

                    if (response.data[0].username == null) {
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
                }, error => {
                        this.successfulSave = false;
                        alert(error);
                        /*if (err.status === 400) {
                            // handle validation error
                            let validationErrorDictionary = JSON.parse(err.text());
                            for (var fieldName in validationErrorDictionary) {
                                if (validationErrorDictionary.hasOwnProperty(fieldName)) {
                                    this.errors.push(validationErrorDictionary[fieldName]);
                                }
                            }
                        } else {
                            this.errors.push("something went wrong!");
                        }*/
                    });

        this.loading = false;
    }
}
