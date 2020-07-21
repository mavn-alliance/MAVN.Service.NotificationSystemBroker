using Autofac;
using MAVN.Service.NotificationSystemBroker.Domain.Services;
using MAVN.Service.NotificationSystemBroker.DomainServices.Services;
using MailKit.Net.Smtp;

namespace MAVN.Service.NotificationSystemBroker.DomainServices
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
