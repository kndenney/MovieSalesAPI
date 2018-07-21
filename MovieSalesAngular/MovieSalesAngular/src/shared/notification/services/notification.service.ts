import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import 'rxjs';
import { Observable } from 'rxjs';

@Injectable()
export class NotificationService {

  private _notification: BehaviorSubject<any> = new BehaviorSubject(null);
  readonly notification$: Observable<any> = this._notification.asObservable(); // .share(); // (5).refCount();
  // .pipe(publishReplay().refCount());

  constructor() {}

  notify(message) {
    this._notification.next(message);
    setTimeout(() => this._notification.next(null), 3000);
  }
}
