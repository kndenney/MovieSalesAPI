import { Injectable } from '@angular/core';
import { Router, CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
import { AuthorizationService } from '../authorization/services/authorization.service';


@Injectable()
export class AuthGuard implements CanActivate {
 
    constructor(private router: Router, private authorizationService: AuthorizationService) { }
 
    canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot) {
        if (localStorage.getItem('currentUser')) {

            //Check to see if the token is expired
            if (!this.authorizationService.isTokenExpired()) {
                return true;
            } else {
                return false;
            }
        }
 
        // not logged in so redirect to login page
        this.router.navigate(['/login'], { queryParams: { returnUrl: state.url }});
        return false;
    }
}