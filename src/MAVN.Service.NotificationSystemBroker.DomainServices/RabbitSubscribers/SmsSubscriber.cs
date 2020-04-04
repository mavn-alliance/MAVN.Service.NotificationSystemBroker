using System.Threading.Tasks;
using Lykke.Common.Log;
using Lykke.RabbitMqBroker.Subscriber;
using Lykke.Service.NotificationSystem.Contract.MessageContracts;
using MAVN.Service.NotificationSystemBroker.Domain.Services;

namespace MAVN.Service.NotificationSystemBroker.DomainServices.RabbitSubscribers
{
    public class SmsSubscriber : JsonRabbitSubscriber<BrokerMessage>
    {
        private readonly ISmsService _smsService;

        public SmsSubscriber(
            string connectionString,
            string exchangeName,
            string queueName,
            ISmsService smsService,
            ILogFactory logFactory)
            : base(connectionString, exchangeName, queueName, logFactory)
        {
            _smsService = smsService;
        }

        protected override async Task ProcessMessageAsync(BrokerMessage message)
        {
            await _smsService.ProcessSmsMessageAsync(
                message.Properties["PhoneNumber"],
                message.Properties["Message"],
                message.MessageId);
        }
    }
}
