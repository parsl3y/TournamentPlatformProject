using Application.Countries.Exceptions;
using Application.Games.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace Api.Modules.Errors;

public static class CountryErrorHandler
{
    public static ObjectResult ToObjectResult(this CountryException e)
    {
        return new ObjectResult(e.Message)
        {
            StatusCode = e switch
            {
                CountryNotFoundException => StatusCodes.Status404NotFound,
                CountryAlreadyExistsException => StatusCodes.Status409Conflict,
                CountryUnknownException => StatusCodes.Status500InternalServerError,
                _ => throw new NotImplementedException("Country error handler does not implemented")
            }
        };
    }
}