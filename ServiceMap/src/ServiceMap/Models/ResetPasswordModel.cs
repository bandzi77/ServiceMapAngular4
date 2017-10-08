using ServiceMap.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ServiceMap.Models
{
    public class ResetPasswordModel
    {
        [Required(ErrorMessage = ConstsData.EmailRequiredMsg)]
        [UIHint("email")]
        [EmailAddress(ErrorMessage = ConstsData.EmailValidationMsg)]
        [RegularExpression(ConstsData.EmailRegExp, ErrorMessage = ConstsData.EmailRegExpMsg)]
        public string Email { get; set; }

        [Required(ErrorMessage = ConstsData.PasswordRequiredMsg)]
        [UIHint("password")]
        [RegularExpression( ConstsData.PasswordRegExp,ErrorMessage = ConstsData.PasswordRegExpMsg)]
        [Display(Name = "Hasło")]
        public string Password { get; set; }

        [Required(ErrorMessage = ConstsData.PasswordConfirmRequiredMsg)]
        [UIHint("password")]
        [RegularExpression(ConstsData.PasswordRegExp, ErrorMessage = ConstsData.PasswordConfirmRegExpMsg)]
        [Display(Name = "Potwierdź hasło")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = ConstsData.TokenValidationMsg)]
        public string Token { get; set; }
    }
}
