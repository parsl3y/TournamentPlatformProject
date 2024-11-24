using System.ComponentModel;
using FluentValidation;

namespace Application.Matches.Commands;

public class CreateMatchCommandValidator : AbstractValidator<CreateMatchCommand>
{
    public CreateMatchCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(255).WithMessage("Name must not be empty");
        RuleFor(x => x.MaxTeams).NotEmpty().WithMessage("Max teams must not be empty");
        RuleFor(x => x.GameId).NotEmpty().WithMessage("Game id must not be empty");
        RuleFor(x => x.StartAt)
            .NotEmpty()
            .WithMessage("StartAt must not be empty")
            ;

    }

}