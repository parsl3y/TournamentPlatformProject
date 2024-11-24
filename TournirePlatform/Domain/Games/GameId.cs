namespace Domain.Countries;

public record GameId(Guid Value)
{
    public static GameId Empty() => new(Guid.Empty);
    public static GameId New() => new(Guid.NewGuid());
    public override string ToString() => Value.ToString();
}