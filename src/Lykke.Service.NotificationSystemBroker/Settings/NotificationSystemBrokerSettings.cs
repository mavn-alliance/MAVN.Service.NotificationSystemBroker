using JetBrains.Annotations;

namespace Lykke.Service.NotificationSystemBroker.Settings
{
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public class NotificationSystemBrokerSettings
    {
        public DbSettings Db { get; set; }
    }
}
