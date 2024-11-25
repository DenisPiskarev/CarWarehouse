using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace CarWarehouse.DAL.Models
{
    public class User : IdentityUser<int>
    {
        public string FullName { get; set; }
        public List<RefreshToken> RefreshTokens { get; set; }
    }
}
