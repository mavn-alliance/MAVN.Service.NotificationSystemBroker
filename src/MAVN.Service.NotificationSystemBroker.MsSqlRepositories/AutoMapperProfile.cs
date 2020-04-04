using AutoMapper;
using MAVN.Service.NotificationSystemBroker.Domain.Entities;
using MAVN.Service.NotificationSystemBroker.MsSqlRepositories.Entities;

namespace MAVN.Service.NotificationSystemBroker.MsSqlRepositories
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
