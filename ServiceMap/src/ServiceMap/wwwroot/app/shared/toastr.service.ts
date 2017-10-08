import { Injectable } from '@angular/core';
import { ToastsManager } from 'ng2-toastr/ng2-toastr';

import { Subject } from 'rxjs/Subject';
import { Observable } from 'rxjs/Observable';

@Injectable()
export class ToastrService {
    private smToastr: IToastrSm;
    private subject: Subject<IToastrSm> = new Subject<IToastrSm>();

    constructor(private toastrMg: ToastsManager) { }

    success(toastr: IToastrSm): void {
        this.toastrMg.success(toastr.message, 'Success!', toastr.options);
    }

    error(toastr: IToastrSm): void {
        this.toastrMg.error(toastr.message, 'Błąd!', toastr.options || { dismiss: 'click' });
    }

    info(toastr: IToastrSm): void {
        this.toastrMg.info(toastr.message, 'Informacja!', toastr.options);
    }

    warning(toastr: IToastrSm): void {
        this.toastrMg.warning(toastr.message, 'Uwaga!', toastr.options);
    }
}

export interface IToastrSm {
    message: string;
    options?: any;
}
