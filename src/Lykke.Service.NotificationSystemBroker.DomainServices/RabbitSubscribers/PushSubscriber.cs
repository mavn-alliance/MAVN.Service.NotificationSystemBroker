using System.Threading.Tasks;
using Lykke.Common.Log;
using Lykke.RabbitMqBroker.Subscriber;
using Lykke.Service.NotificationSystem.Contract.MessageContracts;
using Lykke.Service.NotificationSystemBroker.Domain.Services;

namespace Lykke.Service.NotificationSystemBroker.DomainServices.RabbitSubscribers
{
    public class PushSubscriber : JsonRabbitSubscriber<BrokerMessage>
    {
        private readonly IPushNotificationService _pushNotificationService;

        public PushSubscriber(
            string connectionString,
            string exchangeName,
            string queueName,
            IPushNotificationService pushNotificationService,
            ILogFactory logFactory)
            : base(connectionString, exchangeName, queueName, logFactory)
        {
            _pushNotificationService = pushNotificationService;
        }

        protected override async Task ProcessMessageAsync(BrokerMessage message)
        {
            await _pushNotificationService.ProcessPushNotificationAsync(
                message.MessageId,
                message.Properties["PushRegistrationId"],
                message.Properties["Message"],
                message.Properties["CustomPayload"],
                message.Properties["MessageGroupId"],
                message.MessageParameters);
        }
    }
}
