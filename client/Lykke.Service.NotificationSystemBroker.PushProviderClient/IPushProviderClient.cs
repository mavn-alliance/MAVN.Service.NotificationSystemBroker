using JetBrains.Annotations;

namespace Lykke.Service.NotificationSystemBroker.PushProviderClient
{
    /// <summary>
    /// PushProvider client interface.
    /// </summary>
    [PublicAPI]
    public interface IPushProviderClient
    {
        /// <summary>Application Api interface</summary>
        IPushProviderApi Api { get; }
    }
}
