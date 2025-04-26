

using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Transfer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;


namespace LeadsHub.Core.Services
{
    public sealed class AmazonS3Service
    { 
        private string AwsKeyAccess { get; set; }
        private string AwsKeySecret { get; set; }
        private string BucketName { get; set; }
        private BasicAWSCredentials Credentials { get; set; }

        private readonly IAmazonS3 _s3Client;

        public AmazonS3Service(IConfiguration config)
        {
            BucketName = config["AWS:S3:BucketName"]!;
            AwsKeyAccess = config["AWS:S3:AccessKey"]!;
            AwsKeySecret = config["AWS:S3:SecretKey"]!;
            Credentials = new BasicAWSCredentials(AwsKeyAccess, AwsKeySecret);

            var s3Config = new AmazonS3Config
            {
                RegionEndpoint = Amazon.RegionEndpoint.SAEast1,
            };

            _s3Client = new AmazonS3Client(Credentials, s3Config);
        }

        public async Task<bool> UploadFileAsync(string key, IFormFile file)
        {
            try
            {
                using var newMemoryStream = new MemoryStream();
                await file.CopyToAsync(newMemoryStream);

                var fileTransferUtility = new TransferUtility(_s3Client);

                await fileTransferUtility.UploadAsync(new TransferUtilityUploadRequest
                {
                    InputStream = newMemoryStream,
                    Key = key,
                    BucketName = BucketName,
                    ContentType = file.ContentType
                });
            }
            catch (Exception ex)
            {
                return false;
            }

            return true;
        }
    }
}
