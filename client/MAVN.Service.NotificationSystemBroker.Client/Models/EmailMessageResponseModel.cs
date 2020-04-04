using System;

namespace MAVN.Service.NotificationSystemBroker.Client.Models
{
    /// <summary>
    /// Represents an EmailMessage
    /// </summary>
    public class EmailMessageResponseModel 
    {
        /// <summary>
        /// The MessageId of the message
        /// </summary>
        public Guid MessageId { get; set; }

        /// <summary>
        /// The Email of the recipient
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// The Subject of the email
        /// </summary>
        public string Subject { get; set; }

        /// <summary>
        /// The Body of the email
        /// </summary>
        public string Body { get; set; }

        /// <summary>
        /// The time the message was sent
        /// </summary>
        public DateTime Timestamp { get; set; }
    }
}
