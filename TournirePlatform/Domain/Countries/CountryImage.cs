using Domain.Countries;

namespace Domain.Images;

public class CountryImage
{
    public CountryImageId Id { get; }

    public CountryId CountryId { get; }
    public CountryImage? Country { get; }

    public string S3Path { get; private set; }

    private CountryImage(CountryImageId id, CountryId countryId, string s3Path)
    {
        if (string.IsNullOrWhiteSpace(s3Path))
        {
            throw new ArgumentException("S3 path cannot be empty", nameof(s3Path));
        }

        Id = id;
        CountryId = countryId;
        S3Path = s3Path;
    }

    public void UpdateImageUrl(string newS3Path)
    {
        if (string.IsNullOrWhiteSpace(newS3Path))
        {
            throw new ArgumentException("New image URL cannot be empty", nameof(newS3Path));
        }

        S3Path = newS3Path;
    }

    public static CountryImage New(CountryImageId id, CountryId countryId, string s3Path)
        => new(id, countryId, s3Path);

    public string FilePath => $"{CountryId}/{Id}.png";
}