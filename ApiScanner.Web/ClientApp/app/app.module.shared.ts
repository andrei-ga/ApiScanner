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
import { ApiCreateComponent } from './components/api/api.component';
import { ApiListComponent } from './components/api/list.component';

import { AccountService } from './components/account/account.service';
import { AccountDataService } from './components/account/account-data.service';
import { NotificationDataService } from './components/notification/notification-data.service';
import { ApiService } from './components/api/api.service';
import { LocationService } from './components/location/location.service';
import { GuardLogin, GuardLoggedIn, GuardSeeApi } from './components/account/auth.guard';
import { TranslationService } from './components/shared/services/translation.service';

import { TranslateModule, TranslateLoader, TranslateService } from '@ngx-translate/core';
import { TranslateHttpLoader } from '@ngx-translate/http-loader';

// angular material modules
import {
    MatAutocompleteModule,
    MatButtonModule,
    MatButtonToggleModule,
    MatCardModule,
    MatCheckboxModule,
    MatChipsModule,
    MatDatepickerModule,
    MatDialogModule,
    MatExpansionModule,
    MatGridListModule,
    MatIconModule,
    MatInputModule,
    MatListModule,
    MatMenuModule,
    MatNativeDateModule,
    MatProgressBarModule,
    MatProgressSpinnerModule,
    MatRadioModule,
    MatRippleModule,
    MatSelectModule,
    MatSidenavModule,
    MatSliderModule,
    MatSlideToggleModule,
    MatSnackBarModule,
    MatStepperModule,
    MatTableModule,
    MatTabsModule,
    MatToolbarModule,
    MatTooltipModule,
} from '@angular/material';
import { CdkTableModule } from '@angular/cdk/table';

let materialModules = [
    CdkTableModule,
    MatAutocompleteModule,
    MatButtonModule,
    MatButtonToggleModule,
    MatCardModule,
    MatCheckboxModule,
    MatChipsModule,
    MatDatepickerModule,
    MatDialogModule,
    MatExpansionModule,
    MatGridListModule,
    MatIconModule,
    MatInputModule,
    MatListModule,
    MatMenuModule,
    MatNativeDateModule,
    MatProgressBarModule,
    MatProgressSpinnerModule,
    MatRadioModule,
    MatRippleModule,
    MatSelectModule,
    MatSidenavModule,
    MatSliderModule,
    MatSlideToggleModule,
    MatSnackBarModule,
    MatTableModule,
    MatTabsModule,
    MatToolbarModule,
    MatTooltipModule,
];

export function HttpLoaderFactory(http: HttpClient) {
    return new TranslateHttpLoader(http);
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
            { path: 'apis/:id', component: ApiCreateComponent, data: { title: 'Edit api - Api Scanner', pageHeader: 'EditApi' }, canActivate: [GuardSeeApi] },
            { path: 'register', component: RegisterComponent, data: { title: 'Register - Api Scanner', pageHeader: 'NewAccount' }, canActivate: [GuardLogin] },
            { path: 'login', component: LoginComponent, data: { title: 'Login - Api Scanner', pageHeader: 'SignIn' }, canActivate: [GuardLogin] },
            { path: '**', redirectTo: '' }
        ]),
        TranslateModule.forRoot({
            loader: {
                provide: TranslateLoader,
                useFactory: HttpLoaderFactory,
                deps: [HttpClient]
            }
        })
    ],
    providers: [
        AccountService,
        AccountDataService,
        NotificationDataService,
        ApiService,
        LocationService,
        GuardLogin,
        GuardLoggedIn,
        GuardSeeApi,
        HttpClient,
        TranslateService,
        TranslationService,
    ]
})
export class AppModuleShared {
}
