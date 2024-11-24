using FluentValidation;

namespace Application.Teams.Commands;

public class UpdateTeamValidator : AbstractValidator<UpdateTeamCommand>
{
    public UpdateTeamValidator()
    {
        RuleFor(x => x.Name).NotEmpty();
        RuleFor(x =>x.MatchCount).NotEmpty();
        RuleFor(x => x.MatchCount).NotEmpty();
    }
}