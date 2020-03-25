namespace Lykke.Service.NotificationSystemBroker.SmsProviderClient
{
    /// <summary>
    /// Factory to create new SMS Provider clients
    /// </summary>
    public interface ISmsProviderClientFactory
    {
        /// <summary>
        /// Creates the SMS service provider client
        /// </summary>
        /// <param name="serviceUrl">URL of the service to use</param>
        /// <returns>SMS service provider client</returns>
        ISmsProviderClient CreateClient(string serviceUrl);
    }
}
