using SimulateCredit.Domain.Entities;
using SimulateCredit.Domain.Enums;

namespace SimulateCredit.Domain.Policies;

public class FixedRatePolicy : IRateCalculationPolicy
{
    public bool IsApplicable(Simulation simulation)
    {
        return simulation.InterestRateType == InterestRateType.Fixed;
    }

    public void Apply(Simulation simulation)
    {
        var age = simulation.Customer.GetAge();

        var annualRate = age switch
        {
            <= 25 => 5m,
            <= 40 => 3m,
            <= 60 => 2m,
            _ => 4m
        };

        var r = annualRate / 100 / 12;
        var n = simulation.TermInMonths;
        var pv = simulation.LoanAmount;

        var denominator = 1 - Math.Pow(1 + (double)r, -n);
        var monthlyPayment = (decimal)(((double)pv * (double)r) / denominator);
        var total = monthlyPayment * n;
        var interest = total - pv;

        simulation.ApplySimulationResult(monthlyPayment, total, interest);
    }
}
