using AutoMapper;
using JetBrains.Annotations;
using Lykke.Service.NotificationSystemBroker.Client.Models;
using Lykke.Service.NotificationSystemBroker.Domain.Entities;

namespace Lykke.Service.NotificationSystemBroker.Profiles
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
