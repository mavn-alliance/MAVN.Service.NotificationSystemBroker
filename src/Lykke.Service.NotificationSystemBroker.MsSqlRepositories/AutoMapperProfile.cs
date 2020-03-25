using AutoMapper;
using Lykke.Service.NotificationSystemBroker.Domain.Entities;
using Lykke.Service.NotificationSystemBroker.MsSqlRepositories.Entities;

namespace Lykke.Service.NotificationSystemBroker.MsSqlRepositories
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<EmailMessageEntity, IEmailMessage>();

            CreateMap<IEmailMessage, EmailMessageEntity>()
                .ForMember(src => src.Id, opt => opt.Ignore());
        }
    }
}
