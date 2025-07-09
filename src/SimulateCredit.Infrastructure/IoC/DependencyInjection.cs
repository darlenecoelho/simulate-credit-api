using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SimulateCredit.Application.Ports.Outgoing;
using SimulateCredit.Infrastructure.Adapters.FakeCurrency;
using SimulateCredit.Infrastructure.Adapters.Persistence.Mongo;
using SimulateCredit.Infrastructure.Mapping;
using SimulateCredit.Infrastructure.Settings;

namespace SimulateCredit.Infrastructure.IoC;

public static class DependencyInjection
{
    public static IServiceCollection AddSimulateCreditServices(this IServiceCollection services, IConfiguration configuration)
    {

        services.Configure<MongoDbSettings>(configuration.GetSection("MongoDb"));
        services.AddSingleton<ISimulationRepository, MongoSimulationRepository>();

        // Fake Currency Converter
        services.AddScoped<ICurrencyConverterService, FakeCurrencyConverterService>();

        // AutoMapper
        services.AddAutoMapper(typeof(MappingProfile).Assembly);

        return services;
    }
}