import { Injectable, Inject } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/operator/map';

import { LocationModel } from './location.model';

@Injectable()
export class LocationService {
    constructor(private _http: HttpClient, @Inject('BASE_URL') private _baseUrl: string) { }

    public getLocations(): Observable<LocationModel[]> {
        return this._http.get<LocationModel[]>(`${this._baseUrl}/api/location`);
    }
}