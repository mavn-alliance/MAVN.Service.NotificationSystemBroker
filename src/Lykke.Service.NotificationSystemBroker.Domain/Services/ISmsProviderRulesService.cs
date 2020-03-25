using Lykke.Service.NotificationSystemBroker.Domain.Entities;

namespace Lykke.Service.NotificationSystemBroker.Domain.Services
{
    public interface ISmsProviderRulesService
    {
        ServiceProviderRule GetSmsProviderRule(string phoneNumber);
    }
}
