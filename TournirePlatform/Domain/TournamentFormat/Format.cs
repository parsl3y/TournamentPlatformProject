namespace Domain.TournamentFormat;

public class Format
{
    public FormatId Id { get; set; }
    public string Name { get; set; }

    public Format(FormatId id, string name)
    {
        Id = id;
        Name = name;
    }
    
    public static Format New (FormatId id, string name)
        => new Format(id, name);
    
    public void UpdateDetails(string name)
    {
        Name = name;
    }
}