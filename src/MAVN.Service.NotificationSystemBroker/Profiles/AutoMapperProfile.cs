using AutoMapper;
using JetBrains.Annotations;
using MAVN.Service.NotificationSystemBroker.Client.Models;
using MAVN.Service.NotificationSystemBroker.Domain.Entities;

namespace MAVN.Service.NotificationSystemBroker.Profiles
{
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<IEmailMessage, EmailMessageResponseModel>();
        }
    }
}
