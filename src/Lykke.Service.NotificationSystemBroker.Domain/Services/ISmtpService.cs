using System;
using System.Threading.Tasks;

namespace Lykke.Service.NotificationSystemBroker.Domain.Services
{
    public interface ISmtpService
    {
        Task ProcessMessageAsync(string email, string subject, string body, Guid messageId);
    }
}
