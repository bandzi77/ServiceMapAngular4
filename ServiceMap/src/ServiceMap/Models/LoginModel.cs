using ServiceMap.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ServiceMap.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage = ConstsData.EmailRequiredMsg)]
        [UIHint("email")]
        [EmailAddress(ErrorMessage = ConstsData.EmailValidationMsg)]
        public string Email { get; set; }

        [Required(ErrorMessage = ConstsData.PasswordRequiredMsg)]
        [UIHint("password")]
        [Display(Name ="Hasło")]
        public string Password { get; set; }
    }
}
