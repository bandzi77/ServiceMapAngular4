using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static ServiceMap.Common.Enums;

namespace ServiceMap.Common
{
   public interface IEmailService
    {
        Task<bool> SendEmailAsync(string fromName, string CcEmail, string toEmail, string subject, string message, EmailFormat emailformat, bool addImgFooter);
        bool SendEmail(string fromName, string CcEmail, string toEmail, string subject, string message, EmailFormat emailformat, bool addImgFooter);
    }
}
