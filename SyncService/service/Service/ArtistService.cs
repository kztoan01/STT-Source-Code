using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using core.Dtos.Artist;
using core.Models;
using Microsoft.AspNetCore.Http;
using repository.Repository.Interfaces;
using service.Service.Interfaces;

namespace service.Service;

public class ArtistService : IArtistService
{
    private readonly IArtistRepository _artistRepository;
    private readonly string _bucketName = "sync-music-storage";
    private readonly IAmazonS3 _s3Client;

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

    public async Task<bool> UpdateArtistInforAsync(Guid userId, ArtistImageDTO artistImage)
    {
        var artist = await _artistRepository.GetArtistByUserId(userId);
        if (artist == null) return false;
        var artistImageUrl = await UploadFileAsync(artistImage.image);
        artist.ImageUrl = artistImageUrl;
        artist.artistDescription = artistImage.description;
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

        var aclResponse = await _s3Client.PutACLAsync(new PutACLRequest
        {
            BucketName = _bucketName,
            Key = filePath,
            CannedACL = S3CannedACL.PublicRead
        });

        var url = $"https://{_bucketName}.s3.amazonaws.com/{filePath}";

        return url;
    }
}