using Amazon.S3;
using Amazon.S3.Model;
using Application.Common;
using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Application.Files.Exceptions;
using Domain.Countries;
using Domain.Faculties;
using MediatR;
using Domain.Players;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace Application.Files.Commands;

public class UploadPlayerImageCommand : IRequest<Result<string, UploadImageException>>
{
    public required Guid PlayerId { get; init; }
    public required IFormFile File { get; init; }
}

 public class UploadPlayerImageCommandHandler : IRequestHandler<UploadPlayerImageCommand, Result<string, UploadImageException>>
    {
        private readonly IAmazonS3 _client;
        private readonly IPlayerQueries _playerQueries;
        private readonly IPlayerImageRepository _playerImageRepository;
        private readonly string _bucketName;

        public UploadPlayerImageCommandHandler(IAmazonS3 client, IConfiguration config , IPlayerQueries playerQueries, IPlayerRepositories playerRepositories, IPlayerImageRepository playerImageRepository)
        {
            _client = client;
            _bucketName = config["AWS:BucketName"];
            _playerQueries = playerQueries;
            _playerImageRepository = playerImageRepository;
        }

        public async Task<Result<string, UploadImageException>> Handle(UploadPlayerImageCommand request, CancellationToken cancellationToken)
        {
            var playerId = new PlayerId(request.PlayerId);

            var doesImageExist = await _playerImageRepository.ExistsByPlayerId(playerId, cancellationToken);
            if (doesImageExist)
            {
                return new AlreadyHavePlayerImageException(playerId); 
            }

            var playerOption = await _playerQueries.GetById(playerId, cancellationToken);

            return await playerOption.Match(
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

                    var playerImage = PlayerImage.New(new PlayerImageId(Guid.NewGuid()), playerId, imageUrl);
                    await _playerImageRepository.Add(playerImage, cancellationToken);

                    return imageUrl;
                },
                () => Task.FromResult<Result<string, UploadImageException>>(
                    new NotFoundException(request.PlayerId))
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