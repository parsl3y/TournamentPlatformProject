namespace Domain.Countries;

public record CountryImageId(Guid Value)
{
    public static CountryImageId Empty => new(Guid.Empty);
    
    public static CountryImageId New() => new(Guid.NewGuid());
    
    public override string ToString() => Value.ToString();
}