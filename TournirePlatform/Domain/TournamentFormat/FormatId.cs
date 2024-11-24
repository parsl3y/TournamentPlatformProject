namespace Domain.TournamentFormat;

public record FormatId(Guid Value)
{
    public static FormatId Empty() => new(Guid.Empty);
    public static FormatId New() => new(Guid.NewGuid());
    public override string ToString() => Value.ToString();
}