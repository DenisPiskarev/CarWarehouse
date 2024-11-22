using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarWarehouse.DAL.Models;

namespace CarWarehouse.BLL.Interfaces
{
    public interface ITokenRepository
    {
        Task<RefreshToken?> GetUserByTokenAsync(string token);
    }
}
