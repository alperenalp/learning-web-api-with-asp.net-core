using My.Web.Api.Project.Services;
using Swashbuckle.AspNetCore.Annotations;
using Google.OrTools.LinearSolver;
using System;
using System.Collections.Generic;
using System.Linq;
using Google.OrTools.Sat;
using System.Security.Claims;
using My.Web.Api.Project.Helpers;
using Microsoft.AspNetCore.Mvc;
using My.Web.Api.Project.Models;
using My.Web.Api.Project.Dto;
using System.Threading.Tasks;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Localization;
using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace My.Web.Api.Project.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    [Route("api/core")]
    public class CoreController : ControllerBase
    {

        private ICoreService _coreService;

        public CoreController(ICoreService coreService)
        {
            _coreService = coreService;
        }

        /// <summary>
        /// Haftanın tüm günlerini getirir
        /// </summary>
        [HttpGet("/week")]
        public ActionResult<string> GetAllDays()
        {
            string allDays = _coreService.GetAllDay();
            return Ok(allDays);
        }

        /// <summary>
        /// Hafta içi günlerini getirir
        /// </summary>
        [HttpGet("/midweek")]
        public ActionResult<string> GetMidWeek()
        {
            string midWeek = _coreService.GetMidweek();
            return Ok(midWeek);
        }

        /// <summary>
        /// Hafta sonu günlerini getirir
        /// </summary>
        [HttpGet("/weekend")]
        public ActionResult<string> GetWeekend()
        {
            string weekend = _coreService.GetWeekend();
            return Ok(weekend);
        }

        /// <summary>
        /// Girilen kelimenin içerdiği günler getirilir
        /// </summary>
        [HttpGet("/specialday")]
        public ActionResult<string> GetSpecialDays([FromQuery] string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return BadRequest("Lütfen bir değer giriniz!");
            }

            string speacialDay = _coreService.GetSpecialDay(input);
            if (string.IsNullOrEmpty(speacialDay))
            {
                return NotFound("Aranan kelimeyi içeren günler bulunamadı!");
            }
            else
            {
                return Ok("Bulunan günler: \n" + speacialDay);
            }
        }

        /// <summary>
        /// Tüm müşterileri getirir
        /// </summary>
        [HttpGet("/customer/all")]
        public List<Customer> GetAllCustomer()
        {
            List<Customer> allCustomerList = _coreService.GetAllCustomer();
            return allCustomerList;
        }

        /// <summary>
        /// id ile eşleşen müşteriyi getirir
        /// </summary>
        [HttpGet("/customer/{id}")]
        public ActionResult GetCustomerById(string id)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(id))
                {
                    return BadRequest("Lütfen bir değer giriniz!");
                }

                CustomerDto customer = _coreService.GetCustomerById(id);
                if (customer == null)
                {
                    return NotFound($"{id} ile eşleşen müşteri bulunamadı!");
                }
                else
                {
                    return Ok(customer);
                }
            }
            catch (System.Exception)
            {
                return BadRequest("Bilinmeyen bir hata oluştu");
            }

        }

        /// <summary>
        /// Girilen ülkenin müşterileri listelenir
        /// </summary>
        [HttpGet("/customer/country")]
        public ActionResult GetCustomerByCountry([Required] string country)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(country))
                {
                    return BadRequest("Lütfen bir değer giriniz!");
                }

                List<Customer> customerList = _coreService.GetCustomerByCountry(country);
                if (customerList.Count() == 0)
                {
                    return NotFound($"{country} ülkeli hiçbir müşteri bulunamadı!");
                }
                else
                {
                    return Ok(customerList);
                }
            }
            catch (System.Exception)
            {
                return BadRequest("Bilinmeyen bir hata oluştu");
            }

        }

        /// <summary>
        /// Girilen isime sahip çalışanları getirir.
        /// </summary>
        [HttpGet("/employee/{name}")]
        public ActionResult GetEmployeeByName([Required] string name)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(name))
                {
                    return BadRequest("Lütfen bir değer giriniz!");
                }

                List<EmployeeDto> employeeListDto = _coreService.GetEmployeeByName(name);
                if (employeeListDto.Count() == 0)
                {
                    return NotFound($"{name} isimli hiçbir çalışan bulunamadı!");
                }
                else
                {
                    return Ok(employeeListDto);
                }
            }
            catch (System.Exception)
            {
                return BadRequest("Bilinmeyen bir hata oluştu");
            }

        }

        /// <summary>
        /// Girilen id'ye sahip çalışanın ismini getirir
        /// </summary>
        [HttpGet("/employee/name/{id}")]
        public ActionResult<string> GetEmployeeNameById(int id)
        {
            try
            {
                if (id < 0)
                {
                    return BadRequest("Lütfen doğru bir id giriniz");
                }

                EmployeeNameDto firstName = _coreService.GetEmployeeNameById(id);
                if (firstName == null)
                {
                    return NotFound($"{id} 'ye ait çalışan bulunamadı!");
                }
                else
                {
                    return Ok(firstName);
                }
            }
            catch (System.Exception)
            {
                return BadRequest("Bilinmeyen bir hata oluştu");
            }

        }

        /// <summary>
        /// Girilen tarihlere göre arama yapar
        /// </summary>
        [HttpGet("/order/date")]
        //[DataType(DataType.Date, ErrorMessage = "Date is required")]
        //[DisplayFormat(DataFormatString = "{yyyy/MM/dd:0}", ApplyFormatInEditMode = true)]
        public ActionResult GetOrderByDate([Required] string dateBegin, string dateEnd)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(dateEnd))
                {
                    dateEnd = "2022-01-01"; // DateTime.Now;
                }

                DateTime dateFirst;
                DateTime dateLast;
                try
                {
                    dateFirst = DateTime.Parse(dateBegin);
                    dateLast = DateTime.Parse(dateEnd);
                }
                catch (System.Exception)
                {
                    return BadRequest("Hatalı tarih formatı");
                }

                List<OrderDto> orderList = _coreService.GetOrderByDate(dateFirst, dateLast);
                if (orderList.Count() == 0)
                {
                    return NotFound($"{dateFirst} - {dateLast} arasında sipariş bulunamadı!");
                }
                else
                {
                    return Ok(orderList);
                }
            }
            catch (System.Exception)
            {
                return BadRequest("Bilinmeyen bir hata oluştu");
            }

        }

        /// <summary>
        /// id'si girilen çalışanı günceller
        /// </summary>
        [HttpPut("/employee/update/{id:int}")]
        public ActionResult UpdateEmployee(int id, [FromBody] EmployeeDto employeeDto)
        {
            try
            {

                if (id < 0)  // id.GetType() != typeof(int) || id is int 
                {
                    return BadRequest("Lütfen doğru bir id giriniz");
                }

                if (employeeDto is null)
                {
                    return BadRequest("Lütfen değiştirmek istediğiniz alanları giriniz");
                }

                if (string.IsNullOrWhiteSpace(employeeDto.FirstName.ToString()) ||
                 string.IsNullOrWhiteSpace(employeeDto.LastName.ToString()))
                {
                    return BadRequest("Çalışan ismi boş olamaz!");
                }

                // apicontroller niteliği zaten kontrol sağlıyormuş
                // if (!ModelState.IsValid)
                // {
                //     return BadRequest("Invalid model object");
                // }

                Employee employee = _coreService.UpdateEmployee(id, employeeDto);
                if (employee == null)
                {
                    return NotFound("Çalışan Bulunamadı");
                }
                else
                {
                    return Ok("Güncelleme başarılı");
                }

            }
            catch (System.Exception)
            {
                return BadRequest("Bilinmeyen bir hata oluştu");
            }

        }


        /// <summary>
        /// id'si girilen çalışanı jsonpatch yöntemi ile günceller
        /// </summary>
        [HttpPatch("/employee/patch/{id:int}")]
        public IActionResult PatchEmployee(int id, JsonPatchDocument<Employee> patchDocument)
        {
            try
            {
                if (id < 0)
                {
                    return BadRequest("Lütfen doğru bir id giriniz");
                }

                Employee employee = _coreService.PatchEmployee(id, patchDocument);
                if (employee == null) 
                {
                    return NotFound("Employee Bulunamadı");
                }
                else
                {
                    return Ok("Güncelleme başarılı");
                }
            }
            catch (System.Exception)
            {
                return BadRequest("Bilinmeyen bir hata oluştu");
            }
        }

        /// <summary>
        /// Girilen bilgilere göre arama yapar
        /// </summary>
        [HttpGet("/customer/search")]
        public IActionResult SearchCustomer(string search, string country, string city, string companyName)
        {
            try
            {
                List<Customer> customerList = _coreService.Search(search, country, city, companyName);
                if (customerList.Count() == 0)
                {
                    return BadRequest("Müşteri bulunamadı");
                }
                else
                {
                    return Ok(customerList);
                }
            }
            catch (System.Exception)
            {
                return BadRequest("Bilinmeyen bir hata oluştu");
            }
        }

        /// <summary>
        /// Employee oluşturur
        /// </summary>
        [HttpPost("/employee/create")]
        public IActionResult CreateEmployee([FromBody] EmployeeDto employeeDto)
        {
            try
            {
                if (employeeDto is null)
                {
                    return BadRequest("Lütfen bilgileri doldurunuz");
                }

                if (string.IsNullOrWhiteSpace(employeeDto.FirstName.ToString()) ||
                 string.IsNullOrWhiteSpace(employeeDto.LastName.ToString()))
                {
                    return BadRequest("Çalışan ismi boş olamaz!");
                }
                else
                {
                    _coreService.CreateEmployee(employeeDto);
                    return Ok("Çalışan başarıyla kayıt edildi.");
                }
            }
            catch (System.Exception)
            {
                return BadRequest("Bilinmeyen bir hata oluştu");
            }

        }

        /// <summary>
        /// id numarası ile Employee siler
        /// </summary>
        [HttpDelete("/employee/delete/{id:int}")]
        public IActionResult DeleteEmployee(int id)
        {
            try
            {
                if (id < 0)
                {
                    return BadRequest("Lütfen doğru bir id giriniz");
                }

                string result = _coreService.DeleteEmployee(id);
                if (result ==  null)
                {
                    return NotFound("Silinecek çalışan bulunamadı");
                }
                else
                {
                    return Ok(result);
                }
            }
            catch (System.Exception)
            {
                return BadRequest("Bilinmeyen bir hata oluştu");
            }

        }


    }
}