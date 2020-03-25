using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Lykke.Service.NotificationSystemBroker.MsSqlRepositories.Entities
{
    public class EntityBase
    {
        // This is the base class for all entities.
        [Key]
        [Column("id")]
        public Guid Id { get; set; }
    }
}
