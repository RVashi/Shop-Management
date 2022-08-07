using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Helpers
{
    public class CommonMethods
    {
        public static int GetShopIdFromHeader(HttpContext httpContext)
        {
            if (String.IsNullOrWhiteSpace(httpContext.Request.Headers["X-ShopId"]))
            {
                return 0;
            }
            else
            {
                return Convert.ToInt32(httpContext.Request.Headers["X-ShopId"]);
            }
        }
    }
}
