using Lykke.RabbitMqBroker.Publisher;
using Lykke.Service.NotificationSystem.Contract.MessageContracts;

namespace Lykke.Service.NotificationSystemBroker.Domain.RabbitPublishers
{
    public interface ISmsMessagePublisher : IRabbitPublisher<BrokerMessage>
    {
    }
}
