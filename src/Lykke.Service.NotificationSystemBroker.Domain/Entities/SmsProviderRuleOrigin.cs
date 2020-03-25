using System.Collections.Generic;

namespace Lykke.Service.NotificationSystemBroker.Domain.Entities
{
    public class SmsProviderRuleOrigin
    {
        public string Code { get; set; }
        public List<string> Names { get; set; }
    }
}
