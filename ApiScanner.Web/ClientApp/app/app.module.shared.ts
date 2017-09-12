import { NgModule } from '@angular/core';
import { CommonModule, APP_BASE_HREF } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { HttpClientModule, HttpClient } from '@angular/common/http';
import { RouterModule } from '@angular/router';

import { AppComponent } from './components/app/app.component';
import { NavMenuComponent } from './components/navmenu/navmenu.component';
import { HomeComponent } from './components/home/home.component';
import { RegisterComponent } from './components/account/register.component';
import { LoginComponent } from './components/account/login.component';
import { NotificationComponent } from './components/notification/notification.component';
import { ApiCreateComponent } from './components/api/create.component';
import { ApiListComponent } from './components/api/list.component';

import { AccountService } from './components/account/account.service';
import { AccountDataService } from './components/account/account-data.service';
import { NotificationDataService } from './components/notification/notification-data.service';
import { ApiService } from './components/api/api.service';
import { GuardLogin, GuardLoggedIn } from './components/account/auth.guard';
import { TranslationService } from './components/shared/services/translation.service';

import { TranslateModule, TranslateLoader, TranslateService } from '@ngx-translate/core';
import { TranslateHttpLoader } from '@ngx-translate/http-loader';

import { ORIGIN_URL } from './components/shared/constants/baseurl.constants';

// angular material modules
import {
    MdAutocompleteModule,
    MdButtonModule,
    MdButtonToggleModule,
    MdCardModule,
    MdCheckboxModule,
    MdChipsModule,
    MdCoreModule,
    MdDatepickerModule,
    MdDialogModule,
    MdExpansionModule,
    MdGridListModule,
    MdIconModule,
    MdInputModule,
    MdListModule,
    MdMenuModule,
    MdNativeDateModule,
    MdPaginatorModule,
    MdProgressBarModule,
    MdProgressSpinnerModule,
    MdRadioModule,
    MdRippleModule,
    MdSelectModule,
    MdSidenavModule,
    MdSliderModule,
    MdSlideToggleModule,
    MdSnackBarModule,
    MdSortModule,
    MdTableModule,
    MdTabsModule,
    MdToolbarModule,
    MdTooltipModule,
} from '@angular/material';
import { CdkTableModule } from '@angular/cdk/table';

let materialModules = [
    CdkTableModule,
    MdAutocompleteModule,
    MdButtonModule,
    MdButtonToggleModule,
    MdCardModule,
    MdCheckboxModule,
    MdChipsModule,
    MdCoreModule,
    MdDatepickerModule,
    MdDialogModule,
    MdExpansionModule,
    MdGridListModule,
    MdIconModule,
    MdInputModule,
    MdListModule,
    MdMenuModule,
    MdNativeDateModule,
    MdPaginatorModule,
    MdProgressBarModule,
    MdProgressSpinnerModule,
    MdRadioModule,
    MdRippleModule,
    MdSelectModule,
    MdSidenavModule,
    MdSliderModule,
    MdSlideToggleModule,
    MdSnackBarModule,
    MdSortModule,
    MdTableModule,
    MdTabsModule,
    MdToolbarModule,
    MdTooltipModule,
];

export function createTranslateLoader(http: HttpClient, baseHref: string) {
    // Temporary Azure hack
    if (baseHref === null && typeof window !== 'undefined') {
        baseHref = window.location.origin;
    }
    // i18n files are in `wwwroot/assets/`
    return new TranslateHttpLoader(http, `${baseHref}/assets/i18n/`, '.json');
}

@NgModule({
    declarations: [
        AppComponent,
        NavMenuComponent,
        HomeComponent,
        RegisterComponent,
        LoginComponent,
        NotificationComponent,
        ApiCreateComponent,
        ApiListComponent,
    ],
    imports: [
        CommonModule,
        HttpClientModule,
        FormsModule,
        ...materialModules,
        RouterModule.forRoot([
            { path: '', component: HomeComponent, data: { title: 'Api Scanner' } },
            { path: 'apis/create', component: ApiCreateComponent, data: { title: 'Create api - Api Scanner', pageHeader: 'CreateApi' }, canActivate: [GuardLoggedIn] },
            { path: 'apis/list', component: ApiListComponent, data: { title: 'List apis - Api Scanner', pageHeader: 'ListApis' }, canActivate: [GuardLoggedIn] },
            { path: 'register', component: RegisterComponent, data: { title: 'Register - Api Scanner', pageHeader: 'NewAccount' }, canActivate: [GuardLogin] },
            { path: 'login', component: LoginComponent, data: { title: 'Login - Api Scanner', pageHeader: 'SignIn' }, canActivate: [GuardLogin] },
            { path: '**', redirectTo: '' }
        ]),
        TranslateModule.forRoot({
            loader: {
                provide: TranslateLoader,
                useFactory: (createTranslateLoader),
                deps: [HttpClient, [ORIGIN_URL]]
            }
        }),
    ],
    providers: [
        AccountService,
        AccountDataService,
        NotificationDataService,
        ApiService,
        GuardLogin,
        GuardLoggedIn,
        HttpClient,
        TranslateService,
        TranslationService,
    ]
})
export class AppModuleShared {
}
