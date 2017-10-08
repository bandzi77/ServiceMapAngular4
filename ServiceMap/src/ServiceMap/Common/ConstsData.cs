using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServiceMap.Common
{
    public class ConstsData
    {
        // Resetowanie hasła
        public static readonly string PasswordForNewUserSubject = "Mapa Serwisowa TNT";
        public static readonly string PasswordForNewUserMsg = "<p>Drogi Użytkowniku!<br>  <p>Zostało założone dla Ciebie konto w aplikacji Mapa Serwisowa TNT.<br>   <p>Dane konta:</p><p>Hasło: ";
        public static readonly string PasswordForNewUserQueryLimit = "<br>Limit zapytań w ciągu dnia: ";
        public static readonly string PasswordForNewUserLinkApp = @"<br><a href=""www.tnt.com/express/pl_pl/site/shipping-tools/mapa-serwisowa.html"">Link do aplikacji</a>";
        public static readonly string PasswordForNewMessageFooter = "<p>Pozdrawiamy<br>Zespół TNT";
        public static readonly string ResetLinkPasswordSubject = "Reset hasła";
        public static readonly string ResetLinkPasswordMsg = "<p>Drogi Użytkowniku!<br>    <p>Poniżej znajdziesz link pozwalający na zresetowanie hasła w aplikacji Mapa Serwisowa:<br>";
        public static readonly string ResetLinkPasswordMsgLink1 = "<a href=\"";
        public static readonly string ResetLinkPasswordMsgLink2 = "\">Zresetuj hasło</a><br><br>";
        public static readonly string ResetLinkWrongEmail = "1. Wystąpił błąd podczas resetowania hasła";
        public static readonly string ResetLinkWrongToken = "2. Wystąpił błąd podczas resetowania hasła";
        public static readonly string ResetPasswordSuccess= "Twoje hasło zostało pomyślnie zresetowane.";

        public const string EmailValidationMsg = "Niepoprawny format adresu email";
        public const string EmailRequiredMsg = "Email jest wymagany";
        public const string EmailRegExpMsg = "Dopuszczalne znaki specjalne w email prefix: '.' '_' '%' '+' '-'";
        public const string EmailRegExp = "[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.]+";
        public const string PasswordRequiredMsg = "Hasło jest wymagane";
        public const string PasswordRegExpMsg = "Hasło nie spełnia wymagań";
        public const string PasswordConfirmRequiredMsg = "Potwierdzenie hasła jest wymagane";
        public const string PasswordConfirmRegExpMsg = "Potwierdzone hasło nie spełnia wymagań";
        public const string ClientNumberRegExp = "^[0-9]+$";
        public const string PasswordRegExp = "(?=.*\\d)(?=.*[a-zA-Z])(?=.+[_\\!\\@\\#\\$\\%\\^\\&\\*\\(\\)\\+\\-\\=])(?!.*\\s).{8,12}";
        public const string TokenValidationMsg = "Niepopawny link odzyskiwania hasła";
        public static readonly string DifferencesPasswordMsg = "Podane hasła są różne";

        // Dodawanie/Edycja użytkownika
        public static readonly string UserModelInvalid = "3. Wystąpił błąd podczas dodawania nowego użytkownika";
        public static readonly string UserCreateIdentityError = "4. Wystąpił błąd podczas dodawania nowego użytkownika";
        public static readonly string UserCreateAnotherError = "5. Wystąpił błąd podczas dodawania nowego użytkownika";
        public static readonly string UserCreateSuccess = "6. Użytkownik został dodany";
        public static readonly string UserCreateEmailError = "6.1. Konto zostało utworzone. Wystąpił błąd podczas wysłania emaila do użytkownika z danymi konta.";
        public static readonly string UserCreateInvalidLimit = "6.2. Wystąpił błąd podczas dodawania nowego użytkownika";
        public static readonly string UserCreateInvalidAdminEmail = "6.3. Wystąpił błąd podczas dodawania nowego użytkownika";
        public static readonly string UserAlreadyExists = "7. Podany adres email istnieje już w systemie";
        public static readonly string DeleteUserSuccess = "8. Użytkownik został usunięty z systemu";
        public static readonly string DeleteUserError = "9. Błąd podczas usuwania użytkownika z systemu";
        public static readonly string UpdateUserSuccess = "10. Dane użytkownika zostały zaaktualizowane";
        public static readonly string UpdateUserError = "11. Błąd podczas aktualizacji danych, spróbj ponownie";
        public static readonly string UpdateUserNotExists = "12. Błąd podczas aktualizacji danych, spróbj ponownie";
        public static readonly string UpdateUserIdentity = "13. Błąd podczas aktualizacji danych, spróbj ponownie";
        public static readonly string UpdateUserResetAccessFailed = "14. Błąd podczas aktualizacji danych, spróbj ponownie";
        public static readonly string UpdateUserRoleIdentity = "15. Błąd podczas aktualizacji danych, spróbj ponownie";

        //Wyszukwianie usług
        public static readonly string ExceededNumberOfRequestsPerDay = "16. Przekroczony limit zapytań w ciągu dnia";

        // Zablokowanie konta
        public static readonly string AccountIsLockedSubject = "Blokada konta użytkownika";
        public static readonly string AccountIsLockedMessage = "Informacja,\n\n    Konto użytkownika {0} zostało zablokowane z powodu przekroczenia dozwolnej liczby błędnie wprowadzanego hasła.";

    }
}
