using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using MimeKit;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using MimeKit.Utils;
using static ServiceMap.Common.Enums;

namespace ServiceMap.Common
{
    public class EmailService : IEmailService
    {
        private IConfiguration configuration;
        private IHostingEnvironment env;
        public EmailService(IConfiguration configuration, IHostingEnvironment env)
        {
            this.configuration = configuration;
            this.env = env;
        }
        public async Task<bool> SendEmailAsync(string fromName, string CcEmail, string toEmail, string subject, string message, EmailFormat emailFormat, bool addImgFooter)
        {

            bool result = true;
#if !DEBUG
            try
            {
                var mimeMsg = CreateEmailMessage(fromName, CcEmail, toEmail, subject, message, emailFormat, addImgFooter);
                using (var client = new SmtpClient())
                {
                    client.Connect(mimeMsg.Host, mimeMsg.Port, SecureSocketOptions.None);

                    if (mimeMsg.IsSmtpRequredAuthentication)
                    {
                        client.AuthenticationMechanisms.Remove("XOAUTH2");
                        client.Authenticate(mimeMsg.TntEmail, mimeMsg.Password);
                    }
                    await client.SendAsync(mimeMsg).ConfigureAwait(false);
                    await client.DisconnectAsync(true).ConfigureAwait(false);
                }
                result = true;
            }
            catch
            {
                result = false;
            }
#endif
            return result;

        }


        public bool SendEmail(string fromName, string CcEmail, string toEmail, string subject, string message, EmailFormat emailFormat, bool addImgFooter)
        {

            bool result = true;
#if RELEASE
            try
            {
                var mimeMsg = CreateEmailMessage(fromName, CcEmail, toEmail, subject, message, emailFormat, addImgFooter);
                using (var client = new SmtpClient())
                {
                    //client.LocalDomain = "tnt.com";
                    client.Connect(mimeMsg.Host, mimeMsg.Port, SecureSocketOptions.None);

                    if (mimeMsg.IsSmtpRequredAuthentication)
                    {
                        client.AuthenticationMechanisms.Remove("XOAUTH2");
                        client.Authenticate(mimeMsg.TntEmail, mimeMsg.Password);
                    }
                    client.Send(mimeMsg);
                    client.Disconnect(true);
                }
            }
            catch
            {
                result = false;
            }
#endif
            return result;
        }

        private ExtMimeMessage CreateEmailMessage(string fromName, string CcEmail, string toEmail, string subject, string message, EmailFormat emailFormat, bool addImgFooter)
        {

            int port;
            int.TryParse(configuration["Data:SMTP:port"], out port);

            bool isSmtpRequredAuthentication;
            bool.TryParse(configuration["Data:SMTP:isSmtpRequredAuthentication"], out isSmtpRequredAuthentication);

            var emailMessage = new ExtMimeMessage(configuration["Data:SMTP:host"], port, configuration["Data:SMTP:email"], configuration["Data:SMTP:password"], isSmtpRequredAuthentication);

            emailMessage.From.Add(new MailboxAddress(fromName, emailMessage.TntEmail));
            if (!String.IsNullOrWhiteSpace(CcEmail))
            {
                emailMessage.Cc.Add(new MailboxAddress("", CcEmail));
            }

            emailMessage.To.Add(new MailboxAddress("", toEmail));
            emailMessage.Subject = subject;

            emailMessage.Body = getBodyMessage(emailFormat, message, addImgFooter);

            return emailMessage;
        }

        private class ExtMimeMessage : MimeMessage
        {
            public readonly string Host;
            public readonly int Port;
            public readonly string TntEmail;
            public readonly string Password;
            public readonly bool IsSmtpRequredAuthentication;

            public ExtMimeMessage(string host, int port, string tntEmail, string password, bool isSmtpRequredAuthentication)
            {
                this.Host = host;
                this.Port = port;
                this.TntEmail = tntEmail;
                this.Password = password;
                this.IsSmtpRequredAuthentication = isSmtpRequredAuthentication;
            }
        }


        private MimeEntity getBodyMessage(EmailFormat emailFormat, string message, bool addImgFooter)
        {
            MimeEntity result;

            switch (emailFormat)
            {
                case EmailFormat.html:
                    string text = message;
                    if (addImgFooter)
                    {
                        var footer = AddFotterWithImage();
                        footer.HtmlBody = text + footer.HtmlBody;
                        return result = footer.ToMessageBody();
                    }
                    return result = new TextPart(emailFormat.ToString()) { Text = text };

                case EmailFormat.plain:
                    result = new TextPart(EmailFormat.plain.ToString()) { Text = message };
                    break;
                default:
                    throw new NotImplementedException("Not recognize format");
            }

            return result;
        }

        private BodyBuilder AddFotterWithImage()
        {
            string path = Path.Combine(env.WebRootPath, "app\\assets\\images\\logo.png");
            var builder = new BodyBuilder();
            var image = builder.LinkedResources.Add(path);
            image.ContentId = MimeUtils.GenerateMessageId();
            builder.HtmlBody = "<br><img src = \"cid:" + $"{image.ContentId}" + "\">";
            return builder;
        }
    }
}
