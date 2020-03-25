using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Lykke.Service.NotificationSystemBroker.Domain.Entities;

namespace Lykke.Service.NotificationSystemBroker.MsSqlRepositories.Entities
{
    [Table("email_messages")]
    public class EmailMessageEntity : EntityBase, IEmailMessage
    {
        [Column("email")]
        [MaxLength(320)]
        public string Email { get; set; }

        [Column("message_id")]
        public Guid MessageId { get; set; }

        [Column("subject")]
        [MaxLength(1000)]
        public string Subject { get; set; }

        [Column("body")]
        public string Body { get; set; }

        [Column("timestamp")]
        public DateTime Timestamp { get; set; }
    }
}
