using Application.Countries.Exceptions;
using Application.Games.Exceptions;
using Application.Matches.Exceptions;
using Application.Teams.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace Api.Modules.Errors;

public static class TeamErrorHandler
{
    public static ObjectResult ToObjectResult(this TeamException e)
    {
        return new ObjectResult(e.Message)
        {
            StatusCode = e switch
            {
                TeamNotFoundException => StatusCodes.Status404NotFound,
                TeamAlreadExistsException => StatusCodes.Status409Conflict,
                TeamUknownException => StatusCodes.Status500InternalServerError,
                _ => throw new NotImplementedException("Country error handler does not implemented")
            }
        };
    }
}