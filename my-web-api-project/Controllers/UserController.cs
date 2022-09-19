using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using My.Web.Api.Project.Data;
using My.Web.Api.Project.Models;
using My.Web.Api.Project.Services;

namespace My.Web.Api.Project.Controllers
{
    [ApiController]
    [Route("api/user")]
    public class UserController : ControllerBase
    {
        private IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// Login sayfası
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        [HttpPost("/login")]
        public IActionResult Login(string username, string password)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
                {
                    return BadRequest("Kullanıcı adı ve şifrenizi giriniz!");
                }

                string generatedToken = _userService.Authenticate(username,password);
                if (generatedToken == null)
                {
                    return NotFound("Kullanıcı adı veya şifre yanlış");
                }
                else
                {
                    return Ok(new { Username = username, Token = generatedToken});
                }
                
            }
            catch (System.Exception)
            {
                return BadRequest("Login sırasında bilinmeyen hata oluştu");
            }


        }

        /// <summary>
        /// Kullanıcı kayıt sayfası
        /// </summary>
        /// <param name="userDto"></param>
        /// <returns></returns>
        [HttpPost("/register")]
        public IActionResult Register([FromBody] registerUserDto userDto)
        {
            try
            {
                if (userDto == null)
                {
                    return BadRequest("Lütfen istenilen alanları doldurunuz");
                }

                if (string.IsNullOrWhiteSpace(userDto.Username) || string.IsNullOrWhiteSpace(userDto.Password))
                {
                    return BadRequest("Kullanıcı adı ve şifre zorunludur!");
                }

                User checkUser = _userService.CheckUser(userDto.Username);
                if (checkUser!=null)
                {
                    return BadRequest("Bu kullanıcı zaten mevcut");
                }
                else
                {
                    _userService.Register(userDto);
                    return Ok("Kayıt işlemi başarılı");
                }
            }
            catch (System.Exception)
            {
                throw;
            }

        }

    }
}