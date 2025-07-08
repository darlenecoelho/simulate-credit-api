using SimulateCredit.Domain.Entities;
using SimulateCredit.Domain.Policies;

namespace SimulateCredit.Domain.Services;

public class SimulationLoanPolicyService
{
    private readonly IEnumerable<IRateCalculationPolicy> _policies;

    public SimulationLoanPolicyService(IEnumerable<IRateCalculationPolicy> policies)
    {
        _policies = policies;
    }

    public void ApplyPolicy(Simulation simulation)
    {
        var policy = _policies.FirstOrDefault(p => p.IsApplicable(simulation));

        if (policy is null)
            throw new InvalidOperationException("No applicable rate policy found for this simulation.");

        policy.Apply(simulation);
    }
}