using System;
using System.Collections.Generic;

namespace Lykke.Service.NotificationSystemBroker.Settings.Sms
{
    public class SmsSenderSettings 
    {
        public TimeSpan Timeout { get; set; }
        public int RetriesCount { get; set; }
        public List<SmsProviderRuleSettings> SmsProvidersRules { get; set; }
        public List<SmsProviderSettings> SmsProviders { get; set; }
    }
}
