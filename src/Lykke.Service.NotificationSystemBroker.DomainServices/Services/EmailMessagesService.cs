using System.Collections.Generic;
using System.Threading.Tasks;
using Lykke.Service.NotificationSystemBroker.Domain.Entities;
using Lykke.Service.NotificationSystemBroker.Domain.Repositories;
using Lykke.Service.NotificationSystemBroker.Domain.Services;

namespace Lykke.Service.NotificationSystemBroker.DomainServices.Services
{
    public class EmailMessagesService : IEmailMessagesService
    {
        private readonly IEmailMessageRepository _emailMessageRepository;

        public EmailMessagesService(IEmailMessageRepository emailMessageRepository)
        {
            _emailMessageRepository = emailMessageRepository;
        }

        public async Task<IEnumerable<IEmailMessage>> RetrieveEmailMessages(string email)
        {
            return await _emailMessageRepository.GetEmailMessagesForEmailAsync(email);
        }
    }
}
