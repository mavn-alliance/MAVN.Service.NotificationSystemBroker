using Autofac;
using Lykke.Service.NotificationSystemBroker.Domain.Services;
using Lykke.Service.NotificationSystemBroker.DomainServices.Services;
using MailKit.Net.Smtp;

namespace Lykke.Service.NotificationSystemBroker.DomainServices
{
    public class AutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterMailClient(() => new SmtpClient());

            builder.RegisterType<EmailMessagesService>()
                .As<IEmailMessagesService>()
                .SingleInstance();
        }
    }
}
