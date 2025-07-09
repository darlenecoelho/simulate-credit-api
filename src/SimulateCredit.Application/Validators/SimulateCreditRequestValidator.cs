using FluentValidation;
using SimulateCredit.Application.DTOs;
using SimulateCredit.Domain.ValueObjects;

namespace SimulateCredit.Application.Validators;

public sealed class SimulateCreditRequestValidator : AbstractValidator<SimulateCreditRequest>
{
    public SimulateCreditRequestValidator()
    {
        RuleFor(x => x.LoanAmount)
            .NotNull()
            .SetValidator(new LoanAmountDtoValidator());

        RuleFor(x => x.Customer)
            .NotNull()
            .SetValidator(new CustomerDetailsDtoValidator());

        RuleFor(x => x.Months)
            .GreaterThan(0).WithMessage("Loan term (months) must be greater than 0.");

        RuleFor(x => x.SourceCurrency)
            .Must(c => Currency.IsValid(c.ToString()))
            .WithMessage("Source currency must be a valid ISO 4217 code.");

        RuleFor(x => x.TargetCurrency)
            .Must(c => Currency.IsValid(c.ToString()))
            .WithMessage("Target currency must be a valid ISO 4217 code.");
    }
}

