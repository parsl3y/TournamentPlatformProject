using Domain.Teams;
using FluentValidation;

namespace Application.Teams.Commands;

public class DeleteTeamValidator : AbstractValidator<DeleteTeamCommand>
{
    public DeleteTeamValidator()
    {
        RuleFor(x => x.TeamId).NotEmpty();
    }
}