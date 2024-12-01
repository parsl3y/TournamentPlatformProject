namespace Domain.Teams;

public record TeamImageId(Guid Value)
{
    public static TeamImageId Empty => new(Guid.Empty);
    public static TeamImageId New() => new(Guid.NewGuid());
    public override string ToString() => Value.ToString();
}