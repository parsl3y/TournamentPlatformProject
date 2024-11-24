using Domain.Countries;
using Domain.Game;

namespace Test.Data;

public class CountryData
{
    public static Country MainCountry => Country.New(CountryId.New(),"Main Test Country");
}