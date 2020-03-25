using System.Threading.Tasks;
using Common.Log;
using Lykke.Common.Log;
using Lykke.RabbitMqBroker.Subscriber;
using Lykke.Service.NotificationSystem.Contract.Enums;
using Lykke.Service.NotificationSystem.Contract.MessageContracts;
using Lykke.Service.NotificationSystemBroker.Domain.RabbitPublishers;

namespace Lykke.Service.NotificationSystemBroker.DomainServices.RabbitSubscribers
{
    public class NotificationSystemSubscriber : JsonRabbitSubscriber<BrokerMessage>
    {
        private readonly ILog _log;
        private readonly ISmsMessagePublisher _smsMessagePublisher;
        private readonly IEmailMessagePublisher _emailMessagePublisher;
        private readonly IPushMessagePublisher _pushMessagePublisher;

        public NotificationSystemSubscriber(
            ISmsMessagePublisher smsMessagePublisher,
            IEmailMessagePublisher emailMessagePublisher,
            IPushMessagePublisher pushMessagePublisher,
            string connectionString,
            string exchangeName,
            string queueName,
            ILogFactory logFactory)
            : base(connectionString, exchangeName, queueName, logFactory)
        {
            _smsMessagePublisher = smsMessagePublisher;
            _emailMessagePublisher = emailMessagePublisher;
            _pushMessagePublisher = pushMessagePublisher;

            _log = logFactory.CreateLog(this);
        }

        protected override Task ProcessMessageAsync(BrokerMessage message)
        {
            switch (message.Channel)
            {
                case Channel.Email:
                    return _emailMessagePublisher.PublishAsync(message);
                case Channel.Sms:
                    return _smsMessagePublisher.PublishAsync(message);
                case Channel.PushNotification:
                    return _pushMessagePublisher.PublishAsync(message);
                default:
                    _log.Warning($"Message could not be handled for channel {message.Channel.ToString()}");
                    return Task.CompletedTask;
            }
        }
    }
}
