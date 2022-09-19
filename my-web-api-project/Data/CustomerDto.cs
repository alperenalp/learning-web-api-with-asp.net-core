using System;
using My.Web.Api.Project.Models;

namespace My.Web.Api.Project.Dto
{
    public class CustomerDto
    {
        public string CustomerId { get; set; }
        public string CompanyName { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
    }
    
}