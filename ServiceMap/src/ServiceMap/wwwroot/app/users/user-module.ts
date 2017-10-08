import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { ReactiveFormsModule } from '@angular/forms';
import { UserDetailGuard, UserEditGuard } from './user-guard.service';
import { UserComponent } from './user.component';
import { UserService } from './user.service';
import { SmSharedModule } from '../shared/shared.module';
import { UserListComponent } from './user-list.component';


@NgModule({
    imports: [
        ReactiveFormsModule,
        SmSharedModule,
        RouterModule.forChild([
            { path: 'userlist', component: UserListComponent },
            {
                path: 'adduser',
                canDeactivate: [UserEditGuard],
                canActivate: [UserDetailGuard],
                component: UserComponent
            },
        ])
    ],
    declarations: [
        UserComponent,
        UserListComponent
    ],
    providers: [
        UserService,
        UserDetailGuard,
        UserEditGuard
    ]
})
export class ProductModule { }