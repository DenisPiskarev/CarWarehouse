using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarWarehouse.BLL.Interfaces;
using CarWarehouse.DAL;
using CarWarehouse.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace CarWarehouse.BLL.Repositories
{
    public class TokenRepository : ITokenRepository
    {
        private readonly CarWarehouseContext _context;
        public TokenRepository(CarWarehouseContext context) 
        {
            _context = context;
        }

        public async Task<RefreshToken?> GetUserByTokenAsync(string token)
        {
            return await _context.RefreshTokens
                .Include(rt => rt.User)
                .FirstOrDefaultAsync(rt => rt.Token == token);

        }
    }
}
