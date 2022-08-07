using System;
using System.Collections.Generic;
using System.Text;

namespace WebAPI.Infrastructure.Common
{
    public class ImagesSettings
    {
        public string imageBaseUrl { get; set; }
        public string imageSizesForProduct { get; set; }
        public string searchImageSizesForProduct { get; set; }
        public string deleteImageSizesForProduct { get; set; }
    }
}
