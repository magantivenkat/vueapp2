using Newtonsoft.Json;

namespace GoRegister.ApplicationCore.Domain.Registration.Models
{
    /// <summary>
    /// The emails personalization
    /// </summary>
    public class EmailPersonalization
    {
        /// <summary>
        /// The email audit id
        /// </summary>
        [JsonProperty("emailAuditId")]
        public int EmailAuditId { get; set; }

        /// <summary>
        /// The e-mail subject.
        /// </summary>
        [JsonProperty("subject")]
        public string Subject { get; set; }

        /// <summary>
        /// The e-mail address of the recipient.
        /// </summary>
        [JsonProperty("to")]
        public string To { get; set; }

        /// <summary>
        /// The display name of the recipient. If not specified, the e-mail is sent with e-mail address only.
        /// </summary>
        [JsonProperty("toName")]
        public string ToName { get; set; }

        /// <summary>
        /// The e-mail Cc address.
        /// </summary>
        [JsonProperty("cc")]
        public string Cc { get; set; }

        /// <summary>
        /// The display name of the Cc. 
        /// </summary>
        [JsonProperty("ccName")]
        public string CcName { get; set; }

        /// <summary>
        /// The e-mail Bcc address.
        /// </summary>
        [JsonProperty("bcc")]
        public string Bcc { get; set; }

        /// <summary>
        /// The display name of the Bcc. 
        /// </summary>
        [JsonProperty("bccName")]
        public string BccName { get; set; }

        /// <summary>
        /// The first name of the recipient.
        /// </summary>
        [JsonProperty("firstName")]
        public string FirstName { get; set; }

        /// <summary>
        /// The last name of the recipient.
        /// </summary>
        [JsonProperty("lastName")]
        public string LastName { get; set; }
    }
}