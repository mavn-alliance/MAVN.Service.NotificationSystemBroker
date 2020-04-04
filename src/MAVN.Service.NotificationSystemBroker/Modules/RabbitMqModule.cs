using Autofac;
using Common;
using JetBrains.Annotations;
using Lykke.RabbitMqBroker.Publisher;
using Lykke.RabbitMqBroker.Subscriber;
using Lykke.Service.NotificationSystem.Contract.MessageContracts;
using MAVN.Service.NotificationSystemBroker.Contract;
using MAVN.Service.NotificationSystemBroker.Domain.RabbitPublishers;
using MAVN.Service.NotificationSystemBroker.DomainServices.RabbitPublishers;
using MAVN.Service.NotificationSystemBroker.DomainServices.RabbitSubscribers;
using MAVN.Service.NotificationSystemBroker.Settings;
using Lykke.SettingsReader;

namespace MAVN.Service.NotificationSystemBroker.Modules
{
    [UsedImplicitly]
    public class RabbitMqModule : Module
    {
        private const string UpdateAuditMessagesExchangeName = "lykke.notificationsystem.updateauditmessage";
        private const string BrokerMessagesExchangeName = "lykke.notificationsystem.brokermessage";
        private const string SmsMessagesExchangeName = "lykke.notificationsystem.sms";
        private const string EmailMessagesExchangeName = "lykke.notificationsystem.email";
        private const string PushMessagesExchangeName = "lykke.notificationsystem.push";
        private const string BrokerQueueName = "notificationsystembroker";

        private readonly RabbitMqSettings _settings;

        public RabbitMqModule(IReloadingManager<AppSettings> appSettings)
        {
            _settings = appSettings.CurrentValue.Rabbit;
        }

        protected override void Load(ContainerBuilder builder)
        {
            RegisterRabbitMqSubscribers(builder);
            RegisterRabbitMqPublishers(builder);
        }

        private void RegisterRabbitMqSubscribers(ContainerBuilder builder)
        {
            builder.RegisterJsonRabbitSubscriber<NotificationSystemSubscriber, BrokerMessage>(
                _settings.ConnectionString,
                BrokerMessagesExchangeName,
                BrokerQueueName);

            builder.RegisterJsonRabbitSubscriber<SmsSubscriber, BrokerMessage>(
                _settings.ConnectionString,
                SmsMessagesExchangeName,
                BrokerQueueName);

            builder.RegisterJsonRabbitSubscriber<EmailSubscriber, BrokerMessage>(
                _settings.ConnectionString,
                EmailMessagesExchangeName,
                BrokerQueueName);

            builder.RegisterJsonRabbitSubscriber<PushSubscriber, BrokerMessage>(
                _settings.ConnectionString,
                PushMessagesExchangeName,
                BrokerQueueName);
        }

        private void RegisterRabbitMqPublishers(ContainerBuilder builder)
        {
            builder.RegisterJsonRabbitPublisher<UpdateAuditMessageEvent>(
                _settings.ConnectionString,
                UpdateAuditMessagesExchangeName);

            builder.RegisterType<SmsMessagePublisher>()
                .As<ISmsMessagePublisher>()
                .As<IStartable>()
                .As<IStopable>()
                .WithParameter("connectionString", _settings.ConnectionString)
                .WithParameter("exchangeName", SmsMessagesExchangeName)
                .SingleInstance();

            builder.RegisterType<EmailMessagePublisher>()
                .As<IEmailMessagePublisher>()
                .As<IStartable>()
                .As<IStopable>()
                .WithParameter("connectionString", _settings.ConnectionString)
                .WithParameter("exchangeName", EmailMessagesExchangeName)
                .SingleInstance();

            builder.RegisterType<PushMessagePublisher>()
                .As<IPushMessagePublisher>()
                .As<IStartable>()
                .As<IStopable>()
                .WithParameter("connectionString", _settings.ConnectionString)
                .WithParameter("exchangeName", PushMessagesExchangeName)
                .SingleInstance();
        }
    }
}
