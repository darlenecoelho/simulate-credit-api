namespace SimulateCredit.Application.Ports.Outgoing;

public interface INotificationService
{
    /// <summary>
    /// Simulates sending a notification (email) with the simulation results.
    /// </summary>
    /// <param name="email">Destination address.</param>
    /// <param name="totalAmount">Total amount to be paid.</param>
    /// <param name="monthlyPayment">Monthly installment value.</param>
    /// <param name="totalInterest">Total interest paid.</param>
    Task NotifySimulationAsync(
        string email,
        decimal totalAmount,
        decimal monthlyPayment,
        decimal totalInterest);
}
