using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using My.Web.Api.Project.Models;

namespace My.Web.Api.Project.Dto
{
    public class EmployeeDto
    {
        //[Required, StringLength(30)]
        public string LastName { get; set; }
        //[Required, StringLength(30)]
        public string FirstName { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
    }

    public class EmployeeNameDto{
        public string FirstName { get; set; }
    }
}