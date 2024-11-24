using FluentValidation;

namespace Application.FormatTournaments.Commands;

public class UpdateFormatCommandValidator : AbstractValidator<UpdateFormatCommand>
{
    public UpdateFormatCommandValidator()
    {
        RuleFor(x => x.FormatId).NotEmpty();
        RuleFor(x => x.Name).NotEmpty().MaximumLength(255).MinimumLength(3);
    }
}