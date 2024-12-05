using System.Data;
using FluentValidation;

namespace Application.Teams.Commands;

public class CreateTeamValidator : AbstractValidator<CreateTeamCommand>
{
    public CreateTeamValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(255);
        
        RuleFor(x => x.CreationDate)
            .NotEmpty();
        
        RuleFor(x => x.WinCount)
            .LessThanOrEqualTo(x => x.MatchCount)
            .WithMessage("Win count must be less than or equal to match count.");
    }
}
