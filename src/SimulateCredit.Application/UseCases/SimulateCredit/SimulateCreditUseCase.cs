using MediatR;
using SimulateCredit.Application.DTOs;
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

    public SimulateCreditUseCase(
        ISimulationRepository simulationRepository,
        IMediator mediator,
        ILoanCalculationService calculator,
        ISimulationFactory factory)
    {
        _simulationRepository = simulationRepository;
        _mediator = mediator;
        _calculator = calculator;
        _factory = factory;
    }

    public async Task<SimulateCreditResponse> ExecuteAsync(SimulateCreditRequest request)
    {
        #region Calculate
        var resultDto = await _calculator.CalculateAsync(request);
        #endregion

        #region Create domain entity
        var simulation = _factory.Create(request, resultDto);
        #endregion

        #region persistence
        await _simulationRepository.SaveAsync(simulation);
        #endregion

        #region Publish events
        await _mediator.Publish(new SimulationCompletedNotification(resultDto));
        #endregion

        return new SimulateCreditResponse
        {
            TotalAmount = resultDto.TotalAmount,
            MonthlyPayment = resultDto.MonthlyPayment,
            TotalInterest = resultDto.TotalInterest
        };
    }
}