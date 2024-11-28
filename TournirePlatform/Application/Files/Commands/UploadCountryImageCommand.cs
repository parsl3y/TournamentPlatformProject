using Amazon.S3;
using Amazon.S3.Model;
using Application.Common;
using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Application.Files.Exceptions;
using Domain.Countries;
using Domain.Faculties;
using Domain.Images;
using MediatR;
using Domain.Players;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace Application.Files.Commands;

public class UploadCountryImageCommand : IRequest<Result<string, UploadImageException>>
{
    public required Guid CountryId { get; init; }
    public required IFormFile File { get; init; }
}

 public class UploadCountryImageCommandHandler : IRequestHandler<UploadCountryImageCommand, Result<string, UploadImageException>>
    {
        private readonly IAmazonS3 _client;
        private readonly ICountryQueries _countryQueries;
        private readonly ICountryImageRepository _countryImageRepository;
        private readonly string _bucketName;

        public UploadCountryImageCommandHandler(IAmazonS3 client, IConfiguration config , ICountryQueries countryQueries, ICountryRepositories countryRepositories, ICountryImageRepository countryImageRepository)
        {
            _client = client;
            _bucketName = config["AWS:BucketName"];
            _countryQueries = countryQueries;
            _countryImageRepository = countryImageRepository;
        }

        public async Task<Result<string, UploadImageException>> Handle(UploadCountryImageCommand request, CancellationToken cancellationToken)
        {
            var countryId = new CountryId(request.CountryId);

            var doesImageExist = await _countryImageRepository.ExistsByCountryId(countryId, cancellationToken);
            if (doesImageExist)
            {
                return new AlreadyHaveCountryImageException(countryId); 
            }

            var countryOption = await _countryQueries.GetById(countryId, cancellationToken);

            return await countryOption.Match(
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

                    var countryImage = CountryImage.New(new CountryImageId(Guid.NewGuid()), countryId, imageUrl);
                    await _countryImageRepository.Add(countryImage, cancellationToken);

                    return imageUrl;
                },
                () => Task.FromResult<Result<string, UploadImageException>>(
                    new NotFoundException(request.CountryId))
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