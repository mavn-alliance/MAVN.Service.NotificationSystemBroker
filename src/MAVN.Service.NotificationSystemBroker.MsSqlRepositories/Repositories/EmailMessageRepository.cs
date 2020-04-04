using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Lykke.Common.MsSql;
using MAVN.Service.NotificationSystemBroker.Domain.Entities;
using MAVN.Service.NotificationSystemBroker.Domain.Repositories;
using MAVN.Service.NotificationSystemBroker.MsSqlRepositories.Entities;
using Microsoft.EntityFrameworkCore;

namespace MAVN.Service.NotificationSystemBroker.MsSqlRepositories.Repositories
{
    public class EmailMessageRepository : IEmailMessageRepository
    {
        private readonly MsSqlContextFactory<NotificationSystemBrokerContext> _msSqlContextFactory;
        private readonly IMapper _mapper;

        public EmailMessageRepository(MsSqlContextFactory<NotificationSystemBrokerContext> msSqlContextFactory,
            IMapper mapper)
        {
            _msSqlContextFactory = msSqlContextFactory;
            _mapper = mapper;
        }

        public async Task<Guid> CreateAsync(IEmailMessage emailMessage)
        {
            using (var context = _msSqlContextFactory.CreateDataContext())
            {
                var entity = _mapper.Map<EmailMessageEntity>(emailMessage);

                entity.Timestamp = DateTime.UtcNow;

                context.Add((object)entity);

                await context.SaveChangesAsync();

                return entity.Id;
            }
        }

        public async Task<IEnumerable<IEmailMessage>> GetEmailMessagesForEmailAsync(string email)
        {
            using (var context = _msSqlContextFactory.CreateDataContext())
            {
                var list = await context.EmailMessages
                    .Where(e => e.Email == email)
                    .OrderByDescending(e => e.Timestamp)
                    .ToListAsync();

                return list.Select(e => _mapper.Map<IEmailMessage>(e));
            }
        }

        public async Task<IEnumerable<IEmailMessage>> GetLastFiftyTodayEmailsAsync()
        {
            using (var context = _msSqlContextFactory.CreateDataContext())
            {
                var today = DateTime.UtcNow.Date;
                var list = await context.EmailMessages
                    .Where(i => i.Timestamp >= today)
                    .OrderByDescending(i => i.Timestamp)
                    .Take(50)
                    .ToListAsync();

                return list.Select(e => _mapper.Map<IEmailMessage>(e));
            }
        }

        public async Task<IEmailMessage> GetMessageByMessageIdAsync(Guid messageId)
        {
            using (var context = _msSqlContextFactory.CreateDataContext())
            {
                var entity = await context.EmailMessages
                    .FirstOrDefaultAsync(e => e.MessageId == messageId);

                return entity;
            }
        }
    }
}
