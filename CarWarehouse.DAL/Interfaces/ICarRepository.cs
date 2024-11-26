using System.Security.Claims;
using CarWarehouse.DAL.Models;

namespace CarWarehouse.DAL.Interfaces
{
    public interface ICarRepository
    {
        Task<IEnumerable<Car>> GetAllAsync(ClaimsPrincipal user);
        Task<Car?> GetByIdAsync(int id);
        Task<Car> AddAsync(Car car);
        Task<bool> UpdateAsync(Car car);
        Task<bool> DeleteAsync(int id);
        Task MarkAsUnavailableAsync(int id);

    }
}
