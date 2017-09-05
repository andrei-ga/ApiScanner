import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { AppModuleShared } from './app.module.shared';
import { AppComponent } from './components/app/app.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

import { ORIGIN_URL } from './components/shared/constants/baseurl.constants';

@NgModule({
    bootstrap: [ AppComponent ],
    imports: [
        BrowserModule,
        AppModuleShared,
        BrowserAnimationsModule,
    ],
    providers: [
        { provide: ORIGIN_URL, useFactory: (getOriginUrl) },
        { provide: 'BASE_URL', useFactory: getBaseUrl }
    ]
})
export class AppModule {
}

export function getBaseUrl() {
    return document.getElementsByTagName('base')[0].href;
}

export function getOriginUrl() {
    return window.location.origin;
}