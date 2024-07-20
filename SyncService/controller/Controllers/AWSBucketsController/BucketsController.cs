using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace controller.Controllers.AWSBucketsController;

[Route("[controller]")]
[ApiController]
public class BucketsController : ControllerBase
{
    private readonly string _bucketName = "sync-music-storage";
    private readonly IAmazonS3 _s3Client;

    public BucketsController(IAmazonS3 s3Client)
    {
        _s3Client = s3Client;
    }

    [HttpGet("list-buckets")]
    [Authorize(Roles ="Admin")]
    public async Task<IActionResult> ListBukets()
    {
        var data = await _s3Client.ListBucketsAsync();
        var buckets = data.Buckets.Select(b => { return b.BucketName; });
        return Ok(buckets);
    }

    [HttpPost("upload-music")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UploadFile(IFormFile file)
    {
        if (file == null || file.Length == 0)
            return BadRequest("No file uploaded");

        var fileTransferUtility = new TransferUtility(_s3Client);
        var filePath = $"{file.FileName}";

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

        return Ok(new { Url = url });
    }
}