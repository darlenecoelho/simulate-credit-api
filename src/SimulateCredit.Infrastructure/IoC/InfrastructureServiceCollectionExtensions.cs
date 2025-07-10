using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SimulateCredit.Application.Ports.Outgoing;
using SimulateCredit.Infrastructure.Adapters.Broker;
using SimulateCredit.Infrastructure.Adapters.EmailNotification;
using SimulateCredit.Infrastructure.Adapters.FakeCurrency;
using SimulateCredit.Infrastructure.Adapters.Persistence.Mongo;
using SimulateCredit.Infrastructure.Logging;
using SimulateCredit.Infrastructure.Mapping;
using SimulateCredit.Infrastructure.Settings;

namespace SimulateCredit.Infrastructure.IoC
{
    public static class InfrastructureServiceCollectionExtensions
    {
        public static IServiceCollection AddInfrastructureServices(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            // Settings
            services.Configure<MongoDbSettings>(configuration.GetSection("MongoDb"));
            services.Configure<RabbitMqSettings>(configuration.GetSection("RabbitMq"));

            services.AddAutoMapper(typeof(MappingProfile).Assembly);

            // Persistence
            services.AddSingleton<ISimulationRepository, MongoSimulationRepository>();

            // Messaging
            services.AddSingleton<ICreditSimulationPublisher, RabbitMqPublisherAdapter>();

            // Currency converter 
            services.AddScoped<ICurrencyConverterService, FakeCurrencyConverterService>();

            // Email notifications
            services.AddSingleton<INotificationService, NotificationService>();

            // Audit logging
            services.AddSingleton<IAuditLogger, AuditLogger>();

            return services;
        }
    }
}