using Application.FormatTournaments.Commands;
using FluentValidation;

namespace Application.FormatTournaments;

public class DeleteFormatCommandValidator : AbstractValidator<DeleteFormatCommand>
{
    public DeleteFormatCommandValidator()
    {
        RuleFor(x => x.FormatId).NotNull().NotEmpty();
    }
}