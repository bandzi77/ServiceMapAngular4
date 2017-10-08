import { Injectable } from '@angular/core';

@Injectable()
export class PageService {

    getPager(totalCount: number, currentPage: number = 1, pageSize: number = 10) {
        // calculate total pages
        let totalPages = Math.ceil(totalCount / pageSize);

        let startPage: number, endPage: number;
        if (totalPages <= 10) {
            // less than 10 total pages so show all
            startPage = 1;
            endPage = totalPages;
        } else {
            // more than 10 total pages so calculate start and end pages
            if (currentPage <= 6) {
                startPage = 1;
                endPage = 10;
            } else if (currentPage + 4 >= totalPages) {
                startPage = totalPages - 9;
                endPage = totalPages;
            } else {
                startPage = currentPage - 5;
                endPage = currentPage + 4;
            }
        }

        // calculate start and end item indexes
        let startIndex = (currentPage - 1) * pageSize;
        let endIndex = Math.min(startIndex + pageSize - 1, totalCount - 1);

        // create an array of pages to ng-repeat in the pager control
        
        let pages = this.getArrayOfPages(startPage, endPage + 1);

        // return object with all pager properties required by the view
        return {
            totalCount: totalCount,
            currentPage: currentPage,
            pageSize: pageSize,
            totalPages: totalPages,
            startPage: startPage,
            endPage: endPage,
            startIndex: startIndex,
            endIndex: endIndex,
            pages: pages
        };
    }

    private getArrayOfPages(startPage: number, length: number): number[] {
        var pages: number[]=[];

        for (var i = startPage; i < length; i++) {
            pages.push(i)
        }
        return pages;
    }
}