import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor,
  HttpResponse,
  HttpErrorResponse,
} from '@angular/common/http';
import { Observable } from 'rxjs';
import { tap } from 'rxjs/operators';
import { ErrorHandlers } from './error-handler';

// https://hackernoon.com/global-http-error-catching-in-angular-4-3-9e15cc1e0a6b
@Injectable()
export class RequestInterceptor implements HttpInterceptor {
  constructor(
    public errorHandler: ErrorHandlers,
  ) {}

  intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {

    // Go here for an example:
    // https://hackernoon.com/global-http-error-catching-in-angular-4-3-9e15cc1e0a6b
    // It is older but this below code is the new RxJs 6 example for Angular 6
    
    return next.handle(request).pipe(tap((event: HttpEvent<any>) => {}, (err: any) => {
      
      this.errorHandler.handleError(err);

      if (!navigator.onLine) {
        // No Internet connection
        alert('no internet');
        return;
      }

      // HttpErrorResponse - error from a service call
      if (err instanceof HttpErrorResponse) {
        // do error handling here
        alert('test request');
      } else {
        // Internal Angular error such as:
        //
      }
    }));
  }
}
