import { Injectable, Inject } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/operator/map';

import { WidgetModel } from './widget.model';

@Injectable()
export class WidgetService {
    constructor(private _http: HttpClient, @Inject('BASE_URL') private _baseUrl: string) { }

    public createWidget(widget: WidgetModel) {
        const headers = new HttpHeaders().set('Content-Type', 'application/json');
        return this._http.post(`${this._baseUrl}/api/widget`, JSON.stringify(widget), { headers: headers });
    }

    public getWidgets(): Observable<WidgetModel[]> {
        return this._http.get<WidgetModel[]>(`${this._baseUrl}/api/widget`);
    }

    public getWidget(widgetId: string): Observable<WidgetModel> {
        return this._http.get<WidgetModel>(`${this._baseUrl}/api/widget/${widgetId}`);
    }

    public canSeeWidget(widgetId: string): Observable<boolean> {
        return this._http.get<boolean>(`${this._baseUrl}/api/widget/${widgetId}/access`);
    }

    public updateWidget(widget: WidgetModel) {
        const headers = new HttpHeaders().set('Content-Type', 'application/json');
        return this._http.put(`${this._baseUrl}/api/widget`, JSON.stringify(widget), { headers: headers });
    }

    public deleteWidget(widgetId: string) {
        return this._http.delete(`${this._baseUrl}/api/widget/${widgetId}`);
    }
}