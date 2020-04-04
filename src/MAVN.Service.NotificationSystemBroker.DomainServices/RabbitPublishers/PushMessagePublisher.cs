using Lykke.Common.Log;
using Lykke.RabbitMqBroker.Publisher;
using Lykke.Service.NotificationSystem.Contract.MessageContracts;
using MAVN.Service.NotificationSystemBroker.Domain.RabbitPublishers;

namespace MAVN.Service.NotificationSystemBroker.DomainServices.RabbitPublishers
{
    public class PushMessagePublisher : JsonRabbitPublisher<BrokerMessage>, IPushMessagePublisher
    {
        public PushMessagePublisher(
            ILogFactory logFactory,
            string connectionString,
            string exchangeName)
            : base(logFactory, connectionString, exchangeName)
        {
        }
    }
}
