using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using My.Web.Api.Project.Models;

namespace My.Web.Api.Project.Dto
{
    public class OrderDto
    {
        public int OrderId { get; set; }
        public string CustomerId { get; set; }
        public int? EmployeeId { get; set; }
        public DateTime? OrderDate { get; set; }
        public DateTime? RequiredDate { get; set; }
        public DateTime? ShippedDate { get; set; }
    }
}