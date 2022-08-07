using System;
using System.Collections.Generic;
using System.Text;

namespace WebAPI.Core.Dto
{
    public class MailRequest
    {
        public string ToEmail { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public string guidToken { get; set; }
        public string templateName { get; set; }
        public int emailType { get; set; }
        public string Password { get; set; }
        public string uuid { get; set; }
        public DateTime authTokenIssueAt { get; set; }
        public string orderNumber { get; set; }
        public string orderHtml { get; set; }
        public int? ShopId { get; set; }
        public string WebsiteUrl { get; set; }
        public string WebsiteText { get; set; }
    }
}
