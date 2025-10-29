using GoRegister.ApplicationCore.Data.Enums;
using System;

namespace GoRegister.ApplicationCore.Domain.Registration.Models
{
    public class SendEmailDetailsModel
    {
        public Guid BatchId { get; set; }

        public EmailType EmailType { get; set; }

        public int EmailSentCount { get; set; }

        public bool Success { get; set; }

        public string ResultMessage { get; set; }

        public SendEmailModel SendEmailModel { get; set; }
    }
}
