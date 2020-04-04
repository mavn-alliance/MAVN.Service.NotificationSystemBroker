using System;
using Lykke.HttpClientGenerator.Infrastructure;
using Lykke.HttpClientGenerator.Retries;

namespace MAVN.Service.NotificationSystemBroker.SmsProviderClient
{
    /// <summary>
    /// SMS provider API client
    /// </summary>
    public class SmsProviderClient : ISmsProviderClient
    {
        private readonly string _serviceUrl;
        private readonly TimeSpan _timeout;
        private readonly int _retriesCount;

        /// <summary>
        /// API interface to deal with SMSs
        /// </summary>
        public ISmsProviderApi Api { get; private set; }

        /// <summary>
        /// SMS provider constructor
        /// </summary>
        /// <param name="serviceUrl">URL of the service we want to use</param>
        /// <param name="timeout">Client timeout</param>
        /// <param name="retriesCount">Number of retries per request that should be executed before request fails</param>
        public SmsProviderClient(string serviceUrl, TimeSpan timeout, int retriesCount)
        {
            _serviceUrl = serviceUrl;
            _timeout = timeout;
            _retriesCount = retriesCount;

            InitializeClient();
        }

        private void InitializeClient()
        {
            var clientBuilder = HttpClientGenerator.HttpClientGenerator.BuildForUrl(_serviceUrl)
                .WithAdditionalCallsWrapper(new ExceptionHandlerCallsWrapper())
                .WithRetriesStrategy(new LinearRetryStrategy(_timeout, _retriesCount));

            Api = clientBuilder.Create().Generate<ISmsProviderApi>();
        }
    }
}
