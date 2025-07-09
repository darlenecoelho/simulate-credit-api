namespace SimulateCredit.Application.DTOs;

public sealed class SimulationResult
{
    public SimulateCreditResponse Response { get; init; } = new();
    public string? DocumentId { get; init; } 
}
