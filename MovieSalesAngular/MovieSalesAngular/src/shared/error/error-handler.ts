// errors-handler.ts
//See - https://medium.com/@aleixsuau/error-handling-angular-859d529fa53a
//https://medium.com/@aleixsuau/error-handling-angular-859d529fa53a
import { Router } from '@angular/router';
import { ErrorHandler, Injectable, Injector} from '@angular/core';
import { HttpErrorResponse } from '@angular/common/http';
import { Error, Errors } from './models/error';
import { ErrorsService } from '../error/services/error.service';
import { NotificationService } from '../error/services/notification.service';

@Injectable()
export class ErrorsHandler implements ErrorHandler {
//This is what we want to do
//For brevity sake:
//https://medium.com/@aleixsuau/error-handling-angular-859d529fa53a

constructor(
    // Because the ErrorHandler is created before the providers, we’ll have to use the Injector to get them.
    private injector: Injector,
) { }

    public handleError(error: Error | HttpErrorResponse) {
        
        const notificationService = this.injector.get(NotificationService);
        const errorsService = this.injector.get(ErrorsService);
        const router = this.injector.get(Router);

        let err = new Errors();
        err.data = error.error;
      

        if (error instanceof HttpErrorResponse) {

            err.message[0].Code = error.status.toString();
            err.message[0].Message = error.statusText;
            err.message[0].Path = error.url;

            // Server or connection error happened
            if (!navigator.onLine) {
                // Handle offline error
                //Perhaps redirect to some generic
                //Not online page?
                return notificationService.notify('No Internet Connection');
            } else {

            // Handle Http Error (error.status === 403, 404...)
             /*   let httpErrorCode = error.status;
                switch (httpErrorCode) {
                case UNAUTHORIZED:
                    this.router.navigateByUrl("/login");
                    break;
                case FORBIDDEN:
                    this.router.navigateByUrl("/unauthorized");
                    break;
                case BAD_REQUEST:
                    this.showError(error.message);
                    break;
                default:
                    this.showError(REFRESH_PAGE_ON_TOAST_CLICK_MESSAGE);
                }*/

                 // Http Error
      // Send the error to the server
                errorsService
                    .log(error)
                    .subscribe();

                return notificationService.notify(`${error.status} - ${error.message}`);
            }
        } else {
            // Client Error Happened
            // Send the error to the server and then
            // redirect the user to the page with all the info
            errorsService
                .log(error)
                .subscribe(errorWithContextInfo => {
                    router.navigate(['/error'], { queryParams: errorWithContextInfo });
                });
        }
      }

      private showError(message:string){
      /*  this.toastManager.error(message, DEFAULT_ERROR_TITLE, { dismiss: 'controlled'}).then((toast:Toast)=>{
                let currentToastId:number = toast.id;
                this.toastManager.onClickToast().subscribe(clickedToast => {
                    if (clickedToast.id === currentToastId) {
                        this.toastManager.dismissToast(toast);
                        window.location.reload();
                    }
                });
            });*/
      }
}
