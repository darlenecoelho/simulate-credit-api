namespace SimulateCredit.Domain.Entities;

public sealed class Simulation
{
    public string Email { get; set; } = string.Empty;
    public DateTime BirthDate { get; set; }
    public decimal LoanAmount { get; set; }
    public string LoanCurrency { get; set; } = "BRL";
    public int Months { get; set; }
    public string RateType { get; set; } = "Age";
    public string SourceCurrency { get; set; } = "BRL";
    public string TargetCurrency { get; set; } = "BRL";
    public decimal TotalAmount { get; set; }
    public decimal MonthlyPayment { get; set; }
    public decimal TotalInterest { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
