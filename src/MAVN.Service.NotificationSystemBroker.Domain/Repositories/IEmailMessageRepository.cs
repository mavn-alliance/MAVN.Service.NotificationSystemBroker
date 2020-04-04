using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MAVN.Service.NotificationSystemBroker.Domain.Entities;

namespace MAVN.Service.NotificationSystemBroker.Domain.Repositories
{
    public interface IEmailMessageRepository
    {
        Task<Guid> CreateAsync(IEmailMessage emailMessage);

        Task<IEnumerable<IEmailMessage>> GetEmailMessagesForEmailAsync(string email);

        Task<IEnumerable<IEmailMessage>> GetLastFiftyTodayEmailsAsync();

        Task<IEmailMessage> GetMessageByMessageIdAsync(Guid messageId);
    }
}
