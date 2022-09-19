using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using My.Web.Api.Project.Data;
using My.Web.Api.Project.Helpers;
using My.Web.Api.Project.Models;

namespace My.Web.Api.Project.Services
{
    public interface IUserService
    {
        string Authenticate(string username, string password);
        User CheckUser(string username);
        void Register(registerUserDto userDto);
    }
    public class UserService : IUserService
    {
        private IServiceProvider _provider;
        private readonly NorthwindContext _context;
        private readonly IMapper _mapper;
        private readonly AppSettings _appSettings;
        public UserService(NorthwindContext context, IMapper mapper, IOptions<AppSettings> appSettings)
        {
            _context = context;
            _mapper = mapper;
            _appSettings = appSettings.Value;
        }

        public string Authenticate(string username, string password)
        {
            User user = _context.Users.Where(x => x.Username == username && x.Password == password).SingleOrDefault();
            if (user == null)
                return null;

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.SecretKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim("uname", user.Username),
                    new Claim("urole", user.Role),
                    new Claim("uid", user.Id.ToString())
                }),
                Expires = DateTime.UtcNow.AddMinutes(60),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            //_context.SaveChanges();
            string generatedToken = tokenHandler.WriteToken(token);

            return generatedToken;
        }

        public User CheckUser(string username){
            User user = _context.Users.Where(x => x.Username == username).SingleOrDefault();
            if (user==null)
            {
                return null;
            }
            else
            {
                return user;
            }
        }

        public void Register(registerUserDto userDto){
            User newUser = new User();
            newUser.FirstName = userDto.FirstName;
            newUser.LastName = userDto.LastName;
            newUser.Username = userDto.Username;
            newUser.Password = userDto.Password;
            newUser.Role = "user";
            _context.Users.Add(newUser);
            _context.SaveChanges();
        }

    }
}