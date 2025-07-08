using Flunt.Notifications;
using Flunt.Validations;
using SimulateCredit.Domain.Enums;
using SimulateCredit.Domain.ValueObjects;

namespace SimulateCredit.Domain.Entities;

public class Simulation : Notifiable<Notification>
{

    public Simulation(
        decimal loanAmount,
        int termInMonths,
        Customer customer,
        decimal annualInterestRate,
        InterestRateType interestRateType,
        string currency)
    {
        Id = Guid.NewGuid();
        LoanAmount = loanAmount;
        TermInMonths = termInMonths;
        Customer = customer;
        AnnualInterestRate = annualInterestRate;
        InterestRateType = interestRateType;
        Currency = currency;

        AddNotifications(new Contract<Simulation>()
            .Requires()
            .IsGreaterThan(loanAmount, 0, nameof(LoanAmount), "Loan amount must be greater than zero.")
            .IsGreaterThan(termInMonths, 0, nameof(TermInMonths), "The term must be greater than zero.")
            .IsNotNull(customer, nameof(Customer), "Customer must be provided.")
            .IsNotNullOrEmpty(currency, nameof(Currency), "The currency must be informed.")
        );
        AddNotifications(customer);
    }

    public Guid Id { get; private set; }
    public decimal LoanAmount { get; private set; }
    public int TermInMonths { get; private set; }
    public Customer Customer { get; private set; }
    public decimal AnnualInterestRate { get; private set; }
    public string Currency { get; private set; }

    public decimal MonthlyPayment { get; private set; }
    public decimal TotalAmountToPay { get; private set; }
    public decimal TotalInterest { get; private set; }
    public InterestRateType InterestRateType { get; private set; }
    public void ApplySimulationResult(decimal monthlyPayment, decimal totalToPay, decimal totalInterest)
    {
        MonthlyPayment = monthlyPayment;
        TotalAmountToPay = totalToPay;
        TotalInterest = totalInterest;
    }
}