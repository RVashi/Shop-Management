using System;
using System.Collections.Generic;
using System.Text;
using WebAPI.Core.Dto.Common;

namespace WebAPI.Core.Dto.Upload
{
    public class UploadResponse : CommonResponse
    {
        public UploadFileResponse data { get; set; }
    }

    public class UploadFileResponse
    {
        public bool success { get; set; }
        public string imageUrl { get; set; }
    }


    public class CommonUploadFileResponse
    {
        public bool success { get; set; }
        public string fileName { get; set; }
        public int uploaded { get; set; }
        public string error { get; set; }
        public string url { get; set; }
    }
}
