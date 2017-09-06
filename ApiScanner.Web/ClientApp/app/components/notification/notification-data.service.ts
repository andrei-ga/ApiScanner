import { Injectable } from '@angular/core';
import { Subject } from 'rxjs/Subject';

import { Notification, NotificationClassType } from './notification.model';

@Injectable()
export class NotificationDataService {
    public notification = new Subject<Notification>();
    public removeNotif = new Subject<string>();
    private notificationClasses: string[] = ['alert-success', 'alert-info', 'alert-warning', 'alert-danger'];

    public addNotification(content: string, classType: NotificationClassType, autoClose: boolean): string {
        let newNotif: Notification = {
            id: this.generateGuid(),
            content: content,
            classStyle: this.notificationClasses[classType],
            autoClose: autoClose,
            closeTimer: classType == NotificationClassType.danger ? 4000 : 2500
        }
        this.notification.next(newNotif);
        return newNotif.id;
    }

    public removeNotification(id: string) {
        this.removeNotif.next(id);
    }

    private generateGuid(): string {
        var d = new Date().getTime();
        if (typeof performance !== 'undefined' && typeof performance.now === 'function') {
            d += performance.now(); //use high-precision timer if available
        }
        return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function (c) {
            var r = (d + Math.random() * 16) % 16 | 0;
            d = Math.floor(d / 16);
            return (c === 'x' ? r : (r & 0x3 | 0x8)).toString(16);
        });
    }
}