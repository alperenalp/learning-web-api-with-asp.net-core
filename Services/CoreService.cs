using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Options;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using My.Web.Api.Project.Helpers;
using My.Web.Api.Project.Models;
using Microsoft.Extensions.DependencyInjection;
using My.Web.Api.Project.Dto;
using Microsoft.AspNetCore.JsonPatch;

namespace My.Web.Api.Project.Services
{
    public interface ICoreService
    {
        string GetAllDay();
        string GetMidweek();
        string GetWeekend();
        string GetSpecialDay(string input);
        List<Customer> GetAllCustomer();
        CustomerDto GetCustomerById(string input);
        List<Customer> GetCustomerByCountry(string input);
        List<EmployeeDto> GetEmployeeByName(string input);
        EmployeeNameDto GetEmployeeNameById(int id);
        List<Order> GetAllOrder();
        List<OrderDto> GetOrderByDate(DateTime dateFirst, DateTime dateLast);
        Employee UpdateEmployee(int id, EmployeeDto employeeDto);
        Employee PatchEmployee(int id, JsonPatchDocument<Employee> patchDocument);
        List<Customer> Search(string search, string country, string city, string companyName);
        void CreateEmployee(EmployeeDto employee);
        string DeleteEmployee(int id);

    }
    public class CoreService : ICoreService
    {
        private IServiceProvider _provider;
        private readonly NorthwindContext _context;
        private readonly IMapper _mapper;
        public CoreService(NorthwindContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        string[] weekArray = { "Pazartesi", "Salı", "Çarşamba", "Perşembe", "Cuma", "Cumartesi", "Pazar" };
        public string GetAllDay()
        {
            string week = "";
            foreach (string day in weekArray)
            {
                if (day == "Pazar")
                    week += day;
                else
                    week += day + " - ";
            }

            return week;
        }

        public string GetMidweek()
        {
            string week = "";
            for (int i = 0; i < weekArray.Length - 2; i++)
            {
                if (weekArray[i] == "Pazar")
                    week += weekArray[i];
                else
                    week += weekArray[i] + " - ";
            }

            return week;
        }

        public string GetWeekend()
        {
            string week = "";
            for (int i = 5; i < weekArray.Length; i++)
            {
                if (weekArray[i] == "Pazar")
                    week += weekArray[i];
                else
                    week += weekArray[i] + " - ";

            }
            return week;
        }

        public string GetSpecialDay(string input)
        {
            string week = "";
            foreach (string day in weekArray)
            {
                if (day.IndexOf(input) != -1)
                    week += day + " - ";
            }

            return week;
        }
        public List<Customer> GetAllCustomer()
        {
            List<Customer> customerList = _context.Customers.ToList();
            return customerList;
        }

        public CustomerDto GetCustomerById(string id)
        {
            Customer customer = _context.Customers.Where(x => x.CustomerId == id).SingleOrDefault();
            CustomerDto customerDto = _mapper.Map<CustomerDto>(customer);
            return customerDto;
        }

        public List<Customer> GetCustomerByCountry(string input)
        {
            var query = from Customer in _context.Customers
                        where Customer.Country.Contains(input) // Contains ülkenin tam ismi olmasa bile getirir ör: Germany, germ 
                        select Customer;
            List<Customer> customerList = query.ToList();
            return customerList;
        }

        public List<EmployeeDto> GetEmployeeByName(string input)
        {
            var query = from Employee in _context.Employees
                        where Employee.FirstName == input
                        select Employee;
            List<Employee> employeeList = query.ToList();
            List<EmployeeDto> emlooyeeDtoList = _mapper.Map<List<EmployeeDto>>(employeeList);
            return emlooyeeDtoList;
        }

        public EmployeeNameDto GetEmployeeNameById(int id)
        {
            Employee emloyee = _context.Employees.Where(x => x.EmployeeId == id).SingleOrDefault();
            EmployeeNameDto employeeNameDto = _mapper.Map<EmployeeNameDto>(emloyee);
            return employeeNameDto;
        }

        public List<Order> GetAllOrder()
        {
            List<Order> orderList = _context.Orders.ToList();
            return orderList;
        }

        public List<OrderDto> GetOrderByDate(DateTime dateBegin, DateTime dateEnd)
        {
            List<Order> orderList = _context.Orders.Where(x => x.OrderDate > dateBegin && x.OrderDate < dateEnd).ToList();
            List<OrderDto> orderListDto = _mapper.Map<List<OrderDto>>(orderList);
            return orderListDto;
        }

        public Employee UpdateEmployee(int id, EmployeeDto employeeDto)
        {
            Employee employee = _context.Employees.Where(x => x.EmployeeId == id).SingleOrDefault();
            if (employee == null)
                return null;
            employee.FirstName = employeeDto.FirstName;
            employee.LastName = employeeDto.LastName;
            employee.Country = employeeDto.Country;
            employee.City = employeeDto.City;
            employee.Address = employeeDto.Address;
            _context.SaveChanges();

            Employee newEmployee = _context.Employees.Where(x => x.EmployeeId == id).SingleOrDefault();
            return newEmployee;
        }

        public Employee PatchEmployee(int id, JsonPatchDocument<Employee> patchDocument)
        {
            Employee employee = _context.Employees.Where(x => x.EmployeeId == id).SingleOrDefault();
            if (employee == null)
                return null;
            patchDocument.ApplyTo(employee);
            _context.SaveChanges();
            return employee;
        }

        public List<Customer> Search(string search, string country, string city, string companyName)
        {
            List<Customer> query = null;
            if (!string.IsNullOrWhiteSpace(search))
            {
                query = _context.Customers.Where(it => it.CompanyName.Contains(search) || it.Country.Contains(search) || it.City.Contains(search)).ToList();

                if (!string.IsNullOrWhiteSpace(country))
                {
                    query = query.Where(it => it.Country.Contains(country)).ToList();
                }
                if (!string.IsNullOrWhiteSpace(city))
                {
                    query = query.Where(it => it.City.Contains(city)).ToList();
                }
                if (!string.IsNullOrWhiteSpace(companyName))
                {
                    query = query.Where(it => it.CompanyName.Contains(companyName)).ToList();
                }
                return query;
            }

            if (string.IsNullOrWhiteSpace(search))
            {
                query = _context.Customers.Where(it => it.CompanyName.Contains(companyName) || it.Country.Contains(country) || it.City.Contains(city)).ToList();
                if (!string.IsNullOrWhiteSpace(country))
                {
                    //query = query.Where(it => EF.Functions.Like(it.Country, "%"+country+"%")).ToList();
                    query = query.Where(it => it.Country.Contains(country)).ToList();
                }
                if (!string.IsNullOrWhiteSpace(city))
                {
                    query = query.Where(it => it.City.Contains(city)).ToList();
                }
                if (!string.IsNullOrWhiteSpace(companyName))
                {
                    query = query.Where(it => it.CompanyName.Contains(companyName)).ToList();
                }
                return query;
            }
            return null;

        }

        public void CreateEmployee(EmployeeDto employeeDto)
        {
            Employee newEmployee = new Employee();
            newEmployee.FirstName = employeeDto.FirstName;
            newEmployee.LastName = employeeDto.LastName;
            newEmployee.City = employeeDto.City;
            newEmployee.Country = employeeDto.Country;
            _context.Employees.Add(newEmployee);
            _context.SaveChanges();
        }


        public string DeleteEmployee(int id)
        {
            Employee employee = _context.Employees.Where(p => p.EmployeeId == id).SingleOrDefault();
            if (employee != null)
            {
                _context.Employees.Remove(employee);
                _context.SaveChanges();
                return "Çalışan başarıyla silindi";
            }
            else
            {
                return null;
            }
        }
    }
}
