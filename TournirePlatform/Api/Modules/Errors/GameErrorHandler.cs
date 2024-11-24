using Application.Games.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace Api.Modules.Errors;

public static class GameErrorHandler
{
    public static ObjectResult ToObjectResult(this GameException e)
    {
        return new ObjectResult(e.Message)
        {
            StatusCode = e switch
            {
                GameNotFoundException => StatusCodes.Status404NotFound,
                GameAlreadExistsException => StatusCodes.Status409Conflict,
                GameUknownException => StatusCodes.Status500InternalServerError,
                _ => throw new NotImplementedException("Game error handler does not implemented")
            }
        };
    }
}
