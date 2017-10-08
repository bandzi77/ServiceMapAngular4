using ServiceMap.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ServiceMap.Models.User
{
    public class User
    {
        [Required]
        public string _id { get; set; }

        // Zmieniono nazwę użytkownika na numer - potrzebny refaktor w przyszłości
        [Required]
        [MinLength(9)]
        [MaxLength(9)]
        [RegularExpression(ConstsData.ClientNumberRegExp)]
        public string TntUserName { get; set; }

        [Required]
        [MaxLength(250)]
        public string Email { get; set; }
        [RegularExpression(ConstsData.PasswordRegExp)]
        [Required]
        public string Password { get; set; }
        [Required]
        public bool IsSuperUser { get; set; }
        [Required]
        public bool IsLocked { get; set; }
        public int? LimitOfRequestsPerDay { get; set; }
        public int? NumberOfRequestsPerDay { get; set; }
    }
}
