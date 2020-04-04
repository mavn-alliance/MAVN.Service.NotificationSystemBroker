using System.Data.Common;
using JetBrains.Annotations;
using Lykke.Common.MsSql;
using MAVN.Service.NotificationSystemBroker.MsSqlRepositories.Entities;
using Microsoft.EntityFrameworkCore;

namespace MAVN.Service.NotificationSystemBroker.MsSqlRepositories
{
    public class NotificationSystemBrokerContext : MsSqlContext
    {
        private const string Schema = "notification_system_broker";

        public DbSet<EmailMessageEntity> EmailMessages { get; set; }

        // empty constructor needed for EF migrations
        [UsedImplicitly]
        public NotificationSystemBrokerContext() : base(Schema)
        {
        }

        public NotificationSystemBrokerContext(string connectionString, bool isTraceEnabled)
            : base(Schema, connectionString, isTraceEnabled)
        {
        }

        //Needed constructor for using InMemoryDatabase for tests
        public NotificationSystemBrokerContext(DbContextOptions options)
            : base(Schema, options)
        {
        }

        public NotificationSystemBrokerContext(DbConnection dbConnection)
            : base(Schema, dbConnection)
        {
        }

        protected override void OnLykkeModelCreating(ModelBuilder modelBuilder)
        {
            var emailMessageEntityBuilder = modelBuilder.Entity<EmailMessageEntity>();

            emailMessageEntityBuilder.HasIndex(x => x.Email).IsUnique(false);
        }
    }
}
