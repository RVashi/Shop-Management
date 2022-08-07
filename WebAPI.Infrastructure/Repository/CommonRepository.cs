using Microsoft.Extensions.Options;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using System;
using System.IO;
using System.Net;
using System.Text;
using WebAPI.Core.Dto.Upload;
using WebAPI.Core.Interfaces.Repositories;
using WebAPI.Infrastructure.Common;
using WebAPI.Infrastructure.Context;

namespace WebAPI.Infrastructure.Repository
{
    public class CommonRepository : ICommonRepository
    {
        private readonly ImagesSettings _imagesSettings;
        public const string imageServerUrl = @"ftp://xx.xx.xxx.xxx/xxx.xxx.x/xxxxxxxx";

        public CommonRepository(ApplicationDBContext context, IOptions<ImagesSettings> imagesSettings)
        {
            _imagesSettings = imagesSettings.Value;
        }

        public UploadFileResponse UploadFileToServer(UploadRequest request)
        {
            UploadFileResponse _UploadFileResponse = new UploadFileResponse();
            string subPath = _imagesSettings.imageBaseUrl + request.folder; // Your code goes here
            string imageFullPath = string.Concat(subPath, "/", request.imageName);

            bool exists = System.IO.Directory.Exists(subPath);

            if (!exists)
                System.IO.Directory.CreateDirectory(subPath);

            File.WriteAllBytes(subPath + "/" + request.imageName, Convert.FromBase64String(request.image));

            _UploadFileResponse.success = true;
            _UploadFileResponse.imageUrl = string.Concat("/", request.folder, "/", request.imageName);


            if (!String.IsNullOrWhiteSpace(request.folder) && request.folder.Contains("productos"))
            {

                string fileExtention = new FileInfo(request.imageName).Extension;  // get the file extention.

                string fileNameWithoutExtention = new FileInfo(request.imageName).Name.Replace(new FileInfo(request.imageName).Extension, string.Empty); // get file name without extention.

                string newFileName = string.Empty;
                string mainFileFullPath = imageFullPath;
                string mainFileName = fileNameWithoutExtention;

                string[] splitFolders = request.folder.Split('/');
                if (splitFolders.Length == 2)
                {

                    byte[] imageArray = System.IO.File.ReadAllBytes(imageFullPath);

                    string[] sizeArray = new string[0];

                    sizeArray = _imagesSettings.imageSizesForProduct.Split(',');
                    if (sizeArray.Length > 0)
                    {
                        foreach (var newSize in sizeArray)
                        {
                            newFileName = mainFileName + "_" + newSize;

                            string filePath = mainFileFullPath.Replace(fileNameWithoutExtention, newFileName);
                            if (!File.Exists(filePath))
                            {
                                using (Image oldimage = Image.Load(mainFileFullPath))
                                {
                                    // Resize the given image in place and return it for chaining.
                                    // 'x' signifies the current image processing context.
                                    int newHeight = Convert.ToInt32(Convert.ToInt32(newSize) * oldimage.Height / oldimage.Width);
                                    oldimage.Mutate(x => x.Resize(Convert.ToInt32(newSize), newHeight));

                                    oldimage.Save(filePath);
                                }
                            }
                        }
                    }
                }
                else
                {
                    _UploadFileResponse.success = false;
                    _UploadFileResponse.imageUrl = "Product Folder not found.";
                }
            }
            return _UploadFileResponse;
        }

        public void fileUpload(string fileNameString, string folderPath, string fullFilePath)
        {
            string fileName = fileNameString;
            string fullName = fullFilePath;
            string userName = "planetatestftp";
            string password = "PlanetATEST#112233@5588";
            string server = string.Format("{0}/{1}", imageServerUrl, folderPath);

            bool dirExists = CommonMethods.DoesFtpDirectoryExist(server);
            if (!dirExists)
            {
                try
                {
                    WebRequest request2 = WebRequest.Create(server);
                    request2.Method = WebRequestMethods.Ftp.MakeDirectory;
                    request2.Credentials = new NetworkCredential(userName, password);
                    using (var resp = (FtpWebResponse)request2.GetResponse())
                    {
                        Console.WriteLine(resp.StatusCode);
                        resp.Close();
                    }

                }
                catch (WebException ex)
                {
                    FtpWebResponse response = (FtpWebResponse)ex.Response;
                    if (response.StatusCode == FtpStatusCode.ActionNotTakenFileUnavailable)
                    {
                        response.Close();
                    }
                    else
                    {
                        response.Close();
                    }
                }

            }

            // Get the object used to communicate with the server.
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(new Uri(string.Format("{0}/{1}", server, fileName)));
            request.Method = WebRequestMethods.Ftp.UploadFile;

            // This example assumes the FTP site uses anonymous logon.
            request.Credentials = new NetworkCredential(userName, password);

            // Copy the contents of the file to the request stream.
            byte[] fileContents;
            using (StreamReader sourceStream = new StreamReader(fullName))
            {
                fileContents = Encoding.UTF8.GetBytes(sourceStream.ReadToEnd());
            }

            request.ContentLength = fileContents.Length;

            using (Stream requestStream = request.GetRequestStream())
            {
                requestStream.Write(fileContents, 0, fileContents.Length);
            }

            using (FtpWebResponse response = (FtpWebResponse)request.GetResponse())
            {
                Console.WriteLine($"Upload File Complete, status {response.StatusDescription}");
            }
        }

    }
}