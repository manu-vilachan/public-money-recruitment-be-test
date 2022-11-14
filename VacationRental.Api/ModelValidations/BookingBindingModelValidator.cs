using FluentValidation;
using VacationRental.Api.Models;

namespace VacationRental.Api.ModelValidations;

public class BookingBindingModelValidator : AbstractValidator<BookingBindingModel>
{
    public BookingBindingModelValidator()
    {
        RuleFor(model => model.Nights).GreaterThan(0).WithMessage("Nights must be positive");
    }
}