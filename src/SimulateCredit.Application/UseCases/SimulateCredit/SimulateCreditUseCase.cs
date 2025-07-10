using MediatR;
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
    private readonly IAuditLogger _auditLogger;

    public SimulateCreditUseCase(
        ISimulationRepository simulationRepository,
        IMediator mediator,
        ILoanCalculationService calculator,
        ISimulationFactory factory,
        IAuditLogger auditLogger)
    {
        _simulationRepository = simulationRepository;
        _mediator = mediator;
        _calculator = calculator;
        _factory = factory;
        _auditLogger = auditLogger;
    }

    public async Task<SimulateCreditResponse> ExecuteAsync(SimulateCreditRequest request)
    {
        var email = request.Customer.Email;

        _auditLogger.LogInformation(
            "Starting simulation for {Email} | Loan={Amount} {Currency}, Months={Months}, RateType={RateType}",
            email,
            request.LoanAmount.Amount,
            request.LoanAmount.Currency,
            request.Months,
            request.RateType);

        try
        {
            #region Calculate
            _auditLogger.LogInformation(
                "Simulation for {Email}: Calculating loan values",
                email);

            var resultDto = await _calculator.CalculateAsync(request);

            _auditLogger.LogInformation(
                "Simulation for {Email}: Calculation completed | Total={Total}, Monthly={Monthly}, Interest={Interest}",
                email,
                resultDto.TotalAmount,
                resultDto.MonthlyPayment,
                resultDto.TotalInterest);
            #endregion

            #region Create domain entity
            var simulation = _factory.Create(request, resultDto);
            _auditLogger.LogInformation(
                "Simulation for {Email}: Domain entity created at {CreatedAt}",
                email,
                simulation.CreatedAt);
            #endregion

            #region Persistence
            await _simulationRepository.SaveAsync(simulation);
            _auditLogger.LogInformation(
                "Simulation for {Email}: Persisted successfully",
                email);
            #endregion

            #region Publish Events
            await _mediator.Publish(new SimulationCompletedNotification(resultDto));
            _auditLogger.LogInformation(
                "Simulation for {Email}: Notification published",
                email);
            #endregion

            _auditLogger.LogInformation(
                "Simulation for {Email}: Completed successfully",
                email);

            return new SimulateCreditResponse
            {
                TotalAmount = resultDto.TotalAmount,
                MonthlyPayment = resultDto.MonthlyPayment,
                TotalInterest = resultDto.TotalInterest
            };
        }
        catch (Exception ex)
        {
            _auditLogger.LogError(
                ex,
                "Simulation for {Email} failed",
                email);

            throw new SimulationException(
                $"Error executing simulation for {email}", ex);
        }
    }
}