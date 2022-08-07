using System;
using System.Collections.Generic;
using System.Text;

namespace WebAPI.Core.Entities
{
    public class Shops
    {
        public int Id { get; set; } = 0;
        public string Shop { get; set; }
        public string MailChimpListID { get; set; }
        public bool? IsDeleted { get; set; }
        public string Address { get; set; }
        public string ZipCode { get; set; }
        public string Location { get; set; }
        public int? ProvinceId { get; set; }
        public string ProvinceName { get; set; }
        public int? CountryId { get; set; }
        public string CountryName { get; set; }
        public int? PhysicalStore { get; set; }
    }
}
