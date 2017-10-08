using ServiceMap.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ServiceMap.Models
{
    public class ForgotPasswordModel
    {
        [Required(ErrorMessage = ConstsData.EmailRequiredMsg)]
        [EmailAddress(ErrorMessage = ConstsData.EmailValidationMsg)]
        [RegularExpression(ConstsData.EmailRegExp, ErrorMessage = ConstsData.EmailRegExpMsg)]
        [UIHint("email")]
        public string Email { get; set; }
    }
}
