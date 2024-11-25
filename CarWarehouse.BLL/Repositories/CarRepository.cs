﻿using System;
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
    public class CarRepository : ICarRepository
    {
        private readonly CarWarehouseContext _context;

        public CarRepository(CarWarehouseContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Car>> GetAllAsync()
        {
            return await _context.Cars.ToListAsync();
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
    }
}
