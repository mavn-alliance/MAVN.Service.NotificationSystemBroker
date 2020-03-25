using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Lykke.Logs;
using Lykke.RabbitMqBroker.Publisher;
using Lykke.Service.NotificationSystemBroker.Contract;
using Lykke.Service.NotificationSystemBroker.Domain.Entities;
using Lykke.Service.NotificationSystemBroker.Domain.Services;
using Lykke.Service.NotificationSystemBroker.DomainServices.Services;
using Lykke.Service.NotificationSystemBroker.SmsProviderClient;
using Moq;
using Xunit;

namespace Lykke.Service.NotificationSystemBroker.Tests
{
    public class SmsServiceTests
    {
        private readonly Mock<ISmsProviderRulesService> _smsProviderRulesServiceMock =
            new Mock<ISmsProviderRulesService>();

        private readonly Mock<ISmsProviderClientFactory> _smsProviderClientFactory =
            new Mock<ISmsProviderClientFactory>();

        private readonly Mock<IRabbitPublisher<UpdateAuditMessageEvent>> _auditMessagePublisherMock =
            new Mock<IRabbitPublisher<UpdateAuditMessageEvent>>();

        private readonly ISmsService _service;

        public SmsServiceTests()
        {
            _service = new SmsService(
                _smsProviderRulesServiceMock.Object,
                _smsProviderClientFactory.Object,
                EmptyLogFactory.Instance,
                _auditMessagePublisherMock.Object);
        }

        [Fact]
        public async Task When_Process_Sms_Message_Async_Is_Executed_Expect_That_Sms_Provider_Rule_Service_Is_Called()
        {
            var rule = new ServiceProviderRule
            {
                Code = "123456", SmsProviders = new List<ServiceProvider>
                {
                    new ServiceProvider {Name = "test", ServiceUrl = "http://test.com"}
                }
            };

            _smsProviderRulesServiceMock.Setup(x => x.GetSmsProviderRule(It.IsAny<string>()))
                .Returns(rule);

            var phone = "123456";
            var text = "test message";

            await _service.ProcessSmsMessageAsync(phone, text, Guid.NewGuid());

            _smsProviderRulesServiceMock.Verify(x => x.GetSmsProviderRule(phone), Times.Once());
        }
    }
}
