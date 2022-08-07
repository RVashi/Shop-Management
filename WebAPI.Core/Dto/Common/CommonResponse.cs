using System;
using System.Collections.Generic;
using System.Text;

namespace WebAPI.Core.Dto.Common
{
    public class CommonResponse
    {
        public bool success { get; set; }
        public int responseCode { get; set; }
        public string responseMessage { get; set; }
    }
}
