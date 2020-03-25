using System;
using AutoMapper;
using JetBrains.Annotations;
using Lykke.Sdk;
using Lykke.Service.NotificationSystemBroker.Profiles;
using Lykke.Service.NotificationSystemBroker.Settings;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Lykke.Service.NotificationSystemBroker
{
    [UsedImplicitly]
    public class Startup
    {
        private readonly LykkeSwaggerOptions _swaggerOptions = new LykkeSwaggerOptions
        {
            ApiTitle = "NotificationSystemBroker API",
            ApiVersion = "v1"
        };

        [UsedImplicitly]
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            return services.BuildServiceProvider<AppSettings>(options =>
            {
                options.Extend = (serviceCollection, settings) =>
                {
                    serviceCollection.AddAutoMapper(
                        typeof(AutoMapperProfile),
                        typeof(MsSqlRepositories.AutoMapperProfile));
                };

                options.SwaggerOptions = _swaggerOptions;

                options.Logs = logs =>
                {
                    logs.AzureTableName = "NotificationSystemBrokerLog";
                    logs.AzureTableConnectionStringResolver = settings => settings.NotificationSystemBrokerService.Db.LogsConnString;
                };

                options.Swagger = swagger =>
                {
                    swagger.IgnoreObsoleteActions();
                };
            });
        }

        [UsedImplicitly]
        public void Configure(IApplicationBuilder app, IMapper mapper)
        {
            app.UseMvc();

            app.UseLykkeConfiguration(options =>
            {
                options.SwaggerOptions = _swaggerOptions;
            });

            mapper.ConfigurationProvider.AssertConfigurationIsValid();
        }
    }
}
