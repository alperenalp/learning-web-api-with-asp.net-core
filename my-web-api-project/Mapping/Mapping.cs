using System;
using AutoMapper;
using My.Web.Api.Project.Dto;
using My.Web.Api.Project.Models;

namespace My.Web.Api.Project.Mapping
{
    public class AutoMapping : Profile
    {
        public AutoMapping()
        {
            CreateMap<Customer, CustomerDto>();
            CreateMap<Employee, EmployeeDto>();
            CreateMap<Order, OrderDto>();
            CreateMap<Employee, EmployeeNameDto>();
        }
    }
}