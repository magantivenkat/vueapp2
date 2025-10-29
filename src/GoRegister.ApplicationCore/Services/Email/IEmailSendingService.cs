using GoRegister.ApplicationCore.Framework.Domain;
using GoRegister.ApplicationCore.Services.FileStorage;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MimeKit;
using System;
using System.Threading.Tasks;

namespace GoRegister.ApplicationCore.Services.Email
{
    public interface IEmailSendingService
    {
        Task<Result> Send(EmailObject emailObject);

        Task<Result> SendWithAttachments(EmailObject emailObject);
    }

    public class SmtpEmailSendingService : IEmailSendingService
    {
        private readonly ILogger<SmtpEmailSendingService> _logger;
        private readonly IConfiguration _configuration;
        private readonly IFileStorage _fileStorage;

        public SmtpEmailSendingService(ILogger<SmtpEmailSendingService> logger, IConfiguration configuration, IFileStorage fileStorage)
        {
            _logger = logger;
            _configuration = configuration;
            _fileStorage = fileStorage;
        }

        public async Task<Result> Send(EmailObject emailObject)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(emailObject.FromEmailDisplayName, emailObject.FromEmail));

            string[] toEmails = emailObject.To.Split(";");

            foreach (var item in toEmails)
            {
                //message.To.Add(new MailboxAddress(item));
                message.To.Add(MailboxAddress.Parse(item));
            }

            if (!string.IsNullOrEmpty(emailObject.Cc))
            {
                string[] cCEmails = emailObject.Cc.Split(";");

                foreach (var itemcc in cCEmails)
                {
                    message.Cc.Add(MailboxAddress.Parse(itemcc));
                }
            }
            if (!string.IsNullOrEmpty(emailObject.Bcc))
            {
                string[] bcCEmails = emailObject.Bcc.Split(";");

                foreach (var itembcc in bcCEmails)
                {
                    message.Bcc.Add(MailboxAddress.Parse(itembcc));
                }
            }

            message.Subject = emailObject.Subject;

            var bodyBuilder = new BodyBuilder();
            bodyBuilder.HtmlBody = emailObject.Body; // currently only support html body

            message.Body = bodyBuilder.ToMessageBody();

            var smtpSettings = _configuration.GetSection("SmtpSettings").Get<SmtpSettings>();

            try
            {
                using (var client = new SmtpClient())
                {
                    await client.ConnectAsync(smtpSettings.Host, smtpSettings.Port, smtpSettings.UseSsl);

                    if (smtpSettings.UseAuthentication)
                    {
                        await client.AuthenticateAsync(smtpSettings.UserName, smtpSettings.Password);
                    }

                    await client.SendAsync(message);
                    client.Disconnect(true);
                    return Result.Ok();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);

                return Result.Fail();
            }
        }

        public async Task<Result> SendWithAttachments(EmailObject emailObject)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(emailObject.FromEmailDisplayName, emailObject.FromEmail));

            string[] toEmails = emailObject.To.Split(";");

            foreach (var item in toEmails)
            {
                message.To.Add(new MailboxAddress(item));
            }

            if (!string.IsNullOrEmpty(emailObject.Cc))
            {
                string[] cCEmails = emailObject.Cc.Split(";");

                foreach (var itemcc in cCEmails)
                {
                    message.Cc.Add(MailboxAddress.Parse(itemcc));
                }
            }
            if (!string.IsNullOrEmpty(emailObject.Bcc))
            {
                string[] bcCEmails = emailObject.Bcc.Split(";");

                foreach (var itembcc in bcCEmails)
                {
                    message.Bcc.Add(MailboxAddress.Parse(itembcc));
                }
            }

            message.Subject = emailObject.Subject;

            var bodyBuilder = new BodyBuilder();
            bodyBuilder.HtmlBody = emailObject.Body; // currently only support html body

            if (emailObject.AttachmentFilePaths != null)
            {
                if (emailObject.AttachmentFilePaths.Length > 0)
                {
                    foreach (string attachmentPath in emailObject.AttachmentFilePaths)
                    { 
                        bodyBuilder.Attachments.Add(attachmentPath);                            
                    }
                }
            }

            message.Body = bodyBuilder.ToMessageBody();

            var smtpSettings = _configuration.GetSection("SmtpSettings").Get<SmtpSettings>();

            try
            {
                using (var client = new SmtpClient())
                {
                    await client.ConnectAsync(smtpSettings.Host, smtpSettings.Port, smtpSettings.UseSsl);

                    if (smtpSettings.UseAuthentication)
                    {
                        await client.AuthenticateAsync(smtpSettings.UserName, smtpSettings.Password);
                    }

                    await client.SendAsync(message);
                    client.Disconnect(true);
                    return Result.Ok();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);

                return Result.Fail();
            }
        }


    }
}