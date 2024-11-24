namespace Domain.Teams;

public record TeamId(Guid Value)
{
    public static TeamId New() => new(Guid.NewGuid());
    public static TeamId Empty() => new(Guid.Empty);
    public override string ToString() => Value.ToString();
}