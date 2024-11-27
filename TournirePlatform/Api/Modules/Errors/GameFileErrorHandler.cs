using Application.Files.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace Api.Modules.Errors;

public static class GameFileErrorHandler
{
    public static ObjectResult ToObjectResult(this UploadGameImageException e)
    {
        return new ObjectResult(e.Message)
        {
            StatusCode = e switch
            {
                GameAlreadyHaveAImageException => StatusCodes.Status400BadRequest,
                FileUploadFailedException => StatusCodes.Status400BadRequest,
                GameNotFoundException => StatusCodes.Status404NotFound,
                _ => throw new NotImplementedException("gameImage error handler does not implemented")

            }
        };
    }
}