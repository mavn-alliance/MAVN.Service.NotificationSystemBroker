using MAVN.Service.NotificationSystemBroker.Domain.Entities;

namespace MAVN.Service.NotificationSystemBroker.Domain.Services
{
    public interface ISmsProviderRulesService
    {
        ServiceProviderRule GetSmsProviderRule(string phoneNumber);
    }
}
