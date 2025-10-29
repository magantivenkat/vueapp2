using Newtonsoft.Json;
using System.Collections.Generic;

namespace GoRegister.ApplicationCore.Domain.Registration.Models
{
    public class SendEmailModel
    {
        /// <summary>
        /// The e-mail address of the sender. If not specified, it is resolved from configuration.
        /// </summary>
        [JsonProperty("from")]
        public string From { get; set; }

        /// <summary>
        /// The display name of the sender. If not specified, the name is resolved from configuration.
        /// </summary>
        [JsonProperty("fromName")]
        public string FromName { get; set; }

        /// <summary>
        /// The e-mail body in plain text
        /// </summary>
        [JsonProperty("plainTextContent")]
        public string PlainTextContent { get; set; }

        /// <summary>
        /// The e-mail body in html
        /// </summary>
        [JsonProperty("htmlTextContent")]
        public string HtmlTextContent { get; set; }

        /// <summary>
        /// List of personalization for each message
        /// </summary>
        [JsonProperty("emailPersonalization")]
        public List<EmailPersonalization> EmailPersonalization { get; set; }

    }
}
