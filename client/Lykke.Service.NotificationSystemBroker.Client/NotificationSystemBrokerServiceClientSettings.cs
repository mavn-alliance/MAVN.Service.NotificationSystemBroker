using Lykke.SettingsReader.Attributes;

namespace Lykke.Service.NotificationSystemBroker.Client
{
    /// <summary>
    /// NotificationSystemBroker client settings.
    /// </summary>
    public class NotificationSystemBrokerServiceClientSettings
    {
        /// <summary>Service url.</summary>
        [HttpCheck("api/isalive")]
        public string ServiceUrl { get; set; }
    }
}
