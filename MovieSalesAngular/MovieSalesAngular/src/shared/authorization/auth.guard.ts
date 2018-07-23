import { Injectable } from '@angular/core';
import { Router, CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
import { AuthorizationService } from './services/authorization.service';

/*

BY USING AUTH-GUARD WE CAN CHECK TO MAKE SURE THE JWT TOKEN IS IMPLEMENTED ON A PER COMPONENT
BASIS BY IMPLEMENTING AUTH-GUARD JWT CHECKS ON THE COMPONENTS WE KNOW
NEED JWT CHECKS ON THEM

*/

@Injectable()
export class AuthGuard implements CanActivate {

    constructor(private router: Router, private authorizationService: AuthorizationService) { }

    canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot) {
        if (localStorage.getItem('currentUserToken')) {

            // Check to see if the token is expired
            if (!this.authorizationService.isTokenExpired()) {
                return false; // Returning false means that the route cannot be activated or that you cannot access that route
            } else {
                return true; // Means you can go to the route since the token is not expired
            }
        }

        // not logged in so redirect to login page
        this.router.navigate(['/login'], { queryParams: { returnUrl: state.url }});
        return false;
    }
}
