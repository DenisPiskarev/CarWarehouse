using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Security.Cryptography;
using CarWarehouse.BLL.Interfaces;
using CarWarehouse.BLL.ViewModel;
using CarWarehouse.DAL;
using CarWarehouse.DAL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace CarWarehouse.BLL.Services
{
    public class AuthService : IAuthService
    {
        private readonly AuthSettings _settings;
        private readonly UserManager<User> _userManager;
        private readonly CarWarehouseContext _context;

        public AuthService(IOptions<AuthSettings> settings, UserManager<User> userManager, CarWarehouseContext context)
        {
            _userManager = userManager;
            _settings = settings.Value;
            _context = context;
        }

        public string CreateToken(string username)
        {

            List<Claim> claims = new()
            {                    
                new Claim("username", Convert.ToString(username)),
            };

            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_settings.Key));
            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddHours(2),
                signingCredentials: cred
            );

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }

        public async Task<string> GenerateRefreshTokenAsync(int userId)
        {
            var randomBytes = new byte[64];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomBytes);
            }

            var refreshToken = new RefreshToken
            {
                Token = Convert.ToBase64String(randomBytes),
                Expires = DateTime.UtcNow.AddDays(7),
                Created = DateTime.UtcNow,
                UserId = userId
            };

            // Сохраняем в базе данных
            _context.RefreshTokens.Add(refreshToken);
            await _context.SaveChangesAsync();

            return refreshToken.Token;
        }

        public async Task<LoginResponse> AuthenticateAsync(string email, string password)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null || !await _userManager.CheckPasswordAsync(user, password))
            {
                return new LoginResponse
                {
                    IsSuccess = false,
                    Message = "Invalid credentials"
                };
            }

            var accessToken = CreateToken(user.UserName);
            var refreshToken = await GenerateRefreshTokenAsync(user.Id);

            return new LoginResponse
            {
                IsSuccess = true,
                Token = accessToken,
                RefreshToken = refreshToken,
                Message = "Login successful"
            };
        }

        public async Task<IdentityResult> RegisterAsync(RegisterRequest request)
        {
            var user = new User
            {
                Email = request.Email,
                UserName = request.Email
            };

            return await _userManager.CreateAsync(user, request.Password);
        }
    }
}
