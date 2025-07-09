using FluentValidation;
using SimulateCredit.Application.DTOs;
using SimulateCredit.Domain.ValueObjects;

namespace SimulateCredit.Application.Validators;

public sealed class LoanAmountDtoValidator : AbstractValidator<LoanAmountDto>
{
    public LoanAmountDtoValidator()
    {
        RuleFor(x => x.Amount)
            .GreaterThan(0).WithMessage("Loan amount must be greater than zero.");

        RuleFor(x => x.Currency)
            .NotNull().WithMessage("Currency is required.")
            .Must(c => Currency.IsValid(c.ToString()))
            .WithMessage("Currency must be a valid ISO 4217 code (e.g., USD, BRL, EUR).");
    }
}
