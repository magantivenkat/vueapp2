using GoRegister.ApplicationCore.Data.Enums;
using System;

namespace GoRegister.ApplicationCore.Domain.Registration.Models
{
    public class EmailAuditBatchModel
    {
        public int Id { get; set; }

        public int ProjectId { get; set; }

        public Guid BatchId { get; set; }

        public string UserId { get; set; }

        public int EmailSentCount { get; set; }

        public string EmailType { get; set; }

        public DateTime DateSent { get; set; }

        public string DateSentFormat { get; set; }

        public string HumanizedDateSent { get; set; }

        public string ToolTipAuditInfo { get; set; }

        public string GetEmailType(EmailType emailType, int emailsentCount)
        {
            string batchEmailType;

            switch (emailType)
            {
                case Data.Enums.EmailType.Invitation:
                    batchEmailType = emailsentCount > 1 ? "invites" : "invite";
                    break;
                case Data.Enums.EmailType.InvitationReminder:
                    batchEmailType = emailsentCount > 1 ? "reminders" : "reminder";
                    break;
                case Data.Enums.EmailType.Confirmation:
                    batchEmailType = emailsentCount > 1 ? "confirmations" : "confirmation";
                    break;
                case Data.Enums.EmailType.Decline:
                    batchEmailType = emailsentCount > 1 ? "declines" : "decline";
                    break;
                case Data.Enums.EmailType.Cancellation:
                    batchEmailType = emailsentCount > 1 ? "cancellations" : "cancellation";
                    break;
                case Data.Enums.EmailType.Waiting:
                    batchEmailType = emailsentCount > 1 ? "waitings" : "waiting";
                    break;
                default:
                    batchEmailType = string.Empty;
                    break;
            }

            return batchEmailType;
        }
    }
}
