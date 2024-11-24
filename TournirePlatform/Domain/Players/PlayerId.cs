namespace Domain.Players;

public record PlayerId(Guid Value)
{
    public static PlayerId New() => new(Guid.NewGuid());
    public static PlayerId Empty() => new(Guid.Empty);
    public override string ToString() => Value.ToString();
}