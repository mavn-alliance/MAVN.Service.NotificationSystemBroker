using System;
using System.Threading.Tasks;

namespace Lykke.Service.NotificationSystemBroker.Domain.Services
{
    public interface ISmsService
    {
        /// <summary>
        /// Process SMS message (send SMS via proper SMS provider)
        /// </summary>
        /// <param name="phoneNumber">Phone number to send SMS to</param>
        /// <param name="message">SMS message text that we want to send</param>
        /// <param name="messageId">Message id</param>
        /// <returns>SMS sender result with sending status and error message (in case error happened during sending)</returns>
        Task ProcessSmsMessageAsync(string phoneNumber, string message, Guid messageId);
    }
}
