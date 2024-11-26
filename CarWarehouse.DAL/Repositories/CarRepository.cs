using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarWarehouse.DAL.Interfaces;
using CarWarehouse.DAL;
using CarWarehouse.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace CarWarehouse.DAL.Repositories
{
    public class CarRepository : ICarRepository
    {
        private readonly CarWarehouseContext _context;

        public CarRepository(CarWarehouseContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Car>> GetAllAsync(ClaimsPrincipal user)
        {
            if (user.IsInRole("Manager"))
            {
                return await _context.Cars.ToListAsync();
            }
            else if (user.IsInRole("User"))
            {
                return await _context.Set<Car>()
                    .Where(car => car.IsAvailable)
                    .ToListAsync();
            }

            throw new UnauthorizedAccessException("You do not have permission to view cars.");
        }
        
        public async Task<Car?> GetByIdAsync(int id)
        {
            return await _context.Cars.FindAsync(id);
        }

        public async Task<Car> AddAsync(Car car)
        {
            await _context.Cars.AddAsync(car);
            await _context.SaveChangesAsync();
            return car;
        }

        public async Task<bool> UpdateAsync(Car car)
        {
            var existingCar = await _context.Cars.FindAsync(car.Id);
            if (existingCar == null)
            {
                return false;
            }

            existingCar.Make = car.Make;
            existingCar.Model = car.Model;
            existingCar.Color = car.Color;

            _context.Cars.Update(existingCar);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var car = await _context.Cars.FindAsync(id);
            if (car == null)
            {
                return false;
            }

            _context.Cars.Remove(car);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task MarkAsUnavailableAsync(int carId)
        {
            var car = await GetByIdAsync(carId);
            if (car == null)
            {
                throw new KeyNotFoundException($"Car with ID {carId} not found.");
            }

            car.IsAvailable = false;
            await UpdateAsync(car);
        }
    }
}
