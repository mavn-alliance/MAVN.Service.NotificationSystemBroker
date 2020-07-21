using System;
using Autofac;
using AutoMapper;
using JetBrains.Annotations;
using Lykke.Sdk;
using Lykke.SettingsReader;
using MAVN.Service.NotificationSystemBroker.Profiles;
using MAVN.Service.NotificationSystemBroker.Settings;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MAVN.Service.NotificationSystemBroker
{
    [UsedImplicitly]
    public class Startup
    {
        private IConfigurationRoot _configurationRoot;
        private IReloadingManager<AppSettings> _settingsManager;

        private readonly LykkeSwaggerOptions _swaggerOptions = new LykkeSwaggerOptions
        {
            ApiTitle = "NotificationSystemBroker API",
            ApiVersion = "v1"
        };

        [UsedImplicitly]
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().AddNewtonsoftJson();

            (_configurationRoot, _settingsManager) = services.BuildServiceProvider<AppSettings>(options =>
            {
                options.SwaggerOptions = _swaggerOptions;

                options.Logs = logs =>
                {
                    logs.AzureTableName = "NotificationSystemBrokerLog";
                    logs.AzureTableConnectionStringResolver = settings => settings.NotificationSystemBrokerService.Db.LogsConnString;
                };

                options.Extend = (serviceCollection, settings) =>
                {
                    serviceCollection.AddAutoMapper(
                        typeof(AutoMapperProfile),
                        typeof(MsSqlRepositories.AutoMapperProfile));
                };

                options.Swagger = swagger =>
                {
                    swagger.IgnoreObsoleteActions();
                };
            });
        }

        [UsedImplicitly]
        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.ConfigureLykkeContainer(
                _configurationRoot,
                _settingsManager);
        }

        [UsedImplicitly]
        public void Configure(
            IApplicationBuilder app,
            IMapper mapper,
            IApplicationLifetime appLifetime)
        {
            mapper.ConfigurationProvider.AssertConfigurationIsValid();
            app.UseRouting().UseEndpoints(endpoints => { endpoints.MapControllers(); });
            app.UseLykkeConfiguration(appLifetime, options =>
            {
                options.SwaggerOptions = _swaggerOptions;
            });
        }
    }
}
