using System;
using System.ComponentModel.DataAnnotations;

namespace MAVN.Service.NotificationSystemBroker.Contract.SmsProvider
{
    /// <summary>
    /// Represents a request for sms sending
    /// </summary>
    public class SendSmsRequestModel
    {
        /// <summary>
        /// The MessageId of the sms
        /// </summary>
        [Required]
        public Guid MessageId { get; set; }

        /// <summary>
        /// The PhoneNumber to send to
        /// </summary>
        [Required, MaxLength(20)]
        public string PhoneNumber { get; set; }

        /// <summary>
        /// The message to be sent
        /// </summary>
        [MaxLength(10000)]
        public string Message { get; set; }
    }
}
