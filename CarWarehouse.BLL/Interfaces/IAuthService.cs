using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarWarehouse.BLL.Interfaces
{
    public interface IAuthService
    {
        string CreateToken(string username);
    }
}
