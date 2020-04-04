using System;
using Lykke.HttpClientGenerator;
using Lykke.HttpClientGenerator.Infrastructure;
using Lykke.HttpClientGenerator.Retries;

namespace MAVN.Service.NotificationSystemBroker.PushProviderClient
{
    /// <summary>
    /// PushProvider API aggregating interface.
    /// </summary>
    public class PushProviderClient : IPushProviderClient
    {
        private readonly string _serviceUrl;
        private readonly TimeSpan _timeout;
        private readonly int _retriesCount;

        /// <summary>Inerface to PushProvider Api.</summary>
        public IPushProviderApi Api { get; private set; }

        /// <summary>C-tor</summary>
        public PushProviderClient(IHttpClientGenerator httpClientGenerator)
        {
            Api = httpClientGenerator.Generate<IPushProviderApi>();
        }

        /// <summary>
        /// SMS provider constructor
        /// </summary>
        /// <param name="serviceUrl">URL of the service we want to use</param>
        /// <param name="timeout">Client timeout</param>
        /// <param name="retriesCount">Number of retries per request that should be executed before request fails</param>
        public PushProviderClient(string serviceUrl, TimeSpan timeout, int retriesCount)
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

            Api = clientBuilder.Create().Generate<IPushProviderApi>();
        }
    }
}
