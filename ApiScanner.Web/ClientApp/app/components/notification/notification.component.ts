import { Component, OnDestroy } from '@angular/core';
import { Subscription } from 'rxjs/Subscription';

import { Router, NavigationStart, Event } from '@angular/router';
import 'rxjs/add/operator/filter';

import { NotificationDataService } from './notification-data.service';
import { Notification } from './notification.model';

@Component({
    selector: 'notification-panel',
    templateUrl: './notification.component.html',
    styleUrls: ['./notification.component.css']
})
export class NotificationComponent implements OnDestroy {
    public notifications: Notification[] = new Array();
    private _subscribeNotification: Subscription;
    private _subscribeRemoveNotif: Subscription;

    public closeNotification(id: string) {
        let index: number = this.notifications.findIndex(n => n.id == id);
        if (index >= 0)
            this.notifications.splice(index, 1);
    }

    constructor(private _notificationDataService: NotificationDataService,
        private _router: Router) { }

    ngOnInit() {
        this._subscribeNotification = this._notificationDataService.notification.subscribe(
            data => {
                this.notifications.push(data);
                if (data.autoClose) {
                    setInterval(() => this.closeNotification(data.id), data.closeTimer);
                }
            }
        );

        this._subscribeRemoveNotif = this._notificationDataService.removeNotif.subscribe(
            data => {
                this.closeNotification(data);
            }
        );

        // close non auto closing notifications when route changed
        this._router.events.filter(event => event instanceof NavigationStart)
            .subscribe((event: Event) => {
                for (let i = 0; i < this.notifications.length; i++) {
                    if (!this.notifications[i].autoClose) {
                        this.notifications.splice(i, 1);
                        i--;
                    }
                }
            });
    }

    ngOnDestroy() {
        this._subscribeNotification.unsubscribe();
        this._subscribeRemoveNotif.unsubscribe();
    }
}