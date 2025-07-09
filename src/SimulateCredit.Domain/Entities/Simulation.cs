namespace SimulateCredit.Domain.Entities;

public sealed class Simulation
{
    public Simulation(
        string email,
        DateTime birthDate,
        decimal loanAmount,
        string loanCurrency,
        int months,
        string rateType,
        string sourceCurrency,
        string targetCurrency,
        decimal totalAmount,
        decimal monthlyPayment,
        decimal totalInterest)
    {
        Email = email;
        BirthDate = birthDate;
        LoanAmount = loanAmount;
        LoanCurrency = loanCurrency;
        Months = months;
        RateType = rateType;
        SourceCurrency = sourceCurrency;
        TargetCurrency = targetCurrency;
        TotalAmount = totalAmount;
        MonthlyPayment = monthlyPayment;
        TotalInterest = totalInterest;
        CreatedAt = DateTime.UtcNow;
    }
    public string Email { get; private set; }
    public DateTime BirthDate { get; private set; }
    public decimal LoanAmount { get; private set; }
    public string LoanCurrency { get; private set; }
    public int Months { get; private set; }
    public string RateType { get; private set; }
    public string SourceCurrency { get; private set; }
    public string TargetCurrency { get; private set; }
    public decimal TotalAmount { get; private set; }
    public decimal MonthlyPayment { get; private set; }
    public decimal TotalInterest { get; private set; }
    public DateTime CreatedAt { get; private set; }
}
