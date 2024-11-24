namespace Domain.Matches;

public record MatchId(Guid Value)
{
    public static MatchId Empty() => new(Guid.Empty);

    public static MatchId New() => new(Guid.NewGuid());
    public override string ToString() => Value.ToString();
}