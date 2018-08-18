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

@Injectable()
export class ErrorInterceptor implements HttpInterceptor {
  constructor(
    public errorHandler: ErrorHandlers,
  ) {}

  intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {

    // Go here for an example:
    // https://hackernoon.com/global-http-error-catching-in-angular-4-3-9e15cc1e0a6b
    // It is older but this below code is the new RxJs 6 example for Angular 6

    return next.handle(request).pipe(tap((event: HttpEvent<any>) => {}, (err: any) => {
      if (!navigator.onLine) {
        // No Internet connection
        this.errorHandler.handleError(err, 999);
        return;
      }

      // HttpErrorResponse - error from a service call
      if (err instanceof HttpErrorResponse) {
        // do error handling here
        this.errorHandler.handleError(err, err.status);
        return;
      } else {
        // Internal Angular error such as:
        this.errorHandler.handleError(err, 500);
      }
    }));
  }
}
