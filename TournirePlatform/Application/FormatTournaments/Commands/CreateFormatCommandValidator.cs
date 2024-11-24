using FluentValidation;

namespace Application.FormatTournaments.Commands;

public class CreateFormatCommandValidator : AbstractValidator<CreateFormatCommand>
{
    public CreateFormatCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(255).MinimumLength(3);
    }
}