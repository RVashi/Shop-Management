using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace WebAPI.Core.Dto.Upload
{
    public class UploadRequest
    {
        public string imageName { get; set; }
        public string image { get; set; }
        public string folder { get; set; }
    }


    public class CommonFileModel
    {
        public IFormFile upload { get; set; }
    }
}
