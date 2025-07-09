using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace SimulateCredit.Infrastructure.Adapters.Persistence.Mongo;

public sealed class SimulationDocument
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = default!;

    public string Email { get; set; } = default!;
    public DateTime BirthDate { get; set; }
    public decimal LoanAmount { get; set; }
    public string LoanCurrency { get; set; } = "BRL";
    public int Months { get; set; }
    public string RateType { get; set; } = "AGE";
    public string SourceCurrency { get; set; } = "BRL";
    public string TargetCurrency { get; set; } = "BRL";
    public decimal TotalAmount { get; set; }
    public decimal MonthlyPayment { get; set; }
    public decimal TotalInterest { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
