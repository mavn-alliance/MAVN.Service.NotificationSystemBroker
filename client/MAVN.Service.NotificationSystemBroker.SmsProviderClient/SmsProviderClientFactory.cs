using System;

namespace MAVN.Service.NotificationSystemBroker.SmsProviderClient
{
    /// <summary>
    /// Factory to create new SMS Provider clients
    /// </summary>
    public class SmsProviderClientFactory : ISmsProviderClientFactory
    {
        private readonly TimeSpan _timeout;
        private readonly int _retriesCount;

        /// <summary>
        /// SMS provider factory constructor
        /// </summary>
        /// <param name="timeout">Client timeout in milliseconds</param>
        /// <param name="retriesCount">Number of retries per request that should be executed before request fails</param>
        public SmsProviderClientFactory(TimeSpan timeout, int retriesCount)
        {
            _timeout = timeout;
            _retriesCount = retriesCount;
        }

        /// <summary>
        /// Creates the SMS service provider client
        /// </summary>
        /// <param name="serviceUrl">URL of the service to use</param>
        /// <returns>SMS service provider client</returns>
        public ISmsProviderClient CreateClient(string serviceUrl)
        {
            return new SmsProviderClient(serviceUrl, _timeout, _retriesCount);
        }
    }
}
