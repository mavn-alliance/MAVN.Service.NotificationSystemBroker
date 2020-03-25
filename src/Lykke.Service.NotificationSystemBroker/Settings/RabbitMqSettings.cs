using Lykke.SettingsReader.Attributes;

namespace Lykke.Service.NotificationSystemBroker.Settings
{
    public class RabbitMqSettings
    {
        [AmqpCheck]
        public string ConnectionString { get; set; }
    }
}
