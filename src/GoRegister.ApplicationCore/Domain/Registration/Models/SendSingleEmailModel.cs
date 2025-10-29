using Newtonsoft.Json;

namespace GoRegister.ApplicationCore.Domain.Registration.Models
{
    public class SendSingleEmailModel
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
        /// The display name of the receiver.
        /// </summary>
        [JsonProperty("toName")]
        public string ToName { get; set; }

        /// <summary>
        /// The e-mail address of the receiver.
        /// </summary>
        [JsonProperty("to")]
        public string To { get; set; }

        /// <summary>
        /// The e-mail subject.
        /// </summary>
        [JsonProperty("subject")]
        public string Subject { get; set; }

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

    }
}