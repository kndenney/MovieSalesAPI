import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import 'rxjs';

@Injectable()
export class NotificationService {

  private _notification: BehaviorSubject<string> = new BehaviorSubject(null);
  // readonly notification$: Observable<string> = this._notification.asObservable().pipe(publishReplay().refCount());

  constructor() {}

  notify(message) {
    this._notification.next(message);
    setTimeout(() => this._notification.next(null), 3000);
  }
}
