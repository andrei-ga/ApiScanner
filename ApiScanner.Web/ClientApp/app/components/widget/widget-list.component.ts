import { Component } from '@angular/core';
import { MatTableDataSource } from '@angular/material';

import { WidgetService } from './widget.service';

import { WidgetModel } from './widget.model';

@Component({
    templateUrl: './widget-list.component.html',
    styleUrls: ['./widget-list.component.css']
})
export class WidgetListComponent {
    public displayedColumns: string[] = ['name', 'location', 'public', 'edit'];
    public widgetDataSource: MatTableDataSource<WidgetModel> = new MatTableDataSource<WidgetModel>();
    public selectedIndex = -1;

    constructor(
        private _widgetService: WidgetService) { }

    ngOnInit() {
        this._widgetService.getWidgets()
            .subscribe(
            data => {
                this.widgetDataSource.data = data;
            });
    }

    public highlight(index: number) {
        this.selectedIndex = this.selectedIndex == index ? -1 : index;
    }
}