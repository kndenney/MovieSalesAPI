import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { NotificationService } from './services/notification.service';

@Component({
  selector: 'notification',
  templateUrl: './notification.component.html',
  styleUrls: [ './notification.component.css' ]
})
export class NotificationComponent implements OnInit {
  notification: string;
  showNotification: boolean;

  constructor(
    private notificationService: NotificationService,
  ) {}

  ngOnInit() {
    this.notificationService
            .notification$
            .subscribe(message => {
              alert(message);
              this.notification = message;
              this.showNotification = true;
            });
  }
}
