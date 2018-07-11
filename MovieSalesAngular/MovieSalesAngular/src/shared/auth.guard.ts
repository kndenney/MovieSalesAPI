import { Injectable } from '@angular/core';
import { Router, CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
 
@Injectable()
export class AuthGuard implements CanActivate {
 
    constructor(private router: Router) { }
 
    canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot) {
        if (localStorage.getItem('currentUser')) {

            //Here we take the 'currentUser' and send it via an API call back to our Web API code
            //to an Auth controller and it checks whether or not we get a 404 from the [Authorize] header
            //call - if we get back a 200 OK then we are ok to continue logging in
            //if it comes back with anything other than an OK message we know its a bust and either
            //1. An expired token so redirect them to the login page
            //2. a bad token so they shouldn't have access redirect them to login page
            //3. fake token or empty string token so redirect them to login page
            // logged in so return true
            return true;
        }
 
        // not logged in so redirect to login page
        this.router.navigate(['/login'], { queryParams: { returnUrl: state.url }});
        return false;
    }
}