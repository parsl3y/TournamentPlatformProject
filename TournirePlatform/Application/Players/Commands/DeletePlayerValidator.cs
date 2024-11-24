using FluentValidation;

namespace Application.Players.Commands;

public class DeletePlayerValidator : AbstractValidator<DeletePlayerCommand>
{
    public DeletePlayerValidator()
    {
        RuleFor(x => x.PlayerId).NotEmpty();
    }
}