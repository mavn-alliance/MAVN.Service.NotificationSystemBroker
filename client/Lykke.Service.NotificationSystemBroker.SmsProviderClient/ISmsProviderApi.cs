using System.Threading.Tasks;
using JetBrains.Annotations;
using Refit;

namespace Lykke.Service.NotificationSystemBroker.SmsProviderClient
{
    /// <summary>
    /// A generic contract interface. To be used in each SmsProvider 
    /// </summary>
    [PublicAPI]
    public interface ISmsProviderApi
    {
        /// <summary>
        /// A method used to send sms
        /// </summary>
        /// <param name="model">SMS message request model</param>
        /// <returns>Result with error details in case something happened</returns>
        [Post("/api/sms")]
        Task<SmsSenderResult> SendSmsAsync(SendSmsRequestModel model);
    }
}
