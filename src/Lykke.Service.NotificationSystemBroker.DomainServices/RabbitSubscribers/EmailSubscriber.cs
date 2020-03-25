using System.Threading.Tasks;
using Lykke.Common.Log;
using Lykke.RabbitMqBroker.Subscriber;
using Lykke.Service.NotificationSystem.Contract.MessageContracts;
using Lykke.Service.NotificationSystemBroker.Domain.Services;

namespace Lykke.Service.NotificationSystemBroker.DomainServices.RabbitSubscribers
{
    public class EmailSubscriber : JsonRabbitSubscriber<BrokerMessage>
    {
        private readonly ISmtpService _smtpService;

        public EmailSubscriber(
            string connectionString,
            string exchangeName,
            string queueName,
            ISmtpService smtpService,
            ILogFactory logFactory)
            : base(connectionString, exchangeName, queueName, logFactory)
        {
            _smtpService = smtpService;
        }

        protected override async Task ProcessMessageAsync(BrokerMessage message)
        {
            await _smtpService.ProcessMessageAsync(
                message.Properties["Email"],
                message.Properties["Subject"],
                message.Properties["Body"],
                message.MessageId);
        }
    }
}
