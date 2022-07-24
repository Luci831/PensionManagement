using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using AuthenticationMicroservice.Models;
using System.Net.Http;
using System.Net.Http.Json;
using Newtonsoft.Json;

namespace AuthenticationMicroservice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private IConfiguration _config;

        public TokenController(IConfiguration config)
        {
            _config = config;
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] User login)
        {
            IActionResult response = Unauthorized();
            User user =await AuthenticateUser(login);

            if (user != null)
            {
                Token t=new Token();
                t.token = GenerateJSONWebToken(user);
                t.Id = user.UserId;
                response = Ok(t);
            }

            return response;
        }

        private string GenerateJSONWebToken(User userInfo)
        {

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
              _config["Jwt:Issuer"],
              null,
              expires: DateTime.Now.AddMinutes(30),
              signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private async Task<User> AuthenticateUser(User login)
        {
            if (login.Email == "admin@pensionerdoorway.com" && login.Password == "admin" && login.CatId == 1)
                return login;
            else
            {
                User user = null;
                List<User> loginlist = new List<User>();
                HttpClient client = new HttpClient();
                HttpResponseMessage response = await client.GetAsync("http://localhost:46612/api/Users");
                if (response.IsSuccessStatusCode)
                {
                    var results = response.Content.ReadAsStringAsync().Result;
                    loginlist = JsonConvert.DeserializeObject<List<User>>(results);
                }
                //Validate the User Credentials    
                user = loginlist.FirstOrDefault(x => x.Email == login.Email && x.Password == login.Password && x.CatId == login.CatId);
                return user;
            }
        }
    }
}
