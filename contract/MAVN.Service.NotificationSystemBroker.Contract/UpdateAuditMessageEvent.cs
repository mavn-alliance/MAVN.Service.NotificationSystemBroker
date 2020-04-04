using System;
using MAVN.Service.NotificationSystemBroker.Contract.Enums;

namespace MAVN.Service.NotificationSystemBroker.Contract
{
    /// <summary>
    /// Represents audit message update event
    /// </summary>
    public class UpdateAuditMessageEvent
    {
        /// <summary>
        /// Represents the id of a message that is audited
        /// </summary>
        public string MessageId { get; set; }

        /// <summary>
        /// Represents the date when update happened
        /// </summary>
        public DateTime SentTimestamp { get; set; }

        /// <summary>
        /// Represents delivery status of the message
        /// </summary>
        public DeliveryStatus DeliveryStatus { get; set; }

        /// <summary>
        /// Represents additional comment with details in case delivery failed
        /// </summary>
        public string DeliveryComment { get; set; }
    }
}
