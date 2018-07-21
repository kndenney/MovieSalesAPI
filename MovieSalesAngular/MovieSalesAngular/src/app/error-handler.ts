import { Injectable } from '@angular/core';
import { MatSnackBar } from '@angular/material';

@Injectable()
export class ErrorHandlers {

  constructor(
    public snackbar: MatSnackBar,
  ) {}

  public handleError(err: any, status?: Number) {
    // Send error to database

    // Figure out what kind of error you now have
    // and format your message accordingly
    // and how you want to display under what conditions you want to display
    // what etc.

//We need to figure out when we want to redirect to an error page:

//maybe under the 500 error scenarios.... but others can be snackbar?
//or if the status comes back 0 (unkonwn or something like that)
//then redirect?

    let message: string;

    switch (status) {
        case 200:
            message = 'OK';
            break;
        case 204:
            message = 'No Content';
            break;
        case 301:
            message = 'Moved Permanently';
            break;
        case 304:
            message = 'Not Modified';
            break;
        case 307:
            message = 'Temporary Redirect';
            break;
        case 308:
            message = 'Permanent Redirect';
            break;
        case 400:
            message = 'Bad Request';
            break;
        case 401:
            message = 'Unauthorized Access';
            break;
        case 403:
            message = 'Forbidden';
            break;
        case 404:
            message = 'Resource Not Found';
            break;
        case 405:
            message = 'Method Not Allowed';
            break;
        case 406:
            message = 'Not Acceptable';
            break;
        case 413:
            message = 'Request Entity Too Large';
            break;
        case 414:
            message = 'Request-URI Too Long';
            break;
        case 500:
            message = 'Internal Server Error';
            break;
        case 501:
            message = 'Internal Server Error';
            break;
        case 502:
            message = 'Internal Server Error';
            break;
        case 503:
            message = 'Internal Server Error';
            break;
        case 504:
            message = 'Gateway Timeout';
            break;
        default:
            message = 'Unexpected Error. Please try again';
            break;
    }

    this.snackbar.open(message, 'close');
  }
}
