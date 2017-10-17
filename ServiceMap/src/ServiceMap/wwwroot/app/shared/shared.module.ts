import { NgModule, Provider } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { BusyModule, BusyConfig, BUSY_CONFIG_DEFAULTS } from 'angular2-busy';
import { ModalModule, PopoverModule, TooltipModule } from 'ngx-bootstrap';
import { LgModalComponent } from './lgModal.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { ToastrService } from './toastr.service';
import { ToastModule, ToastOptions } from 'ng2-toastr/ng2-toastr';
import { CustomOption } from './toastr-custom-option';
import { DataTableModule, SharedModule, MultiSelectModule, ToggleButtonModule, DropdownModule } from 'primeng/primeng';
import { BusyConfigFactory } from './busy-config';

//let options: BusyConfig = {
//    delay: 0,
//    minDuration: 0,
//    backdrop: true,
//    message: 'Proszę czekać...',
//    template: BUSY_CONFIG_DEFAULTS.template,
//    wrapperClass: BUSY_CONFIG_DEFAULTS.wrapperClass
//};


@NgModule({
    declarations: [
        LgModalComponent
    ],
    exports: [
        CommonModule,
        BusyModule,
        FormsModule,
        LgModalComponent,
        ModalModule,
        PopoverModule,
        TooltipModule,
        ToastModule,
        DataTableModule,
        SharedModule,
        MultiSelectModule,
        ToggleButtonModule,
        DropdownModule,
        BrowserAnimationsModule
    ],
    imports: [
        CommonModule,
        BrowserAnimationsModule,
        BusyModule,
        ModalModule.forRoot(),
        PopoverModule.forRoot(),
        TooltipModule.forRoot(),
        ToastModule.forRoot(),
        DropdownModule
    ],
     
    providers: [
        ToastrService,
        {
            provide: ToastOptions,
            useClass: CustomOption,
        },
        {
            provide: BusyConfig,
            useFactory: BusyConfigFactory
        }
    ]
})

export class SmSharedModule {
}