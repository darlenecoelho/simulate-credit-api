using SimulateCredit.Domain.Entities;

namespace SimulateCredit.Domain.Policies;

public interface IRateCalculationPolicy
{
    bool IsApplicable(Simulation simulation);
    void Apply(Simulation simulation);
}