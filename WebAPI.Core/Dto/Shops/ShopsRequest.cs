using System;
using System.Collections.Generic;
using System.Text;

namespace WebAPI.Core.Dto.Shops
{
    public class ShopListRequest
    {
        public int pageNo { get; set; }
        public int pageSize { get; set; }
    }
}
