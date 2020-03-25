namespace Lykke.Service.NotificationSystemBroker.PushProviderClient.Models.Responses
{
    /// <summary>
    /// Represents send push notification response 
    /// </summary>
    public class SendPushNotificationResponse
    {
        /// <summary>
        /// Result of an operation
        /// </summary>
        public ResultCode Result { get; set; }

        /// <summary>
        /// Error message. Filled only when there is an error
        /// </summary>
        public string ErrorMessage { get; set; }
    }
}
