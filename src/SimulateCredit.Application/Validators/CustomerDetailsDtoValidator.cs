using FluentValidation;
using SimulateCredit.Application.DTOs;

namespace SimulateCredit.Application.Validators;

public sealed class CustomerDetailsDtoValidator : AbstractValidator<CustomerDetailsDto>
{
    public CustomerDetailsDtoValidator()
    {
        RuleFor(x => x.BirthDate)
            .LessThan(DateTime.Today).WithMessage("Birth date must be in the past.");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Email format is invalid.");
    }
}