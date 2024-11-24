namespace Domain.Faculties;

public record GameImageId(Guid Value)
{
    public static GameImageId Empty => new(Guid.Empty);
    public static GameImageId New() => new(Guid.NewGuid());
    public override string ToString() => Value.ToString();
}