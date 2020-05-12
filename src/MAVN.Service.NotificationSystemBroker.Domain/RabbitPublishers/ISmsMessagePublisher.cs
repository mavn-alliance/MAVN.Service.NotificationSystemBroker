using Lykke.RabbitMqBroker.Publisher;
using MAVN.Service.NotificationSystem.Contract.MessageContracts;

namespace MAVN.Service.NotificationSystemBroker.Domain.RabbitPublishers
{
    public interface ISmsMessagePublisher : IRabbitPublisher<BrokerMessage>
    {
    }
}
