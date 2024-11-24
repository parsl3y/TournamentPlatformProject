using Application.FormatTournaments.Exceptions;
using Microsoft.AspNetCore.Mvc;
using FormatException = System.FormatException;

namespace Api.Modules.Errors;

public static class FormatErrorHandler
{
    public static ObjectResult ToObjectResult(this TournamentFormatException e)
    {
        return new ObjectResult(e.Message)
        {
            StatusCode = e switch
            {
                FormatNotFoundException  => StatusCodes.Status404NotFound,
                FormatAlreadyExistsException => StatusCodes.Status409Conflict,
                FormatUnknownException => StatusCodes.Status400BadRequest,
                _ => throw new NotImplementedException("Tournament error handler does not implemented")

            }
        };
    }
}