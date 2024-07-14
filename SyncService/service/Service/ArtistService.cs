using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using core.Dtos.Album;
using core.Dtos.Artist;
using core.Models;
using Microsoft.AspNetCore.Http;
using repository.Repository.Interfaces;
using service.Service.Interfaces;

namespace service.Service;

public class ArtistService : IArtistService
{
    private readonly IArtistRepository _artistRepository;
    private readonly IAmazonS3 _s3Client;
    private readonly string _bucketName = "sync-music-storage";
    public ArtistService(IArtistRepository artistRepository, IAmazonS3 amazonS3)
    {
        _artistRepository = artistRepository;
        _s3Client = amazonS3;
    }



    public async Task<ArtistDTO> GetArtistDTOById(Guid id)
    {
        return await _artistRepository.GetArtistDTOById(id);
    }

    public async Task<List<MusicResponseDTO>> GetAllArtistMusicsAsync(Guid artistId)
    {
        return await _artistRepository.GetAllArtistMusicsAsync(artistId);
    }

    public async Task<Artist> GetArtistByUserIdAsync(Guid userId)
    {
        return await _artistRepository.GetArtistByUserId(userId);
    }

    public async Task<Artist> CreateArtist(Artist artist)
    {
        return await _artistRepository.CreateArtist(artist);
    }

    public async Task<bool> UpdateArtistImageAsync(Guid artistId, IFormFile image)
    {
        var artist = await _artistRepository.GetArtistByUserId(artistId);
        if (artist == null)
        {
            return false;
        }
        var artistImageUrl = UploadFileAsync(image);
        artist.ImageUrl = artistImageUrl.Result;
        return await _artistRepository.updateArtist(artist);
    }
    private async Task<string> UploadFileAsync(IFormFile file)
    {
        var fileTransferUtility = new TransferUtility(_s3Client);
        var fileExtension = Path.GetExtension(file.FileName);
        var filePath = $"image/{Guid.NewGuid()}{fileExtension}";

        using (var stream = file.OpenReadStream())
        {
            await fileTransferUtility.UploadAsync(stream, _bucketName, filePath);
        }

        var url = _s3Client.GetPreSignedURL(new GetPreSignedUrlRequest
        {
            BucketName = _bucketName,
            Key = filePath,
            Expires = DateTime.UtcNow.AddMinutes(30)
        });

        return url;
    }
}