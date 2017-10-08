import { NgModule } from '@angular/core';
import { PageComponent } from './page.component';
import { CommonModule } from '@angular/common';
import { PageService } from './page.service'
import { IPage } from './page'

@NgModule({
    declarations: [
        PageComponent
    ],
    imports: [CommonModule      
    ],
    exports: [PageComponent ],
    providers: [
        PageService
    ]
})

export class PageModule { }