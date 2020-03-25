using JetBrains.Annotations;

namespace Lykke.Service.NotificationSystemBroker.Client
{
    /// <summary>
    /// NotificationSystemBroker client interface.
    /// </summary>
    [PublicAPI]
    public interface INotificationSystemBrokerClient
    {
        /// <summary>Application Api interface</summary>
        IEmailMessagesApi Api { get; }
    }
}
