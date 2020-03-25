using Autofac;
using Lykke.Common.MsSql;
using Lykke.Service.NotificationSystemBroker.Domain.Repositories;
using Lykke.Service.NotificationSystemBroker.MsSqlRepositories.Repositories;

namespace Lykke.Service.NotificationSystemBroker.MsSqlRepositories
{
    public class AutofacModule : Module
    {
        private readonly string _connectionString;

        public AutofacModule(string connectionString)
        {
            _connectionString = connectionString;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterMsSql(
                _connectionString,
                connString => new NotificationSystemBrokerContext(connString, false),
                dbConn => new NotificationSystemBrokerContext(dbConn));

            builder.RegisterType<EmailMessageRepository>()
                .As<IEmailMessageRepository>()
                .SingleInstance();
        }

    }
}
