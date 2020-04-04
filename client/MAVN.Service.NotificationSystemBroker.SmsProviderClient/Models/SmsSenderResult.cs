using MAVN.Service.NotificationSystemBroker.SmsProviderClient.Enums;

namespace MAVN.Service.NotificationSystemBroker.SmsProviderClient
{
    /// <summary>
    /// The result returned after sending an sms
    /// </summary>
    public class SmsSenderResult
    {
        /// <summary>
        /// Result enum containing status - Ok, Error
        /// </summary>
        public SmsResult Result { get; set; }

        /// <summary>
        /// Optional error message if status is Error
        /// </summary>
        public string ErrorMessage { get; set; }
    }
}
