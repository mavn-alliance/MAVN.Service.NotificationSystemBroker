using Lykke.HttpClientGenerator;

namespace Lykke.Service.NotificationSystemBroker.Client
{
    /// <summary>
    /// NotificationSystemBroker API aggregating interface.
    /// </summary>
    public class NotificationSystemBrokerClient : INotificationSystemBrokerClient
    {
        /// <summary>Interface to NotificationSystemBroker API.</summary>
        public IEmailMessagesApi Api { get; private set; }

        /// <summary>C-tor</summary>
        public NotificationSystemBrokerClient(IHttpClientGenerator httpClientGenerator)
        {
            Api = httpClientGenerator.Generate<IEmailMessagesApi>();
        }
    }
}
