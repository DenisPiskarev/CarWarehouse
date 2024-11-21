using CarWarehouse.BLL.Enums;
using CarWarehouse.BLL.Interfaces;
using CarWarehouse.DAL;
using CarWarehouse.DAL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CarWarehouse.BLL.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly UserManager<User> _userManager;
        private readonly CarWarehouseContext _carWarehouseContext;

        public UserRepository(UserManager<User> userManager, CarWarehouseContext carWarehouseContext)
        {
            _userManager = userManager;
            _carWarehouseContext = carWarehouseContext;
        }

        public async Task<User> GetByIdAsync(int id)
        {
            return await _userManager.Users.FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await Task.Run(() => _userManager.Users.ToList());
        }

        public async Task<IdentityResult> CreateAsync(User user, string password)
        {

            await _userManager.CreateAsync(user, password);

            var findUser = await _carWarehouseContext.Users.FirstOrDefaultAsync(x => x.Email == user.Email);

            return await _userManager.AddToRoleAsync(findUser, Roles.User);
        }

        public async Task<IdentityResult> UpdateAsync(User user)
        {
            return await _userManager.UpdateAsync(user);
        }

        public async Task<IdentityResult> DeleteAsync(User user)
        {
            return await _userManager.DeleteAsync(user);
        }

        public async Task<IdentityResult> ChangeUserRoleAsync(int userId, string newRole)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == userId);
            var addRoleResult = await _userManager.AddToRoleAsync(user, newRole);
        
            return addRoleResult;
        }
    }
}
