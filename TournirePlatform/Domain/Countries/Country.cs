namespace Domain.Countries;

public class Country
{
    public CountryId Id { get; }

    public string Name { get; private set; }

    public Country(CountryId id, string name)
    {
        Id = id;
        Name = name;
    }
    public static Country New(CountryId id, string name)
        => new Country(id, name);
    public void UpdateDetails(string name)
    {
        Name = name;
    }
    
}