using Amazon.S3;
using Amazon.S3.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Helpers
{
    public class S3ClientHelper
    {
        protected AmazonS3Config config;
        protected AmazonS3Client s3Client;

        protected AmazonResponseObject UploadFile(string BucketName, string FolderName, string FileName, Stream InputStream, S3CannedACL acl)
        {
            try
            {
                PutObjectRequest request = new PutObjectRequest()
                {
                    BucketName = BucketName,
                    CannedACL = acl,
                    Key = FolderName + "/" + FileName,
                    InputStream = InputStream
                };
                var task = s3Client.PutObjectAsync(request);
                task.Wait();
                /* Return Web Url*/
                return new AmazonResponseObject() { ObjectKey = request.Key, ObjectUrl = config.ServiceURL + "/" + BucketName + "/" + request.Key /* Return Web Url*/ };
            }
            catch
            {
                return null;
            }
        }
    }
    public class AmazonResponseObject
    {
        public string ObjectKey { get; set; }
        public string ObjectUrl { get; set; }
    }
}
