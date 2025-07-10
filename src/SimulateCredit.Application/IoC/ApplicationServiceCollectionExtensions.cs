using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using SimulateCredit.Application.Behaviors;
using SimulateCredit.Application.Factories;
using SimulateCredit.Application.Interfaces;
using SimulateCredit.Application.Ports.Incoming;
using SimulateCredit.Application.Services;
using SimulateCredit.Application.UseCases.SimulateCredit;
using SimulateCredit.Application.Validators;

namespace SimulateCredit.Application.IoC
{
    public static class ApplicationServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationServices(
            this IServiceCollection services)
        {

            services.AddValidatorsFromAssemblyContaining<SimulateCreditRequestValidator>();

            services.AddTransient(
             typeof(IPipelineBehavior<,>),
             typeof(ValidationBehavior<,>));

            // Business services
            services.AddScoped<ILoanCalculationService, LoanCalculationService>();

            // Factories
            services.AddSingleton<ISimulationFactory, SimulationFactoryAdapter>();

            // Use cases
            services.AddScoped<ISimulateCreditUseCase, SimulateCreditUseCase>();

            // MediatR for notifications/commands
            services.AddMediatR(cfg =>
                cfg.RegisterServicesFromAssemblies(
                    typeof(SimulateCreditUseCase).Assembly));

            return services;
        }
    }
}
