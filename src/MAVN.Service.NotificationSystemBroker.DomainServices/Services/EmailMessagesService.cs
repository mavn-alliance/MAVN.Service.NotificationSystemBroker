using System.Collections.Generic;
using System.Threading.Tasks;
using MAVN.Service.NotificationSystemBroker.Domain.Entities;
using MAVN.Service.NotificationSystemBroker.Domain.Repositories;
using MAVN.Service.NotificationSystemBroker.Domain.Services;

namespace MAVN.Service.NotificationSystemBroker.DomainServices.Services
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
