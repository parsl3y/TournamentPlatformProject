namespace Domain.TeamsMatchs;

public record TeamMatchId(Guid Value)
{
    public static TeamMatchId New() => new(Guid.NewGuid());
    public static TeamMatchId Empty() => new(Guid.Empty);
    
    public override string ToString() => Value.ToString();
}