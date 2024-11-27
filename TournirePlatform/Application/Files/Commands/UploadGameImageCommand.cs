using Amazon.S3;
using Amazon.S3.Model;
using Application.Common;
using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Application.Files.Exceptions;
using Domain.Countries;
using Domain.Faculties;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace Application.Files.Commands;

public class UploadGameImageCommand : IRequest<Result<string, UploadGameImageException>>
{
    public required Guid GameId { get; init; }
    public required IFormFile File { get; init; }
}

 public class UploadGameImageCommandHandler : IRequestHandler<UploadGameImageCommand, Result<string, UploadGameImageException>>
    {
        private readonly IAmazonS3 _client;
        private readonly IGameQueries _gameQueries;
        private readonly IGameImageRepository _gameImageRepository;
        private readonly string _bucketName;

        public UploadGameImageCommandHandler(IAmazonS3 client, IConfiguration config , IGameQueries gameQueries, IGameRepositories gameRepositories, IGameImageRepository gameImageRepository)
        {
            _client = client;
            _bucketName = config["AWS:BucketName"];
            _gameQueries = gameQueries;
            _gameImageRepository = gameImageRepository;
        }

        public async Task<Result<string, UploadGameImageException>> Handle(UploadGameImageCommand request, CancellationToken cancellationToken)
        {
            var gameId = new GameId(request.GameId);

            var doesImageExist = await _gameImageRepository.ExistsByGameId(gameId, cancellationToken);
            if (doesImageExist)
            {
                return new GameAlreadyHaveAImageException(gameId); 
            }

            var gameOption = await _gameQueries.GetById(gameId, cancellationToken);

            return await gameOption.Match(
                async game =>
                {
                    var fileExtension = Path.GetExtension(request.File.FileName).ToLower();
                    var fileKey = $"post_images/{Guid.NewGuid()}{fileExtension}";

                    using var fileStream = request.File.OpenReadStream();

                    var uploadResult = await UploadFileAsync(_bucketName, fileStream, request.File.ContentType, fileKey, cancellationToken);
                    if (!uploadResult)
                    {
                        return new FileUploadFailedException();
                    }

                    var imageUrl = $"https://{_bucketName}.s3.amazonaws.com/{fileKey}";

                    var gameImage = GameImage.New(new GameImageId(Guid.NewGuid()), gameId, imageUrl);
                    await _gameImageRepository.Add(gameImage, cancellationToken);

                    return imageUrl;
                },
                () => Task.FromResult<Result<string, UploadGameImageException>>(
                    new GameNotFoundException(request.GameId))
            );
        }


    private async Task<bool> UploadFileAsync(string bucketName, Stream fileStream, string contentType, string fileKey, CancellationToken cancellationToken)
    {
        var request = new PutObjectRequest
        {
            BucketName = bucketName,
            Key = fileKey,
            InputStream = fileStream,
            CannedACL = S3CannedACL.PublicRead,
            ContentType = contentType
        };

        try
        {
            var response = await _client.PutObjectAsync(request, cancellationToken);
            return response.HttpStatusCode == System.Net.HttpStatusCode.OK;
        }
        catch (Exception e)
        {
            return false;
        }
    }
}