using System;
using System.Collections.Generic;
using System.Text;

namespace WebAPI.Infrastructure.Common
{
    public class MailSettings
    {
        public string Mail { get; set; }
        public string DisplayName { get; set; }
        public string Password { get; set; }
        public string Host { get; set; }
        public int Port { get; set; }
        public string BaseURL { get; set; }
        public string ConfirmEmailBaseUrl { get; set; }
        public int EmailLinkValidTime { get; set; }
        public string SupportEmail { get; set; }
    }
}
