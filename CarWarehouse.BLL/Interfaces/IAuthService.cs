using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using CarWarehouse.BLL.ViewModel;
using CarWarehouse.DAL.Models;
using Microsoft.AspNetCore.Identity;

namespace CarWarehouse.BLL.Interfaces
{
    public interface IAuthService
    {
        Task<AuthenticateResponse> AuthenticateAsync(AuthenticateRequest model, string ipAddress);
        Task<AuthenticateResponse> RefreshTokenAsync(string token, string ipAddress);
        Task<bool> RevokeTokenAsync(string token, string ipAddress);
        Task<User> GetUserById(int id);
    }
}
