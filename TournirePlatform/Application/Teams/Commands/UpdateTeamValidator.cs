using FluentValidation;

namespace Application.Teams.Commands;

public class UpdateTeamValidator : AbstractValidator<UpdateTeamCommand>
{
    public UpdateTeamValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Name is required.");
        
        RuleFor(x => x.MatchCount)
            .NotEmpty()
            .WithMessage("Match count is required.");

        RuleFor(x => x.WinCount)
            .NotEmpty()
            .WithMessage("Win count is required.");

        RuleFor(x => x.WinCount)
            .LessThanOrEqualTo(x => x.MatchCount)
            .WithMessage("Win count must be less than or equal to match count.");
    }
}
