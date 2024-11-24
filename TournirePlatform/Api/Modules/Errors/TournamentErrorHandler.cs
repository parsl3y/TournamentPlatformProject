using Application.Tournaments.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace Api.Modules.Errors;

public static class TournamentErrorHandler
{
    public static ObjectResult ToObjectResult(this TournamentException e)
    {
        return new ObjectResult(e.Message)
        {
            StatusCode = e switch
            {
                TournamentNotFoundException => StatusCodes.Status404NotFound,
                TournamentAlreadyExistsException => StatusCodes.Status409Conflict,
                TournamentUnknownException => StatusCodes.Status400BadRequest,
                TournamentGameNotFoundException => StatusCodes.Status404NotFound,
                TournamentCountryNotFoundException =>   StatusCodes.Status404NotFound,
                TournamentFormatNotFoundException => StatusCodes.Status404NotFound,
                _ => throw new NotImplementedException("Tournament error handler does not implemented")

            }
        };
    }
}