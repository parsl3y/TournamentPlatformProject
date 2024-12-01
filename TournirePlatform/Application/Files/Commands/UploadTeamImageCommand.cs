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
using Domain.Teams;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace Application.Files.Commands;

public class UploadTeamImageCommand : IRequest<Result<string, UploadImageException>>
{
    public required Guid TeamId { get; init; }
    public required IFormFile File { get; init; }
}

 public class UploadTeamImageCommandHandler : IRequestHandler<UploadTeamImageCommand, Result<string, UploadImageException>>
    {
        private readonly IAmazonS3 _client;
        private readonly ITeamQuery _teamQuery;
        private readonly ITeamImageRepository _teamImageRepository;
        private readonly string _bucketName;

        public UploadTeamImageCommandHandler(IAmazonS3 client, IConfiguration config , ITeamQuery teamQuery, ITeamRepository teamRepositories, ITeamImageRepository teamImageRepository)
        {
            _client = client;
            _bucketName = config["AWS:BucketName"];
            _teamQuery = teamQuery;
            _teamImageRepository = teamImageRepository;
        }

        public async Task<Result<string, UploadImageException>> Handle(UploadTeamImageCommand request, CancellationToken cancellationToken)
        {
            var teamId = new TeamId(request.TeamId);

            var doesImageExist = await _teamImageRepository.ExistsByTeamId(teamId, cancellationToken);
            if (doesImageExist)
            {
                return new AlreadyHaveTeamImageException(teamId); 
            }

            var teamOption = await _teamQuery.GetById(teamId, cancellationToken);

            return await teamOption.Match(
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

                    var teamImage = TeamImage.New(new TeamImageId(Guid.NewGuid()), teamId, imageUrl);
                    await _teamImageRepository.Add(teamImage, cancellationToken);

                    return imageUrl;
                },
                () => Task.FromResult<Result<string, UploadImageException>>(
                    new NotFoundException(request.TeamId))
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