import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';

import { AuthorizationService } from '../shared/authorization/services/authorization.service';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';

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
        private authorizationService: AuthorizationService) { }
 
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
                data => {
                    alert('test');
                    console.log(data);
                    this.router.navigate([this.returnUrl]);
                }); /*,
                error => {

                    this.error = error;
                    this.loading = false;

                //in case of error, add the callback to bring the item back and re-throw the error.
                    //error.callback=()=>this.items.splice(index, 0, item);
                    //error.callback?error.callback():window.location.reload;
                    throw error;
                });*/
    }
}
