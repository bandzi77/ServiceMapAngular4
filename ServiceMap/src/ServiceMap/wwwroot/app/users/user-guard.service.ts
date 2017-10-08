import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, Router, CanDeactivate } from '@angular/router';

import { UserComponent } from './user.component';

@Injectable()
export class UserDetailGuard implements CanActivate {

    constructor(private router: Router) {
    }

    canActivate(route: ActivatedRouteSnapshot): boolean {
        let id = String(route.queryParams['_id']);
        let email = route.queryParams['email'];
        if ((id === 'undefined' || id == null) || (id !== '0'  && (email === 'undefined' || email == null))) {
            alert('Niepoprawny adres http');
            // start a new navigation to redirect to list page
            this.router.navigate(['/userlist']);
            // abort current navigation
            return false;
        };
        return true;
    }
}

@Injectable()
export class UserEditGuard implements CanDeactivate<UserComponent> {
    canDeactivate(component: UserComponent): boolean {
        if (component.userForm.dirty) {
            let productName = component.userForm.get('email').value || 'nowego użytkownika';
            return confirm(`Opuszczenie strony spowoduje utratę danych dla ${productName}?`);
        }
        return true;
    }
}
