import { Injectable } from '@angular/core';
import { Subject } from 'rxjs/Subject';

import { AccountService } from './account.service';

import { AccountModel } from './account.model';

@Injectable()
export class AccountDataService {
    public account = new Subject<AccountModel>();

    public refreshData() {
        this._accountService.getAccountData()
            .subscribe(
            data => {
                if (data)
                    this.account.next(data);
                else
                    this.account.next();
            });
    }

    constructor(private _accountService: AccountService) {
        this.refreshData();
    }
}