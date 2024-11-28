using CarWarehouse.BLL.DTO;
using CarWarehouse.DAL.Models;

namespace CarWarehouse.BLL.Interfaces
{
    public interface IAuthService
    {
        Task<AuthenticateResponse> AuthenticateAsync(AuthenticateRequest model, string ipAddress);
        Task<AuthenticateResponse> RefreshTokenAsync(string token, string ipAddress);
        Task<bool> RevokeTokenAsync(string token, string ipAddress);
        Task<User> GetUserById(int id);
        public Task Initializer();
    }
}
