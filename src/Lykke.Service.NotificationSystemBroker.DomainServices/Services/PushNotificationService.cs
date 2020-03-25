using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Common.Log;
using Lykke.Common.Log;
using Lykke.RabbitMqBroker.Publisher;
using Lykke.Service.NotificationSystemBroker.Contract;
using Lykke.Service.NotificationSystemBroker.Contract.Enums;
using Lykke.Service.NotificationSystemBroker.Domain.Services;
using Lykke.Service.NotificationSystemBroker.PushProviderClient;
using Lykke.Service.NotificationSystemBroker.PushProviderClient.Models.Requests;
using Lykke.Service.NotificationSystemBroker.PushProviderClient.Models.Responses;
using Newtonsoft.Json;

namespace Lykke.Service.NotificationSystemBroker.DomainServices.Services
{
    public class PushNotificationService : IPushNotificationService
    {
        private readonly ILog _log;
        private readonly IRabbitPublisher<UpdateAuditMessageEvent> _auditMessagePublisher;
        private readonly IPushProviderClient _pushProviderClient;

        public PushNotificationService(
            IRabbitPublisher<UpdateAuditMessageEvent> auditMessagePublisher,
            IPushProviderClient pushProviderClient,
            ILogFactory logFactory)
        {
            _auditMessagePublisher = auditMessagePublisher;
            _pushProviderClient = pushProviderClient;
            _log = logFactory.CreateLog(this);
        }

        public async Task ProcessPushNotificationAsync(
            Guid messageId,
            string pushRegistrationId,
            string message,
            string customPayload,
            string messageGroupId,
            Dictionary<string, string> messageParameters)
        {
            var customPayloadDict = !string.IsNullOrWhiteSpace(customPayload)
                ? JsonConvert.DeserializeObject<Dictionary<string, string>>(customPayload)
                : new Dictionary<string, string>();

            if (!string.IsNullOrWhiteSpace(messageGroupId))
                customPayloadDict.TryAdd("MessageGroupId", messageGroupId);

            if (messageParameters != null && messageParameters.Count > 0)
            {
                foreach (var messageParameter in messageParameters)
                {
                    if (customPayloadDict.ContainsKey(messageParameter.Key))
                        _log.Warning(
                            $"Message parameter with key {messageParameter.Key} already exists in CustomPayload",
                            messageParameter.Key);
                    else
                        customPayloadDict.Add(messageParameter.Key, messageParameter.Value);
                }
            }

            var request = new SendPushNotificationRequest
            {
                MessageId = messageId.ToString(),
                CustomPayload = customPayloadDict,
                Message = message,
                PushRegistrationId = pushRegistrationId
            };

            var response = await _pushProviderClient.Api.SendPushNotificationAsync(request);

            var auditMessage = new UpdateAuditMessageEvent
            {
                MessageId = messageId.ToString(),
                SentTimestamp = DateTime.UtcNow
            };

            if (response.Result != ResultCode.Ok)
            {
                var errorMessage = $"Could not send PUSH for registration {pushRegistrationId}: {response.ErrorMessage}";
                _log.Error(message: errorMessage, context: new { messageId });
                auditMessage.DeliveryStatus = DeliveryStatus.Error;
                auditMessage.DeliveryComment = errorMessage;
            }
            else
            {
                auditMessage.DeliveryStatus = DeliveryStatus.Ok;
            }

            await _auditMessagePublisher.PublishAsync(auditMessage);
        }
    }
}
