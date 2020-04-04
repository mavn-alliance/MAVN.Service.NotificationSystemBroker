using System;

namespace MAVN.Service.NotificationSystemBroker.Domain.Entities
{
    public interface IEmailMessage
    {
        Guid MessageId { get; set; }

        string Email { get; set; }

        string Subject { get; set; }

        string Body { get; set; }

        DateTime Timestamp { get; set; }
    }
}
