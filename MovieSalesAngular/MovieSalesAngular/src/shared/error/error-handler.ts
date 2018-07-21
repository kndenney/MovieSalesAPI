// errors-handler.ts
// See - https://medium.com/@aleixsuau/error-handling-angular-859d529fa53a
// https://medium.com/@aleixsuau/error-handling-angular-859d529fa53a
import { Router } from '@angular/router';
import { ErrorHandler, Injectable, Injector, NgZone} from '@angular/core';
import { HttpErrorResponse } from '@angular/common/http';
import { ApplicationError, ApplicationErrors } from './models/application-error';
import { ErrorsService } from './services/error.service';
import { NotificationService } from '../notification/services/notification.service';
import { StatusCodes } from './StatusCodes';

@Injectable()
export class ErrorsHandler implements ErrorHandler {
// This is what we want to do
// For brevity sake:
// https://medium.com/@aleixsuau/error-handling-angular-859d529fa53a

constructor(
    // Because the ErrorHandler is created before the providers, we’ll have to use the Injector to get them.
    private injector: Injector,
    private zone: NgZone,
    private statusCodes: StatusCodes
) { }

//https://stackoverflow.com/questions/45623417/angular-doesnt-see-the-injected-service

    public handleError(error: Error | HttpErrorResponse) {
        const notificationService = this.injector.get(NotificationService);
        const errorsService = this.injector.get(ErrorsService);
        const router = this.injector.get(Router);

        const err = new ApplicationErrors();
       
        if (error instanceof HttpErrorResponse) {

            // Server or connection error happened
            if (!navigator.onLine) {
                // Handle offline error
                // Perhaps redirect to some generic
                // Not online page?

                /*errorsService
                .log(error)
                .subscribe(errorWithContextInfo => {
                    this.zone.run(() => router.navigate(['/notonline']));
                });*/

                return notificationService.notify('No Internet Connection');
            } else {

            // Handle Http Error (error.status === 403, 404...)
                const httpErrorCode = error.status;
                switch (httpErrorCode) {
                case 401:
                    this.zone.run(() => router.navigate(['/login']));
                    break;
                case 403:
                    this.zone.run(() => router.navigate(['/login']));
                    break;
                case 500:
                    this.zone.run(() => router.navigate(['/error']));
                    break;
                default:
                    break; // this.zone.run(() => router.navigate(['/error']));
                }

                 // Http Error
      // Send the error to the server
               /* errorsService
                    .log(error)
                    .subscribe(errorWithContextInfo => {
                       // router.navigate(['/error']); // { queryParams: errorWithContextInfo });
                        this.zone.run(() => router.navigate(['/error']));
                    });*/

                return notificationService.notify(`${error.status} - ${error.message}`);
            }
        } else {
            // Client Error Happened
            // Send the error to the server and then
            // redirect the user to the page with all the info
            errorsService
                .log(error)
                .subscribe(errorWithContextInfo => {
                   // router.navigate(['/error'], { queryParams: errorWithContextInfo });
                    this.zone.run(() => router.navigate(['/error']));
                   
                });
        }
      }
}
