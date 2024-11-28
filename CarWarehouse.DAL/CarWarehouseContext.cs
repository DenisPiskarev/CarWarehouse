using CarWarehouse.DAL.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CarWarehouse.DAL
{
    public class CarWarehouseContext : IdentityDbContext<User, Role, int>
    {
        public CarWarehouseContext(DbContextOptions<CarWarehouseContext> options)
    : base(options)
        {

        }
        public DbSet<Car> Cars { get; set; }
    }
}
