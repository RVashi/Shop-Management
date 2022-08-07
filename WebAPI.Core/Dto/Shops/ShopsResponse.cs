using System;
using System.Collections.Generic;
using System.Text;
using WebAPI.Core.Dto.Common;

namespace WebAPI.Core.Dto.Shops
{
    public class ShopListResponseDTO : ShopsResponseDTO
    {
        public int TotalRecords { get; set; } = 0;
    }

    public class ShopResponse
    {
        public int Id { get; set; }
        public string Shop { get; set; }
        public string Address { get; set; }
        public string ZipCode { get; set; }
        public string Location { get; set; }
        public string ProvinceName { get; set; }
    }

    public class ShopsResponseDTO : CommonResponse
    {
        public dynamic data { get; set; }
    }
}
