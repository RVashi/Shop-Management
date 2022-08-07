using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using WebAPI.Core.Dto;
using WebAPI.Core.Interfaces.Services;

namespace WebAPI.Infrastructure.Common
{
    public class MailService : IMailService
    {
        private readonly MailSettings _mailSettings;
        public MailService(IOptions<MailSettings> mailSettings)
        {
            _mailSettings = mailSettings.Value;
        }

        public async Task SendEmailAsync(MailRequest mailRequest)
        {
            var email = new MimeMessage();
            email.Sender = MailboxAddress.Parse(_mailSettings.Mail);
            if (String.IsNullOrWhiteSpace(mailRequest.ToEmail))
            {
                email.To.Add(MailboxAddress.Parse(_mailSettings.SupportEmail));
            }
            else
            {
                email.To.Add(MailboxAddress.Parse(mailRequest.ToEmail));
            }
            email.Subject = mailRequest.Subject;
            var builder = new BodyBuilder();
            string template = string.IsNullOrEmpty(mailRequest.templateName) ? "SampleEmailTemplate.cshtml" : mailRequest.templateName;
            var pathToFile =
                     "Templates"
                    + Path.DirectorySeparatorChar.ToString()
                    + template;

            using (StreamReader SourceReader = System.IO.File.OpenText(pathToFile))
            {
                builder.HtmlBody = SourceReader.ReadToEnd();

                builder.HtmlBody = builder.HtmlBody.Replace("{0}", _mailSettings.ConfirmEmailBaseUrl + "XXXX/XXXX?g=" + mailRequest.guidToken);
            }
            email.Body = builder.ToMessageBody();
            using var smtp = new SmtpClient();
            smtp.Connect(_mailSettings.Host, _mailSettings.Port, SecureSocketOptions.StartTls);
            smtp.Authenticate(_mailSettings.Mail, _mailSettings.Password);
            await smtp.SendAsync(email);
            smtp.Disconnect(true);
        }
    }
}
