using System.Data;
using System.Xml.Schema;
using FluentValidation;

namespace Application.Players.Commands;

public class CreatePlayerValidator : AbstractValidator<CreatePlayerCommand>
{
    public CreatePlayerValidator()
    {
        RuleFor(x => x.NickName).NotEmpty().MaximumLength(255).MinimumLength(3);
        RuleFor(x => x.GameId).NotEmpty();
        RuleFor(x => x.CountryId).NotEmpty();
        RuleFor(x => x.Rating).NotEmpty().GreaterThan(0);
    }
}