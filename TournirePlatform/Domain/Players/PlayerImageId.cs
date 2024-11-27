namespace Domain.Players;

public record PlayerImageId(Guid Value)
{
    public static PlayerImageId Empty => new(Guid.Empty);
    public static PlayerImageId New() => new(Guid.NewGuid());
    public override string ToString() => Value.ToString();
}