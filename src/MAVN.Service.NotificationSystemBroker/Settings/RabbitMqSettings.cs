using Lykke.SettingsReader.Attributes;

namespace MAVN.Service.NotificationSystemBroker.Settings
{
    public class RabbitMqSettings
    {
        [AmqpCheck]
        public string ConnectionString { get; set; }
    }
}
