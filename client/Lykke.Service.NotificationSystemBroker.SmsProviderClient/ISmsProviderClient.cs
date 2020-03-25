namespace Lykke.Service.NotificationSystemBroker.SmsProviderClient
{
    /// <summary>
    /// SMS provider API client
    /// </summary>
    public interface ISmsProviderClient
    {
        /// <summary>
        /// API interface to deal with SMSs
        /// </summary>
        ISmsProviderApi Api { get; }
    }
}
