using Application.Matches.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace Api.Modules.Errors;

public static class MatchErrorHandler
{
   public static ObjectResult ToObjectResult(this MatchException e)
    {
        return new ObjectResult(e.Message)
        {
            StatusCode = e switch
            {
                MatchNotFoundException => StatusCodes.Status404NotFound,
                MatchAlreadyExistsException => StatusCodes.Status409Conflict,
                MatchUknownException => StatusCodes.Status500InternalServerError,
                MatchCannotBeDeletedException => StatusCodes.Status400BadRequest,
                MatchGameNotFoundException => StatusCodes.Status404NotFound,
                MatchTournamentNotFoundException => StatusCodes.Status404NotFound,
                MatchWasFinishedException => StatusCodes.Status500InternalServerError,
                MatchJoinInTournamentException => StatusCodes.Status500InternalServerError,
                _ => throw new NotImplementedException("Game error handler does not implemented")
            }
        };
    }

}