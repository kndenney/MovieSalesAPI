// errors.service.ts
import { ErrorHandler, Injectable, Injector} from '@angular/core';
import { Location, LocationStrategy, PathLocationStrategy } from '@angular/common';
import { HttpErrorResponse } from '@angular/common/http';
import { Observable, of, Subject } from 'rxjs';

// Cool library to deal with errors: https://www.stacktracejs.com
// import * as StackTraceParser from 'error-stack-parser';

@Injectable()
export class ErrorsService {
constructor(
    private injector: Injector,
  ) { }

public dataFromError: any;

log(error): Observable<any> {
    // Send error to server so fill server error properties
    const errorToSend = this.addContextInfo(error);
    // As long as this service is singleton this
    // value will hold across components and other parts
    // of the application
    // *Also - create a separete error structure
    // for our client app here which is adifferent
    // *we may not need to send things to the client like
    // stacktrace and other details - make it more generic
    // for the client so you don't leak errors visibly to the
    // user (they don't want or care)
    //*** I am thinking somehow... if the API errors out
    //from the server then we need to somehow retrieve the error
    //details from the server - the API call return data
    //and format that for display to teh end user
    //or maybe ... the server error button
    //and code will guide us as to what to do there */
    this.dataFromError = errorToSend;
    return fakeHttpService.post(errorToSend);
}

addContextInfo(error) {
    // All the context details that you want (usually coming from other services; Constants, UserService...)
    const name = error.name || null;
    const appId = 'this-could-come-from-internal-config-file';
    const user = 'this-could-come-from-jwt-token';
    const time = new Date().getTime();
    const id = `${appId}-${user}-${time}`;
    const location = this.injector.get(LocationStrategy);
    const url = location instanceof PathLocationStrategy ? location.path() : '';
    const status = error.status || null;
    const message = error.message || error.toString();
    const stack = error instanceof HttpErrorResponse ? null : error;
    const errorToSend = {name, appId, user, time, id, url, status, message, stack};
    return errorToSend;
  }
}

class fakeHttpService {
  static post(error): Observable<any> {
    console.log('Error sent to the server: ', error);
    return of(error); // Observable.of(error);
  }
}