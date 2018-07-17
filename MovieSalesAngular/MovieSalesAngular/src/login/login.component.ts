import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';

import { AuthorizationService } from '../shared/authorization/services/authorization.service';
 
@Component({
    templateUrl: 'login.component.html'
})
 
export class LoginComponent implements OnInit {
    model: any = {};
    loading = false;
    returnUrl: string;
    error = '';
 
    constructor(
        private route: ActivatedRoute,
        private router: Router,
        private authorizationService: AuthorizationService) { }
 
    ngOnInit() {
        // reset login status
        this.authorizationService.logout();
 
        // get return url from route parameters or default to '/'
        this.returnUrl = this.route.snapshot.queryParams['returnUrl'] || '/';
    }
 
    login() {
        this.loading = true;
        this.authorizationService.login(this.model.username, this.model.password)
            .subscribe(
                data => {
                    alert('test');
                    console.log(data);
                    this.router.navigate([this.returnUrl]);
                },
                error => {
                    this.error = error;
                    this.loading = false;
                });
    }
}