import { Injectable, OnInit } from '@angular/core';
import { BehaviorSubject } from 'rxjs/BehaviorSubject';

import { TranslateService } from '@ngx-translate/core';

@Injectable()
export class TranslationService implements OnInit {
    public language = new BehaviorSubject<string>('en');

    constructor(private _translate: TranslateService) { }

    public changeLanguage(lang: string) {
        if (this._translate.currentLang != lang) {
            localStorage.setItem('lang', lang);
            this._translate.use(lang);
            this.language.next(lang);
        }
    }

    ngOnInit() {
        this.language.next(this.getLanguage());
    }

    public getLanguage(): string {
        return this._translate.currentLang;
    }
}