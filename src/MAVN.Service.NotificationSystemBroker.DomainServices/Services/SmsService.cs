using System;
using System.Threading.Tasks;
using Common.Log;
using Lykke.Common.Log;
using Lykke.RabbitMqBroker.Publisher;
using MAVN.Service.NotificationSystemBroker.Contract;
using MAVN.Service.NotificationSystemBroker.Contract.Enums;
using MAVN.Service.NotificationSystemBroker.Domain.Services;
using MAVN.Service.NotificationSystemBroker.SmsProviderClient;
using MAVN.Service.NotificationSystemBroker.SmsProviderClient.Enums;

namespace MAVN.Service.NotificationSystemBroker.DomainServices.Services
{
    public class SmsService : ISmsService
    {
        private readonly ISmsProviderRulesService _smsProviderRulesService;
        private readonly ISmsProviderClientFactory _smsProviderClientFactory;
        private readonly IRabbitPublisher<UpdateAuditMessageEvent> _auditMessagePublisher;
        private readonly ILog _log;

        public SmsService(
            ISmsProviderRulesService smsProviderRulesService,
            ISmsProviderClientFactory smsProviderClientFactory,
            ILogFactory logFactory,
            IRabbitPublisher<UpdateAuditMessageEvent> auditMessagePublisher)
        {
            _smsProviderRulesService = smsProviderRulesService;
            _smsProviderClientFactory = smsProviderClientFactory;
            _auditMessagePublisher = auditMessagePublisher;
            _log = logFactory.CreateLog(this);
        }

        public async Task ProcessSmsMessageAsync(string phoneNumber, string message, Guid messageId)
        {
            if (string.IsNullOrEmpty(phoneNumber))
            {
                _log.Warning("Phone number must not be empty", context: new {messageId});
                return;
            }

            // Get providers matching this phone number
            var rule = _smsProviderRulesService.GetSmsProviderRule(phoneNumber);
            var providers = rule?.SmsProviders;

            var auditMessage = new UpdateAuditMessageEvent {MessageId = messageId.ToString()};

            if (providers == null)
            {
                var errorMessage = "Could not find any sms providers to match with phone number";
                _log.Error(message: errorMessage, context: new {messageId});

                auditMessage.DeliveryStatus = DeliveryStatus.Error;
                auditMessage.DeliveryComment = errorMessage;
                auditMessage.SentTimestamp = DateTime.UtcNow;
                await _auditMessagePublisher.PublishAsync(auditMessage);
                return;
            }

            // Try to send SMS provider by provider - one should succeed
            foreach (var provider in providers)
            {
                var client = _smsProviderClientFactory.CreateClient(provider.ServiceUrl);

                SmsSenderResult response;
                try
                {
                    response = await client.Api.SendSmsAsync(new SendSmsRequestModel
                    {
                        PhoneNumber = phoneNumber, Message = message, MessageId = messageId
                    });
                }
                catch (Exception e)
                {
                    var errorMessage = "Could not send SMS with specific provider";
                    _log.Error(e, errorMessage, new {messageId, Provider = provider.Name});

                    auditMessage.DeliveryStatus = DeliveryStatus.Error;
                    auditMessage.DeliveryComment = $"{errorMessage}{Environment.NewLine}{e.Message}";
                    auditMessage.SentTimestamp = DateTime.UtcNow;
                    await _auditMessagePublisher.PublishAsync(auditMessage);
                    continue;
                }

                if (response.Result != SmsResult.Ok)
                {
                    var errorMessage = $"Could not send SMS with provider {provider.Name}: {response.ErrorMessage}";
                    _log.Warning(errorMessage, context: new {messageId});

                    auditMessage.DeliveryStatus = DeliveryStatus.Error;
                    auditMessage.DeliveryComment = errorMessage;
                    auditMessage.SentTimestamp = DateTime.UtcNow;

                    await _auditMessagePublisher.PublishAsync(auditMessage);

                    continue;
                }

                _log.Info("SMS sent successfully", new {messageId});

                auditMessage.DeliveryStatus = DeliveryStatus.Ok;
                auditMessage.SentTimestamp = DateTime.UtcNow;
                await _auditMessagePublisher.PublishAsync(auditMessage);
                return;
            }

            var errorMsg = "Could not send SMS with all available providers.";
            _log.Error(message: errorMsg, context: new { messageId });

            auditMessage.DeliveryStatus = DeliveryStatus.Error;
            auditMessage.DeliveryComment = errorMsg;
            auditMessage.SentTimestamp = DateTime.UtcNow;

            await _auditMessagePublisher.PublishAsync(auditMessage);
        }
    }
}
