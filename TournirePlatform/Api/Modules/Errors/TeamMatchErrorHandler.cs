using Application.TeamMatch.Exceptions;
using Domain.TeamsMatchs;
using Microsoft.AspNetCore.Mvc;

namespace Api.Modules.Errors;

public static class TeamMatchErrorHandler
{
    public static ObjectResult ToObjectResult(this TeamMatchException e)
    {
        return new ObjectResult(e.Message)
        {
            StatusCode = e switch
            {
                TeamAlreadyJoinInMatchException => StatusCodes.Status409Conflict,
                TeamNotFoundException => StatusCodes.Status404NotFound,
                MatchNotFoundException => StatusCodes.Status404NotFound,
                TeamsMatchNotFoundException => StatusCodes.Status404NotFound,
                TeamMatchNotFoundException => StatusCodes.Status404NotFound,
                TeamUknownMatchException => StatusCodes.Status400BadRequest,
                MatchIsAlreadyFullException => StatusCodes.Status400BadRequest,
                MatchInTeamMatchNotFoundException => StatusCodes.Status404NotFound,
                TeamNotInMatchException => StatusCodes.Status400BadRequest,
                TeamMatchUnknown => StatusCodes.Status400BadRequest,
                MatchWasFinishedException => StatusCodes.Status409Conflict,
                _ => throw new NotImplementedException("TeamMatch error handler does not implemented")
            }
        };
    }
}