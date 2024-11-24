using Domain.Countries;

namespace Application.Countries.Exceptions;

public abstract class CountryException(CountryId id, string message, Exception? innerException = null)
    : Exception(message, innerException)
{
    public CountryId CountryId { get; } = id;
}

public class CountryNotFoundException(CountryId id) 
    : CountryException(id, $"Country with ID {id} not found.");

public class CountryAlreadyExistsException(CountryId id) 
    : CountryException(id, $"Country already exists with ID {id}.");

public class CountryUnknownException(CountryId id, Exception innerException) 
    : CountryException(id, $"Unknown exception for the country with ID {id}.", innerException);