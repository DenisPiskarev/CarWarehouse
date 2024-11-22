using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarWarehouse.BLL.ViewModel;
using Microsoft.AspNetCore.Identity;

namespace CarWarehouse.BLL.Interfaces
{
    public interface IAuthService
    {
        Task<LoginResponse> AuthenticateAsync(string email, string password);
        Task<IdentityResult> RegisterAsync(RegisterRequest request);
    }
}
