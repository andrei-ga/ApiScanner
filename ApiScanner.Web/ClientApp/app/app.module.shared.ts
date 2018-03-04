import { NgModule } from '@angular/core';
import { CommonModule, APP_BASE_HREF } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { HttpClientModule, HttpClient } from '@angular/common/http';
import { RouterModule } from '@angular/router';
import { NgxChartsModule } from '@swimlane/ngx-charts';

import { AppComponent } from './components/app/app.component';
import { NavMenuComponent } from './components/navmenu/navmenu.component';
import { HomeComponent } from './components/home/home.component';
import { RegisterComponent } from './components/account/register.component';
import { LoginComponent } from './components/account/login.component';
import { NotificationComponent } from './components/notification/notification.component';
import { ApiCreateComponent } from './components/api/api-edit.component';
import { ApiComponent } from './components/api/api.component';
import { ApiListComponent } from './components/api/api-list.component';
import { WidgetEditComponent } from './components/widget/widget-edit.component';
import { WidgetListComponent } from './components/widget/widget-list.component';
import { WidgetComponent } from './components/widget/widget.component';
import { AdminConfigurationComponent } from './components/admin/configuration/configuration.component';

import { AccountService } from './components/account/account.service';
import { AccountDataService } from './components/account/account-data.service';
import { NotificationDataService } from './components/notification/notification-data.service';
import { ApiService } from './components/api/api.service';
import { WidgetService } from './components/widget/widget.service';
import { ApiLogService } from './components/api-log/api-log.service';
import { LocationService } from './components/location/location.service';
import { GuardLogin, GuardLoggedIn, GuardSeeApi, GuardSeeWidget, GuardAdmin } from './components/account/auth.guard';
import { TranslationService } from './components/shared/services/translation.service';
import { PageHeaderService } from './components/shared/services/page-header.service';
import { MatPaginatorIntlService } from './components/shared/services/mat-paginator-intl.service';

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
    MatPaginatorModule,
    MatPaginatorIntl,
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
    MatPaginatorModule,
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
        ApiComponent,
        ApiListComponent,
        WidgetEditComponent,
        WidgetListComponent,
        WidgetComponent,
        AdminConfigurationComponent,
    ],
    imports: [
        CommonModule,
        HttpClientModule,
        FormsModule,
        NgxChartsModule,
        ...materialModules,
        RouterModule.forRoot([
            { path: '', component: HomeComponent, data: { title: 'Api Scanner', pageHeader: { pageTitle: '', links: [] } } },
            { path: 'apis/create', component: ApiCreateComponent, data: { title: 'Create api - Api Scanner', pageHeader: { pageTitle: 'CreateApi', links: [{ name: 'ListApis', url: '/apis/list' }] } }, canActivate: [GuardLoggedIn] },
            { path: 'apis/list', component: ApiListComponent, data: { title: 'List apis - Api Scanner', pageHeader: { pageTitle: 'ListApis', links: [] } }, canActivate: [GuardLoggedIn] },
            { path: 'apis/:id', component: ApiComponent, data: { title: 'View api - Api Scanner', pageHeader: { pageTitle: 'ViewApi', links: [{ name: 'ListApis', url: '/apis/list' }] } }, canActivate: [GuardSeeApi] },
            { path: 'apis/:id/edit', component: ApiCreateComponent, data: { title: 'Edit api - Api Scanner', pageHeader: { pageTitle: 'EditApi', links: [{ name: 'ListApis', url: '/apis/list' }] } }, canActivate: [GuardSeeApi] },
            { path: 'widgets/create', component: WidgetEditComponent, data: { title: 'Edit widget - Api Scanner', pageHeader: { pageTitle: 'CreateWidget', links: [{ name: 'ListWidgets', url: '/widgets/list' }] } }, canActivate: [GuardLoggedIn] },
            { path: 'widgets/list', component: WidgetListComponent, data: { title: 'List widgets - Api Scanner', pageHeader: { pageTitle: 'ListWidgets', links: [] } }, canActivate: [GuardLoggedIn] },
            { path: 'widgets/:id', component: WidgetComponent, data: { title: 'View widget - Api Scanner', pageHeader: { pageTitle: 'ViewWidget', links: [{ name: 'ListWidgets', url: '/widgets/list' }] } }, canActivate: [GuardSeeWidget] },
            { path: 'widgets/:id/embed', component: WidgetComponent, data: { title: 'View widget - Api Scanner', pageHeader: { pageTitle: 'ViewWidget', links: [], embed: true } }, canActivate: [GuardSeeWidget] },
            { path: 'widgets/:id/edit', component: WidgetEditComponent, data: { title: 'Edit widget - Api Scanner', pageHeader: { pageTitle: 'EditWidget', links: [{ name: 'ListWidgets', url: '/widgets/list' }] } }, canActivate: [GuardSeeWidget] },
            { path: 'register', component: RegisterComponent, data: { title: 'Register - Api Scanner', pageHeader: { pageTitle: 'NewAccount', links: [] } }, canActivate: [GuardLogin] },
            { path: 'login', component: LoginComponent, data: { title: 'Login - Api Scanner', pageHeader: { pageTitle: 'SignIn', links: [] } }, canActivate: [GuardLogin] },
            { path: 'admin/configuration', component: AdminConfigurationComponent, data: { title: 'Admin configuration - Api Scanner', pageHeader: { pageTitle: 'ConfigurationList', links: [] } }, canActivate: [GuardAdmin] },
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
        PageHeaderService,
        AccountDataService,
        NotificationDataService,
        ApiService,
        WidgetService,
        ApiLogService,
        LocationService,
        GuardLogin,
        GuardLoggedIn,
        GuardSeeApi,
        GuardSeeWidget,
        GuardAdmin,
        HttpClient,
        TranslateService,
        TranslationService,
        { provide: MatPaginatorIntl, useClass: MatPaginatorIntlService }
    ]
})
export class AppModuleShared {
}
