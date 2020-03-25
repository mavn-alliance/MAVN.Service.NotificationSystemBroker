using Lykke.Service.NotificationSystemBroker.Domain.Enums;

namespace Lykke.Service.NotificationSystemBroker.Settings
{
    public class EmailSenderSettings
    {
        public SmtpSettings Smtp { get; set; }

        public EmailSenderWorkMode WorkMode { get; set; }
    }
}
