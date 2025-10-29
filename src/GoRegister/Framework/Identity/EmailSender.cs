using GoRegister.ApplicationCore.Services.Email;
using Microsoft.AspNetCore.Identity.UI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoRegister.Framework.Identity
{
    public class EmailSender : IEmailSender
    {
        private readonly IEmailSendingService _emailSendingService;

        public EmailSender(IEmailSendingService emailSendingService)
        {
            _emailSendingService = emailSendingService;
        }

        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var emailObject = new EmailObject
            {
                FromEmail = "goregister-noreply@notifications.amexgbt.com",
                FromEmailDisplayName = "GoRegister",
                Subject = subject,
                Body = htmlMessage,
                To = email
            };

            await _emailSendingService.Send(emailObject);
        }
    }
}
