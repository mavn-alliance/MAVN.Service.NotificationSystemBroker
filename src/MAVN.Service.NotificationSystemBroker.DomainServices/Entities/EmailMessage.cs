using System;
using MAVN.Service.NotificationSystemBroker.Domain.Entities;

namespace MAVN.Service.NotificationSystemBroker.DomainServices.Entities
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
