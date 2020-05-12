using Lykke.Common.Log;
using Lykke.RabbitMqBroker.Publisher;
using MAVN.Service.NotificationSystem.Contract.MessageContracts;
using MAVN.Service.NotificationSystemBroker.Domain.RabbitPublishers;

namespace MAVN.Service.NotificationSystemBroker.DomainServices.RabbitPublishers
{
    public class EmailMessagePublisher : JsonRabbitPublisher<BrokerMessage>, IEmailMessagePublisher
    {
        public EmailMessagePublisher(
            ILogFactory logFactory,
            string connectionString,
            string exchangeName)
            : base(logFactory, connectionString, exchangeName)
        {
        }
    }
}
