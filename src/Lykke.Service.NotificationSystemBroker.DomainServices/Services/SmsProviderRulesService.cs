using System;
using System.Collections.Generic;
using System.Linq;
using Lykke.Service.NotificationSystemBroker.Domain.Entities;
using Lykke.Service.NotificationSystemBroker.Domain.Services;

namespace Lykke.Service.NotificationSystemBroker.DomainServices.Services
{
    public class SmsProviderRulesService : ISmsProviderRulesService
    {
        private readonly SortedDictionary<string, ServiceProviderRule> _rulesDict;

        public SmsProviderRulesService(IEnumerable<ServiceProvider> smsProviders, 
            IEnumerable<SmsProviderRuleOrigin> smsProviderRulesOrigin)
        {
            var providersDict = smsProviders.ToDictionary(k => k.Name, k => k);

            _rulesDict = new SortedDictionary<string, ServiceProviderRule>(
                smsProviderRulesOrigin.Select(origin => new ServiceProviderRule
                    {
                        Code = origin.Code,
                        SmsProviders = origin.Names.Select(name => providersDict[name]).ToList()
                    })
                    .ToDictionary(k => k.Code),
                // Order by code length descending
                Comparer<string>.Create((a, b) =>
                {
                    var lengthComparison = a.Length.CompareTo(b.Length);

                    // In case that string are same length, we will compare (and sort) by string
                    return lengthComparison == 0 ? string.Compare(a, b, StringComparison.OrdinalIgnoreCase) : lengthComparison;
                }));
        }

        public ServiceProviderRule GetSmsProviderRule(string phoneNumber)
        {
            // Determine which provider(s) to use based on phone number
            _rulesDict.TryGetValue(phoneNumber, out var rule);

            if (rule == null)
            {
                rule = _rulesDict.Values
                    .FirstOrDefault(x =>
                        phoneNumber.Length >= x.Code.Length &&
                        phoneNumber.StartsWith(x.Code, StringComparison.InvariantCultureIgnoreCase));
            }

            // Handle *
            if (rule == null)
                _rulesDict.TryGetValue("*", out rule);

            return rule;
        }
    }
}
