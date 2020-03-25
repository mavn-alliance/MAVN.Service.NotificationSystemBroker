using Lykke.Common.Log;
using Lykke.RabbitMqBroker.Publisher;
using Lykke.Service.NotificationSystem.Contract.MessageContracts;
using Lykke.Service.NotificationSystemBroker.Domain.RabbitPublishers;

namespace Lykke.Service.NotificationSystemBroker.DomainServices.RabbitPublishers
{
    public class SmsMessagePublisher : JsonRabbitPublisher<BrokerMessage>, ISmsMessagePublisher
    {
        public SmsMessagePublisher(
            ILogFactory logFactory,
            string connectionString,
            string exchangeName)
            : base(logFactory, connectionString, exchangeName)
        {
        }
    }
}
