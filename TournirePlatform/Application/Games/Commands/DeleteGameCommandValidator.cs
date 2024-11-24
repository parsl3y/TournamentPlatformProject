using FluentValidation;

namespace Application.Games.Commands;

public class DeleteGameCommandValidator : AbstractValidator<DeleteGameCommand>
{
    public DeleteGameCommandValidator()
    {
        RuleFor(x => x.GameId).NotNull().NotEmpty();
    }
}