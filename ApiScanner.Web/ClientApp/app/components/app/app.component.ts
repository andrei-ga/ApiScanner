import 'rxjs/add/operator/filter';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/mergeMap';

import { Component, OnInit, PLATFORM_ID, Inject } from '@angular/core';
import { Title } from '@angular/platform-browser';
import { Router, NavigationEnd, ActivatedRoute } from '@angular/router';
import { isPlatformBrowser } from '@angular/common';

import { TranslateService } from '@ngx-translate/core';
import { TranslationService } from '../shared/services/translation.service';
import { PageHeaderService } from '../shared/services/page-header.service';

import { NavMenuModel } from '../navmenu/navmenu.model';

@Component({
    selector: 'app',
    templateUrl: './app.component.html',
    styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
    public embed: boolean = true;
    public pageHeader: NavMenuModel;

    constructor(
        @Inject(PLATFORM_ID) private platformId: Object,
        private _router: Router,
        private _titleService: Title,
        private _route: ActivatedRoute,
        private _headerService: PageHeaderService,
        private _translation: TranslationService,
        private _translate: TranslateService) {
        this._translate.setDefaultLang('en');
    }

    ngOnInit() {
        // Client side code only
        if (isPlatformBrowser(this.platformId)) {
            // Initialize localization (translation) module
            let lang = localStorage.getItem('lang');
            if (lang == null) {
                lang = 'en';
                localStorage.setItem('lang', lang);
            }
            this._translation.changeLanguage(lang);
        }

        this._router.events
            .filter(event => event instanceof NavigationEnd)
            .map(() => this._route)
            .map(route => {
                while (route.firstChild) route = route.firstChild;
                return route;
            })
            .filter(route => route.outlet === 'primary')
            .mergeMap(route => route.data)
            .subscribe((event) => {
                // Set title defined by routes
                this._titleService.setTitle(event['title']);
                this.pageHeader = event['pageHeader'];
                if (this.pageHeader) {
                    this.embed = this.pageHeader.embed == true;
                    this._headerService.setEmbed(this.embed);
                }
            });
    }
}
