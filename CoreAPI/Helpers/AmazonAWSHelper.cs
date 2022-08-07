using Amazon.S3;
using System.IO;

namespace WebAPI.Helpers
{
    public class AmazonAWSHelper : S3ClientHelper
    {
        /// <summary>
        /// This class is for AWS S3 storage
        /// </summary>
        public AmazonAWSHelper()
        {
            config = new AmazonS3Config()
            {
                ServiceURL = "https://s3.wasabisys.com"
            };

            s3Client = new AmazonS3Client("xxxxxxxxxxxxxxxxx",
                "xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx", config);
        }

        public AmazonResponseObject UploadFile(string BucketName, string FolderName, string FileName, Stream InputStream)
        {
            return UploadFile(BucketName, FolderName, FileName, InputStream, S3CannedACL.BucketOwnerFullControl);
        }
    }
}
