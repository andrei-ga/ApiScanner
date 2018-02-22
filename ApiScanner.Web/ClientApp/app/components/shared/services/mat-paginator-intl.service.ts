import { Injectable } from '@angular/core';
import { MatPaginatorIntl } from '@angular/material';
import { TranslateService } from '@ngx-translate/core';

@Injectable()
export class MatPaginatorIntlService extends MatPaginatorIntl {
    public itemsPerPageLabel: string = 'Items per page';
    public nextPageLabel: string = 'Next page';
    public previousPageLabel: string = 'Previous page';
    public firstPageLabel: string = 'First page';
    public lastPageLabel: string = 'Last page';

    constructor(
        private _translate: TranslateService) {
        super();
        this._translate.get(['PaginatorItemsPerPage', 'PaginatorNextPage', 'PaginatorPreviousPage', 'PaginatorFirstPage', 'PaginatorLastPage'])
            .subscribe(data => {
                this.itemsPerPageLabel = data.PaginatorItemsPerPage;
                this.nextPageLabel = data.PaginatorNextPage;
                this.previousPageLabel = data.PaginatorPreviousPage;
                this.firstPageLabel = data.PaginatorFirstPage;
                this.lastPageLabel = data.PaginatorLastPage;
            });
    }
}