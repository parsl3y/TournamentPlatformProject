using Amazon.S3.Model;
using Microsoft.AspNetCore.Mvc;
using Application.Common.Interfaces.Repositories;
using Application.Files;
using Microsoft.EntityFrameworkCore;
using Application.Files.Commands;
using Application.Files.Exceptions;
using Domain.Countries;
using Domain.Faculties;
using MediatR;

namespace Api.Controllers
{
    [Route("file")]
    [ApiController]
    public class UploadController : ControllerBase
    {
        private readonly ISender _sender;
        private readonly IGameImageRepository _gameImageRepository;

        public UploadController(ISender sender, IGameImageRepository gameImageRepository)
        {
            _sender = sender;
            _gameImageRepository = gameImageRepository;
        }
        

        [HttpPost("uploadGame")]
        public async Task<ActionResult> Upload([FromForm] IFormFile file, [FromForm] Guid gameId,
            CancellationToken cancellationToken)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded.");

            var command = new UploadGameImageCommand()
            {
                GameId = gameId,
                File = file
            };

            var result = await _sender.Send(command, cancellationToken);

            return result.Match<ActionResult>(
                imageUrl => Ok(new { Message = "File uploaded successfully", ImageUrl = imageUrl }),
                e => e switch
                {
                    NotFoundException => NotFound(e.Message),
                    FileUploadFailedException => StatusCode(500, e.Message),
                    _ => StatusCode(500, "An unexpected error occurred.")
                }
            );
        }

        [HttpGet("GameGet/{gameId:guid}")]
         public async Task<ActionResult<IEnumerable<GameImage>>> GetBySneakerId(Guid gameId, CancellationToken cancellationToken)
         {
             var images = await _gameImageRepository.GetByGameId(new GameId(gameId), cancellationToken);
             if (!images.Any())
             {
                return NotFound("No images found for this sneaker.");
            }
             return Ok(images);
         }
    }
}