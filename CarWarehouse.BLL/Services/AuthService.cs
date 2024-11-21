using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using CarWarehouse.BLL.Interfaces;
using CarWarehouse.BLL.ViewModel;
using CarWarehouse.DAL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace CarWarehouse.BLL.Services
{
    public class AuthService : IAuthService
    {
        private readonly IConfiguration _configuration;
        private readonly UserManager<User> _userManager;

        public AuthService(IUserRepository userRepository, IConfiguration configuration, UserManager<User> userManager)
        {
            _userManager = userManager;
            _configuration = configuration;
        }

        public string CreateToken(string username)
        {

            List<Claim> claims = new()
            {                    
                new Claim("username", Convert.ToString(username)),
            };

            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:Token").Value));
            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddHours(2),
                signingCredentials: cred
            );

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }

        public async Task<LoginResponse> AuthenticateAsync(string email, string username, string password)
        {
            var loginResponse = new LoginResponse { };
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
            {
                return null;
            }

            string token = CreateToken(user.UserName);

            loginResponse.Token = token;
            loginResponse.responseMsg = new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.OK
            };
            
            return loginResponse;
        }
    }
}
