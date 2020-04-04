using System.Collections.Generic;
using System.Linq;
using Autofac;
using AutoMapper;
using JetBrains.Annotations;
using Lykke.Sdk;
using MAVN.Service.NotificationSystemBroker.Domain.Entities;
using MAVN.Service.NotificationSystemBroker.Domain.Services;
using MAVN.Service.NotificationSystemBroker.DomainServices.Services;
using MAVN.Service.NotificationSystemBroker.PushProviderClient;
using MAVN.Service.NotificationSystemBroker.Services;
using MAVN.Service.NotificationSystemBroker.Settings;
using MAVN.Service.NotificationSystemBroker.SmsProviderClient;
using Lykke.SettingsReader;

namespace MAVN.Service.NotificationSystemBroker.Modules
{
    [UsedImplicitly]
    public class ServiceModule : Module
    {
        private readonly AppSettings _appSettings;

        public ServiceModule(IReloadingManager<AppSettings> appSettings)
        {
            _appSettings = appSettings.CurrentValue;
        }

        protected override void Load(ContainerBuilder builder)
        {
            // Do not register entire settings in container, pass necessary settings to services which requires them

            builder.RegisterModule(new MsSqlRepositories.AutofacModule(
                _appSettings.NotificationSystemBrokerService.Db.DataConnString));
            builder.RegisterModule(new DomainServices.AutofacModule());

            builder.RegisterType<Mapper>().As<IMapper>().SingleInstance();

            builder.RegisterType<StartupManager>()
                .As<IStartupManager>()
                .SingleInstance();

            builder.RegisterType<ShutdownManager>()
                .As<IShutdownManager>()
                .AutoActivate()
                .SingleInstance();

            RegisterSmtp(builder);
            RegisterSms(builder);
            RegisterPushNotifications(builder);
        }

        private void RegisterSmtp(ContainerBuilder builder)
        {
            builder.RegisterType<SmtpService>()
                .As<ISmtpService>()
                .InstancePerDependency()
                .WithParameter("host", _appSettings.EmailSender.Smtp.Host)
                .WithParameter("port", _appSettings.EmailSender.Smtp.Port)
                .WithParameter("username", _appSettings.EmailSender.Smtp.Username)
                .WithParameter("password", _appSettings.EmailSender.Smtp.Password)
                .WithParameter("emailSender", _appSettings.EmailSender.Smtp.Sender)
                .WithParameter("emailSenderName", _appSettings.EmailSender.Smtp.SenderName)
                .WithParameter("useSsl", _appSettings.EmailSender.Smtp.UseSsl)
                .WithParameter("emailSenderWorkMode", _appSettings.EmailSender.WorkMode);
        }

        private void RegisterSms(ContainerBuilder builder)
        {
            builder.RegisterType<SmsProviderClientFactory>()
                .As<ISmsProviderClientFactory>()
                .SingleInstance()
                .WithParameter(TypedParameter.From(_appSettings.SmsSender.RetriesCount))
                .WithParameter(TypedParameter.From(_appSettings.SmsSender.Timeout));

            builder.RegisterType<SmsProviderRulesService>()
                .As<ISmsProviderRulesService>()
                .SingleInstance()
                .WithParameter(TypedParameter.From(
                    _appSettings.SmsSender.SmsProviders.Select(s =>
                        new ServiceProvider {Name = s.Name, ServiceUrl = s.ServiceUrl})))
                .WithParameter(TypedParameter.From(
                    _appSettings.SmsSender.SmsProvidersRules.Select(s =>
                        new SmsProviderRuleOrigin {Code = s.Code, Names = new List<string>(s.Names)})));

            builder.RegisterType<SmsService>()
                .As<ISmsService>()
                .SingleInstance();
        }

        private void RegisterPushNotifications(ContainerBuilder builder)
        {
            var clientFactory = new PushProviderClientFactory(_appSettings.PushSender.Timeout, _appSettings.PushSender.RetriesCount);
            var client = clientFactory.CreateClient(_appSettings.PushSender.ServiceClient);

            builder.RegisterInstance(client)
                .As<IPushProviderClient>()
                .SingleInstance();

            builder.RegisterType<PushNotificationService>()
                .As<IPushNotificationService>()
                .SingleInstance();
        }
    }
}
