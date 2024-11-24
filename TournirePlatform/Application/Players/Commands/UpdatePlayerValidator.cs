using FluentValidation;

namespace Application.Players.Commands;

public class UpdatePlayerValidator: AbstractValidator<UpdatePlayerCommand>
{
    public UpdatePlayerValidator()
    {
        RuleFor(x => x.NickName).NotEmpty().MaximumLength(255).MinimumLength(3);
        RuleFor(x => x.Rating).NotEmpty().GreaterThan(0);
    }
}