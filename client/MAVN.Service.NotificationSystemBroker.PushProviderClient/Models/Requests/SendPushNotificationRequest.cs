using System.Collections.Generic;

namespace MAVN.Service.NotificationSystemBroker.PushProviderClient.Models.Requests
{
    /// <summary>
    /// Represents send push notification request
    /// </summary>
    public class SendPushNotificationRequest
    {
        /// <summary>
        /// Id of the message that we are going to push
        /// </summary>
        public string MessageId { get; set; }

        /// <summary>
        /// Unique value to identity push registration in order to push message
        /// </summary>
        public string PushRegistrationId { get; set; }

        /// <summary>
        /// Test of the message that we want to push
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Additional message values that we want to push
        /// </summary>
        public Dictionary<string, string> CustomPayload { get; set; }
    }
}
