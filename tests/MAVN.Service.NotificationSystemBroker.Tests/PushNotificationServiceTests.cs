using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Lykke.Logs;
using Lykke.RabbitMqBroker.Publisher;
using MAVN.Service.NotificationSystemBroker.Contract;
using MAVN.Service.NotificationSystemBroker.DomainServices.Services;
using MAVN.Service.NotificationSystemBroker.PushProviderClient;
using MAVN.Service.NotificationSystemBroker.PushProviderClient.Models.Requests;
using MAVN.Service.NotificationSystemBroker.PushProviderClient.Models.Responses;
using Moq;
using Newtonsoft.Json;
using Xunit;

namespace MAVN.Service.NotificationSystemBroker.Tests
{
    public class PushNotificationServiceTests
    {
        private readonly PushNotificationService _service;

        private readonly Mock<IRabbitPublisher<UpdateAuditMessageEvent>> _auditPublisherMock =
            new Mock<IRabbitPublisher<UpdateAuditMessageEvent>>();

        private readonly Mock<IPushProviderClient> _pushProviderMock = new Mock<IPushProviderClient>();

        public PushNotificationServiceTests()
        {
            _service = new PushNotificationService(
                _auditPublisherMock.Object,
                _pushProviderMock.Object,
                EmptyLogFactory.Instance);
        }

        [Fact]
        public async Task When_SendPushNotificationAsyncSucceeds_ExpectAllMethodsCalled()
        {
            //Arrange
            var messageId = Guid.NewGuid();
            var message = "test";
            var pushRegistrationId = "aiDi";
            var customPayload = new Dictionary<string, string>
            {
                {"key", "val"}
            };
            
            _pushProviderMock.Setup(x => x.Api.SendPushNotificationAsync(It.IsAny<SendPushNotificationRequest>()))
                .Returns(Task.FromResult(new SendPushNotificationResponse
                {
                    Result = ResultCode.Ok
                }));

            //Act
            await _service.ProcessPushNotificationAsync(
                messageId,
                pushRegistrationId,
                message,
                JsonConvert.SerializeObject(customPayload),
                null,
                new Dictionary<string, string>());

            //Assert
            _pushProviderMock.Verify(x => x.Api.SendPushNotificationAsync(It.IsAny<SendPushNotificationRequest>()), Times.Once());
            _auditPublisherMock.Verify(x => x.PublishAsync(It.IsAny<UpdateAuditMessageEvent>()), Times.Once);
        }

        [Fact]
        public async Task When_SendPushNotificationAsyncFails_ExpectAllMethodsCalled()
        {
            //Arrange
            var messageId = Guid.NewGuid();
            var message = "test";
            var pushRegistrationId = "aiDi";
            var customPayload = new Dictionary<string, string>
            {
                {"key", "val"}
            };
            
            _pushProviderMock.Setup(x => x.Api.SendPushNotificationAsync(It.IsAny<SendPushNotificationRequest>()))
                .Returns(Task.FromResult(new SendPushNotificationResponse
                {
                    Result = ResultCode.Error,
                    ErrorMessage = "ErrorTest"
                }));

            //Act
            await _service.ProcessPushNotificationAsync(
                messageId,
                pushRegistrationId,
                message,
                JsonConvert.SerializeObject(customPayload),
                null,
                new Dictionary<string, string>());

            //Assert
            _pushProviderMock.Verify(x => x.Api.SendPushNotificationAsync(It.IsAny<SendPushNotificationRequest>()), Times.Once());
            _auditPublisherMock.Verify(x => x.PublishAsync(It.IsAny<UpdateAuditMessageEvent>()), Times.Once);
        }
    }
}
