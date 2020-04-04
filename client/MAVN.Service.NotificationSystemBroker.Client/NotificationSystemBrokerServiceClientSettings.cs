using Lykke.SettingsReader.Attributes;

namespace MAVN.Service.NotificationSystemBroker.Client
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
