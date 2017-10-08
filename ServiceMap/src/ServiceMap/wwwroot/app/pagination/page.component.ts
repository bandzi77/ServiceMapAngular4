import { Component, Input, Output, EventEmitter, OnInit, OnChanges } from '@angular/core';
import { PageService } from './page.service'

@Component({
    selector: 'page-app',
    templateUrl: 'app/pagination/page.component.html',
    styleUrls: ['app/pagination/page.component.css']
})

export class PageComponent implements OnInit, OnChanges {
    // array of all items to be paged
    @Input() totalCount: number;
    @Input() pageSize: number;

    @Output() pageClicked: EventEmitter<number> = new EventEmitter<number>();

    pager: any = {};

    constructor(private _pageservice: PageService) { }

    ngOnChanges(): void {
        this.pager = this._pageservice.getPager(this.totalCount, 1, this.pageSize);
    };

    ngOnInit() {
        this.pager = this._pageservice.getPager(this.totalCount, 1, this.pageSize);
    };

    onSetPage(page: number) {
        if ((page < 1 || page > this.pager.totalPages) || (page == 1 && this.pager.totalPages == 1) || (page == this.pager.currentPage)) {
            return;
        }
        this.pageClicked.emit(page);
        this.pager = this._pageservice.getPager(this.totalCount, page, this.pageSize);
    };
}