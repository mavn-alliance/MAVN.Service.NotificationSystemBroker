using System.Threading.Tasks;
using JetBrains.Annotations;
using MAVN.Service.NotificationSystemBroker.PushProviderClient.Models.Requests;
using MAVN.Service.NotificationSystemBroker.PushProviderClient.Models.Responses;
using Refit;

namespace MAVN.Service.NotificationSystemBroker.PushProviderClient
{
    /// <summary>
    /// PushProvider client API interface.
    /// </summary>
    [PublicAPI]
    public interface IPushProviderApi
    {
        /// <summary>
        /// Send push notification
        /// </summary>
        /// <param name="model">Data to use when sending push notification</param>
        /// <returns>Response on push notification</returns>
        [Post("/api/pushNotification")]
        Task<SendPushNotificationResponse> SendPushNotificationAsync(SendPushNotificationRequest model);
    }
}
