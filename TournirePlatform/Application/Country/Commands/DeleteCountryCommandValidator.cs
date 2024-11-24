using FluentValidation;

namespace Application.Countries.Commands;

public class DeleteCountryCommandValidator : AbstractValidator<DeleteCountryCommand>
{
    public DeleteCountryCommandValidator()
    {
        RuleFor(x => x.CountryId).NotNull().NotEmpty();
    }
}