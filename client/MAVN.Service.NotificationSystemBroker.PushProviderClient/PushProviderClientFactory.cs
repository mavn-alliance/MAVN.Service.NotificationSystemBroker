using System;

namespace MAVN.Service.NotificationSystemBroker.PushProviderClient
{
    /// <summary>
    /// Factory to create new Push Provider clients
    /// </summary>
    public class PushProviderClientFactory : IPushProviderClientFactory
    {
        private readonly TimeSpan _timeout;
        private readonly int _retriesCount;

        /// <summary>
        /// Push provider factory constructor
        /// </summary>
        /// <param name="timeout">Client timeout in milliseconds</param>
        /// <param name="retriesCount">Number of retries per request that should be executed before request fails</param>
        public PushProviderClientFactory(TimeSpan timeout, int retriesCount)
        {
            _timeout = timeout;
            _retriesCount = retriesCount;
        }

        /// <summary>
        /// Creates the Push service provider client
        /// </summary>
        /// <param name="serviceUrl">URL of the service to use</param>
        /// <returns>Push service provider client</returns>
        public IPushProviderClient CreateClient(string serviceUrl)
        {
            return new PushProviderClient(serviceUrl, _timeout, _retriesCount);
        }
    }
}
