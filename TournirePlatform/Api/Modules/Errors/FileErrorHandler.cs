using Application.Files.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace Api.Modules.Errors;

public static class FileErrorHandler
{
    public static ObjectResult ToObjectResult(this UploadImageException e)
    {
        return new ObjectResult(e.Message)
        {
            StatusCode = e switch
            {
                AlreadyHaveAImageException => StatusCodes.Status400BadRequest,
                FileUploadFailedException => StatusCodes.Status400BadRequest,
                PlayerAlreadyHasAnImageException => StatusCodes.Status400BadRequest,
                NotFoundException => StatusCodes.Status404NotFound,
                _ => throw new NotImplementedException("gameImage error handler does not implemented")

            }
        };
    }
}