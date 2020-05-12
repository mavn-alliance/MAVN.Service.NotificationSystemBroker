using System.Threading.Tasks;
using Lykke.Common.Log;
using Lykke.RabbitMqBroker.Subscriber;
using MAVN.Service.NotificationSystem.Contract.MessageContracts;
using MAVN.Service.NotificationSystemBroker.Domain.Services;

namespace MAVN.Service.NotificationSystemBroker.DomainServices.RabbitSubscribers
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
