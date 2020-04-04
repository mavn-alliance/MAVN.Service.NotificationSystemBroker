using JetBrains.Annotations;

namespace MAVN.Service.NotificationSystemBroker.Settings
{
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public class NotificationSystemBrokerSettings
    {
        public DbSettings Db { get; set; }
    }
}
