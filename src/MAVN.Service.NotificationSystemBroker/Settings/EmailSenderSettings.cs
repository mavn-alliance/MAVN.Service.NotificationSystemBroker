using MAVN.Service.NotificationSystemBroker.Domain.Enums;

namespace MAVN.Service.NotificationSystemBroker.Settings
{
    public class EmailSenderSettings
    {
        public SmtpSettings Smtp { get; set; }

        public EmailSenderWorkMode WorkMode { get; set; }
    }
}
