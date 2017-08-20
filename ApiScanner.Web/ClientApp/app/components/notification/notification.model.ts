export class Notification {
    id: string;
    content: string;
    classStyle: string;
    autoClose: boolean;
    closeTimer: number;
}

export enum NotificationClassType {
    success,
    info,
    warning,
    danger
}