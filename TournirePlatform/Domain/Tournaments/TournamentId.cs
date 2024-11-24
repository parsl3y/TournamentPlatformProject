namespace Domain.Tournaments;

public record TournamentId(Guid Value)
{
    public static TournamentId New() => new(Guid.NewGuid());
    public static TournamentId Empty() => new(Guid.Empty);
    public override string ToString() => Value.ToString();
} 