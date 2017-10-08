import { Component, Input, ViewChild, ViewContainerRef } from '@angular/core'
import { ModalDirective } from 'ngx-bootstrap';

@Component({
    selector: 'lg-modal',
    templateUrl: 'app/shared/lgModal.component.html'
})

export class LgModalComponent {
    @Input() myheader: string;
    @ViewChild('lgModal') public lgModalRef: ModalDirective;

    show() {
        this.lgModalRef.show();
    }
    hide() {
        this.lgModalRef.hide();
    }
}