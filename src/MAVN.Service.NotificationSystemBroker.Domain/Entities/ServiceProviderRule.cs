using System.Collections.Generic;

namespace MAVN.Service.NotificationSystemBroker.Domain.Entities
{
    public class ServiceProviderRule
    {
        public string Code { get; set; }

        public List<ServiceProvider> SmsProviders { get; set; }
    }
}
