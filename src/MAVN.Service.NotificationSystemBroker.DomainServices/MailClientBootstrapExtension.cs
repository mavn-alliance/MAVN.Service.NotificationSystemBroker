using System;
using Autofac;
using Autofac.Builder;
using MailKit;

namespace MAVN.Service.NotificationSystemBroker.DomainServices
{
    static class MailClientBootstrapExtension
    {
        public static void RegisterMailClient(this ContainerBuilder builder, Func<IMailTransport> mailClientCreator)
        {
            Autofac.RegistrationExtensions.AsSelf<MailClientFactory, SimpleActivatorData>(
                Autofac.RegistrationExtensions.RegisterInstance<MailClientFactory>(builder, new MailClientFactory(mailClientCreator)))
                .SingleInstance();
        }
    }
}
