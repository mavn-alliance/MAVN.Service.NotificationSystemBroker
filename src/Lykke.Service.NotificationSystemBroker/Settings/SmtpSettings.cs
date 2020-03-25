using Lykke.SettingsReader.Attributes;

namespace Lykke.Service.NotificationSystemBroker.Settings
{
    public class SmtpSettings
    {
        public string Host { get; set; }

        public string Port { get; set; }

        [Optional]
        public string Username { get; set; }

        [Optional]
        public string Password { get; set; }

        public string Sender { get; set; }

        [Optional]
        public string SenderName { get; set; }

        public bool UseSsl { get; set; }
    }
}
