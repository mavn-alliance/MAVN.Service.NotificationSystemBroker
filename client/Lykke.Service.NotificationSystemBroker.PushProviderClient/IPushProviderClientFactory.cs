namespace Lykke.Service.NotificationSystemBroker.PushProviderClient
{
    /// <summary>
    /// Factory to create new Push Provider clients
    /// </summary>
    public interface IPushProviderClientFactory
    {
        /// <summary>
        /// Creates the Push service provider client
        /// </summary>
        /// <param name="serviceUrl">URL of the service to use</param>
        /// <returns>Push service provider client</returns>
        IPushProviderClient CreateClient(string serviceUrl);
    }
}
