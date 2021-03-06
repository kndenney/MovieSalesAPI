import { Injectable } from '@angular/core';
import { HttpRequest, HttpHandler, HttpEvent, HttpInterceptor } from '@angular/common/http';
import { Observable, Subject, asapScheduler, pipe, of, from, interval, merge, fromEvent } from 'rxjs';

@Injectable()
export class TokenInterceptor implements HttpInterceptor {
    intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
        // add authorization header with jwt token if available
        // if no token is available then that means for this application either
        //1. they are hitting the login page
        //2. they are hitting the app for the first time
        //3. their localStorage cache is cleared
        //4. they are not authorized
        const currentUser = JSON.parse(localStorage.getItem('currentUserToken'));
        if (currentUser) {
            request = request.clone({
                setHeaders: {
                    Authorization: `Bearer ${currentUser}`
                }
            });
        }
 
        return next.handle(request);
    }
}