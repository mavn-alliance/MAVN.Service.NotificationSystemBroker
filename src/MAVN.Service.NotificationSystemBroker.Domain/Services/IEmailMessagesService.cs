using System.Collections.Generic;
using System.Threading.Tasks;
using MAVN.Service.NotificationSystemBroker.Domain.Entities;

namespace MAVN.Service.NotificationSystemBroker.Domain.Services
{
    public interface IEmailMessagesService
    {
        Task<IEnumerable<IEmailMessage>> RetrieveEmailMessages(string email);
    }
}
