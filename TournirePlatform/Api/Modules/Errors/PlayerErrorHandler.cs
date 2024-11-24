using Application.Players.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace Api.Modules.Errors;

public static class PlayerErrorHandler
{
    public static ObjectResult ToObjectResult(this PlayerException e)
    {
        return new ObjectResult(e.Message)
        {
            StatusCode = e switch
            {
                PlayerNotFoundException => StatusCodes.Status404NotFound,
                PlayerAlreadyExistsException => StatusCodes.Status409Conflict,
                PlayerUknownException => StatusCodes.Status500InternalServerError,
                PlayerCountryNotFoundException => StatusCodes.Status404NotFound,
                PlayerGameNotFoundException => StatusCodes.Status404NotFound,
                PlayerTeamNotFoundException => StatusCodes.Status404NotFound,
                _ => throw new NotImplementedException("Player error handler does not implemented")
            }
        };
    }
}