using Lykke.SettingsReader.Attributes;

namespace Lykke.Service.NotificationSystemBroker.Settings
{
    public class DbSettings
    {
        [AzureTableCheck]
        public string LogsConnString { get; set; }

        public string DataConnString { get; set; }
    }
}
