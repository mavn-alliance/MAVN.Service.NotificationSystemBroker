using System;
using Lykke.Service.NotificationSystemBroker.Domain.Entities;

namespace Lykke.Service.NotificationSystemBroker.DomainServices.Entities
{
    public class EmailMessage : IEmailMessage
    {
        public Guid MessageId { get; set; }
        public string Email { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
