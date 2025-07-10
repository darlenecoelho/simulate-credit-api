using MediatR;
using Microsoft.Extensions.Logging;
using SimulateCredit.Application.DTOs;
using SimulateCredit.Application.Exceptions;
using SimulateCredit.Application.Interfaces;
using SimulateCredit.Application.Notifications;
using SimulateCredit.Application.Ports.Incoming;
using SimulateCredit.Application.Ports.Outgoing;

namespace SimulateCredit.Application.UseCases.SimulateCredit;

public sealed class SimulateCreditUseCase : ISimulateCreditUseCase
{
    private readonly ISimulationRepository _simulationRepository;
    private readonly IMediator _mediator;
    private readonly ILoanCalculationService _calculator;
    private readonly ISimulationFactory _factory;
    private readonly ILogger<SimulateCreditUseCase> _logger;

    public SimulateCreditUseCase(
        ISimulationRepository simulationRepository,
        IMediator mediator,
        ILoanCalculationService calculator,
        ISimulationFactory factory,
        ILogger<SimulateCreditUseCase> logger)
    {
        _simulationRepository = simulationRepository;
        _mediator = mediator;
        _calculator = calculator;
        _factory = factory;
        _logger = logger;
    }

    public async Task<SimulateCreditResponse> ExecuteAsync(SimulateCreditRequest request)
    {
        _logger.LogInformation("Starting simulation for {Email}", request.Customer.Email);
        try
        {
            #region Calculate
            _logger.LogInformation("Calculating loan values");
            var resultDto = await _calculator.CalculateAsync(request);
            #endregion

            #region Create domain entity
            var simulation = _factory.Create(request, resultDto);
            _logger.LogInformation("Simulation entity created");
            #endregion

            #region persistence
            await _simulationRepository.SaveAsync(simulation);
            _logger.LogInformation("Simulation persisted");
            #endregion

            #region Publish events
            await _mediator.Publish(new SimulationCompletedNotification(resultDto));
            _logger.LogInformation("Simulation events published");
            #endregion

            return new SimulateCreditResponse
            {
                TotalAmount = resultDto.TotalAmount,
                MonthlyPayment = resultDto.MonthlyPayment,
                TotalInterest = resultDto.TotalInterest
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error executing simulation for {Email}", request.Customer.Email);
            throw new SimulationException("An error occurred while executing the simulation", ex);
        }
    }
}