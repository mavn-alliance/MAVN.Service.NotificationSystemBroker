using System.Collections.Generic;
using System.Threading.Tasks;
using Lykke.Service.NotificationSystemBroker.Domain.Entities;

namespace Lykke.Service.NotificationSystemBroker.Domain.Services
{
    public interface IEmailMessagesService
    {
        Task<IEnumerable<IEmailMessage>> RetrieveEmailMessages(string email);
    }
}
