import {
    trigger,
    state,
    style,
    transition,
    animate, Component, OnInit, OnDestroy, ViewContainerRef
} from '@angular/core';
import { FormControl, FormBuilder, FormGroup, Validators, AbstractControl, ValidatorFn } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { IUser } from './user';
import { UserService } from './user.service';
import 'rxjs/add/operator/debounceTime';
import { Subscription } from 'rxjs/Subscription';
import { Observable } from 'rxjs/Observable';
import { Location } from '@angular/common';
import { ToastrService, IToastrSm } from '../shared/toastr.service';
import { IResult } from '../shared/common';

@Component({
    selector: 'cr-user',
    templateUrl: './user.component.html',
    styleUrls: ['./user.component.css'],
    animations: [
        trigger('shrinkOut', [
            state('true', style({ opacity: 1 })),
            state('void', style({ opacity: 0 })),
            transition(':enter', animate('0ms ease-in-out')),
            transition(':leave', animate('0ms ease-in-out'))//300ms ease-in-out
        ])
    ]
})
export class UserComponent implements OnInit, OnDestroy {

    busyIndicator: Subscription;
    pageTitle: string = '';
    errorMessage: string;
    private _inputType = {
        keydown: 'text',
        keyup: 'password'
    }
    inputType: string = "password";
    user: IUser = new IUser();
    userForm: FormGroup;
    userNameMessage: string = '';
    emailMessage: string = '';
    passwordMessage: string = '';
    limitPerDayMessage: string = '';
    private userNameRegEx: string = '^[0-9]+$'
    private sub: Subscription;
    private emailTntRegEx: string = '[a-zA-Z0-9._%+-]+@(TNT.COM|tnt.com)';
    private regExpEmail = new RegExp(this.emailTntRegEx);
    emailRegEx: string = '[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.]+'; //'[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+';
    passwordRegEx: string = '(?=.*\\d)(?=.*[a-zA-Z])(?=.+[_\\!\\@\\#\\$\\%\\^\\&\\*\\(\\)\\+\\-\\=])(?!.*\\s).{8,12}'
    patternEmailTnt: string = 'Niepoprawny adres email z domeny tnt.com.';
    patternEmail: string = 'Niepoprawny adres email.';
    result: IResult;
    isDisabledCheckBoxTntAccount: boolean = false;

    private userNameValidationMessages = {
        required: 'Numer Klienta jest wymagana.',
        pattern: 'Dopuszczalne znaki to cyfry.',
        minlength: 'Wymagana długość to 9 cyfr.'
    };

    private emailValidationMessages = {
        required: 'Email jest wymagany.',
        pattern: this.patternEmail,
        maxlength: 'Email nie może przekraczać 250 znaków.'
    };

    private passValidationMessages = {
        required: "Hasło jest wymagane.",
        pattern: "Hasło nie spełnia wymagań.",
        minlength: 'Hasło jest za krótkie.',
        maxlength: 'Hasło jest za długie.'
    };

    private limitOfRequestsPerDayMessage = {
        required: "Limit zapytań jest wymagany.",
        range: "Wprowadź wartość z przedziału od 1 do 500"
    };


    constructor(
        private fb: FormBuilder,
        private route: ActivatedRoute,
        private router: Router,
        private userService: UserService,
        private location: Location,
        private toastService: ToastrService ) {
    }

    ngOnInit(): void {

        // Tworzy rective form
        this._createReactiveUserForm();

        // Ustawia komunikaty dla walidatorów
        this._setMessageForValidators();

        // Tworzy obiekt z danymi użytkownika do edycji
        this._userInit();
    }

    onBack() {
        this.location.back();
    }

    ngOnDestroy(): void {
        this.sub.unsubscribe();
    }

    private _createReactiveUserForm(): void {
        this.userForm = this.fb.group({
            _id: "0",
            tntUserName: [null, this._getUserNameValidators()],
            email: [{ value: '', disabled: true }, this._getEmailValidators()],
            password: [null, this._getPassswordValidators()],
            limitOfRequestsPerDay: [null, this._getLimitPerDayValidators()],
            isSuperUser: false,
            isLocked: false
        });
    }

    // Tworzy obiekt z danymi użytkownika do edycji
    private _userInit(): void {
        this.sub = this.route.queryParams.subscribe(
            params => {
                let user = <IUser>{
                    _id: String(params['_id']),
                    tntUserName: String(params['tntUserName']),
                    email: String(params['email']),
                    password: '',
                    limitOfRequestsPerDay: Number(params['limitOfRequestsPerDay']),
                    isSuperUser: String(params['isSuperUser']) === "true",
                    isLocked: String(params['isLocked']) === "true"
                }
                this.onUserRetrieved(user);
            }
        );
    }

    // Metoda wypełniająca danymi formularz jeśli jest w trybie edycji użytkownika, jeśli nie wyświetla pusty
    onUserRetrieved(user: IUser): void {
        this.user = user;

        if (this.userForm) {
            this.userForm.enable();
        }

        if (this.user._id === "0") {
            this.resetForm();
            this.pageTitle = 'Dodaj nowego użytkownika';
        } else {
            this.pageTitle = `Edytuj użytkownika: ${this.user.email}`;

            // Blokuje pola kluczowe do edycii
            this.userForm.get('email').disable();
            this.userForm.get('password').disable();

            // Nie pozwala na zmianę typu konta jeśli email nie był z domeny TNT
            if (!this.regExpEmail.test(this.user.email)) {
                this.userForm.get('isSuperUser').disable();
                this.isDisabledCheckBoxTntAccount = true;
            }

            // Wypełnia formularz do edycji danymi z query params
            this.userForm.patchValue({
                _id: this.user._id === "undefined" ? 0 : this.user._id,
                tntUserName: this.user.tntUserName === "undefined" ? null : this.user.tntUserName,
                email: this.user.email === "undefined" ? null : this.user.email,
                password: '********',
                limitOfRequestsPerDay: isNaN(this.user.limitOfRequestsPerDay) ? null : this.user.limitOfRequestsPerDay,
                isSuperUser: this.user.isSuperUser,
                isLocked: this.user.isLocked
            });
        }
    }

    setNotification(ischecked: boolean): void {
        const emailControl = this.userForm.get('email');
        const limitOfRequestsPerDayControl = this.userForm.get('limitOfRequestsPerDay');

        // zmiania reguły walidatora w zależnośći od tego czy jest to konto z domeny TNT
        if (ischecked) {
            this.emailValidationMessages.pattern = this.patternEmailTnt;
            emailControl.setValidators(this._getTntEmailValidators());
            limitOfRequestsPerDayControl.reset();
            limitOfRequestsPerDayControl.clearValidators();
        }
        else {
            this.emailValidationMessages.pattern = this.patternEmail;
            emailControl.setValidators(this._getEmailValidators());
            limitOfRequestsPerDayControl.setValidators(this._getLimitPerDayValidators());
        }

        emailControl.updateValueAndValidity();
        limitOfRequestsPerDayControl.updateValueAndValidity();
    }

    setMessage(c: AbstractControl, ValidationMessages: Object, resultObject: string): void {
        this[resultObject] = '';
        if ((c.touched || c.dirty) && c.errors) {
            this[resultObject] = Object.keys(c.errors).map(key =>
                ValidationMessages[key]).join(' ');
        }
    }

    private onEyeEvent(event: MouseEvent): void {
        if (event.type === 'mousedown' && event.button === 0 && this.user._id === "0") {
            this.inputType = this._inputType.keydown;
        }
        if (((event.type === 'mouseup' && event.button === 0) || event.type === 'mouseleave') && this.user._id === "0") {
            this.inputType = this._inputType.keyup;
        }
    }

    deleteUser(): void {
        if (this.user._id === "0") {
            // Dla nowego użytkownika, tylko czyści formularz
            this.resetForm()
        } else {
            if (confirm(`Czy chcesz usunąć użytkownika: ${this.user.email}?`)) {

                this.busyIndicator = this.userService.deleteUser(this.user._id)
                    .subscribe(result => {
                        this.onDeleteComplete(result, this.user);
                    },
                    (error: any) =>
                        this.errorMessage = <any>error
                    );
            }
        }
    }

    saveUser() {
        // Zabezpieczenie przed wpisaniem pustych znaków
        this.userForm.get('tntUserName').setValue(this.userForm.get('tntUserName').value.trim());

        if (this.userForm.dirty && this.userForm.valid) {
            // Copy the form values over the product object values
            let p = Object.assign({}, this.user, this.userForm.value);

            this.busyIndicator = this.userService.saveUser(p)
                .subscribe(result => {
                    this.onSaveComplete(result, this.user);
                },
                (error: any) => this.errorMessage = <any>error
                );
        } else if (!this.userForm.dirty) {
            // nic nie rób
        }

        console.log(this.userForm);
        console.log('Saved: ' + JSON.stringify(this.userForm.value));
    }


    private onDeleteComplete(res: IResult, user: IUser): void {
        if (res.success) {
            this.toastService.success(<IToastrSm>{
                message: res.message
            });
            this.onBack();
        } else {
            this.toastService.error(<IToastrSm>{
                message: res.message,
            });
        }
        // Reset the form to clear the flags
        // this.userForm.reset();
        // TODO
        //this.router.navigate(['/userlist']);
        //this.onBack()
    }

    private onSaveComplete(res: IResult, user: IUser): void {
        if (res.success) {

            this.resetForm();
            this.toastService.success(<IToastrSm>{
                message: res.message
            });

            if (user._id !== "0") {
               this.onBack();
            }
            // this.userForm.markAsPristine();
        } else {
            this.toastService.error(<IToastrSm>{
                message: res.message
            });
        }
    }

    private resetForm(): void {
        this.userForm.reset();
        this.userForm.patchValue({
            _id: "0",
            isSuperUser: false,
            isLocked: false
        });
    }

    private _setMessageForValidators() {

        const userNameControl = this.userForm.get('tntUserName');
        userNameControl.valueChanges.debounceTime(0).subscribe(value =>
            this.setMessage(userNameControl, this.userNameValidationMessages, 'userNameMessage'));

        const emailControl = this.userForm.get('email');
        emailControl.valueChanges.debounceTime(0).subscribe(value =>
            this.setMessage(emailControl, this.emailValidationMessages, 'emailMessage'));

        const passwordControl = this.userForm.get('password');
        passwordControl.valueChanges.debounceTime(0).subscribe(value =>
            this.setMessage(passwordControl, this.passValidationMessages, 'passwordMessage'));

        const limitOfRPerDayControl = this.userForm.get('limitOfRequestsPerDay');
        limitOfRPerDayControl.valueChanges.debounceTime(0).subscribe(value =>
            this.setMessage(limitOfRPerDayControl, this.limitOfRequestsPerDayMessage, 'limitPerDayMessage'));

        this.userForm.get('isSuperUser').valueChanges
            .subscribe(value => this.setNotification(value));
    }

    private _getUserNameValidators(): ValidatorFn {
        return Validators.compose([
            Validators.required,
            Validators.pattern(this.userNameRegEx),
            Validators.minLength(9),
            Validators.maxLength(9)]
        );
    }

    private _getEmailValidators(): ValidatorFn {
        return Validators.compose([
            Validators.required,
            Validators.maxLength(250),
            Validators.pattern(this.emailRegEx)]
        );
    }


    private _getTntEmailValidators(): ValidatorFn {
        return Validators.compose([
            Validators.required,
            Validators.maxLength(250),
            Validators.pattern(this.emailTntRegEx)]
        );
    }

    private _getPassswordValidators(): ValidatorFn {
        return Validators.compose([
            Validators.required,
            Validators.maxLength(12),
            Validators.pattern(this.passwordRegEx)]
        );
    }

    private _getLimitPerDayValidators(): ValidatorFn {
        return Validators.compose([
            Validators.required,
            checkRange(1, 500)]
        );
    }
}



function checkRange(min: number, max: number): ValidatorFn {
    return (c: AbstractControl): { [key: string]: boolean } | null => {
        // Requred będzie odpowiada za brak danych
        if ((c.value || "").length == 0)
        { return null; }

        if (c.value !== undefined && (isNaN(c.value) || c.value < min || c.value > max)) {
            return { 'range': true };
        };
        return null;
    };
}
