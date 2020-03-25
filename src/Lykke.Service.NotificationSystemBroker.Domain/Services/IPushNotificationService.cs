using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lykke.Service.NotificationSystemBroker.Domain.Services
{
    public interface IPushNotificationService
    {
        Task ProcessPushNotificationAsync(
            Guid messageId,
            string pushRegistrationId,
            string message,
            string customPayload,
            string messageGroupId,
            Dictionary<string, string> messageParameters);
    }
}
