using CarWarehouse.DAL.Models;
using Microsoft.AspNetCore.Mvc;
using CarWarehouse.BLL.Enums;
using Microsoft.AspNetCore.Authorization;
using CarWarehouse.DAL.Interfaces;

namespace CarWarehouse.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class CarsController : ControllerBase
    {
        private readonly ICarRepository _carRepository;

        public CarsController(ICarRepository carRepository)
        {
            _carRepository = carRepository;
        }

        [Authorize(Roles = Roles.Manager + "," + Roles.User)]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var cars = await _carRepository.GetAllAsync(User);
            return Ok(cars);
        }

        [Authorize(Roles = Roles.Manager + "," + Roles.User)]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var car = await _carRepository.GetByIdAsync(id);
            if (car == null)
                return NotFound();

            return Ok(car);
        }

        [Authorize(Roles = Roles.Manager)]
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] Car car)
        {
            Car addCar = new Car()
            {
                Make = car.Make,
                Model = car.Model,
                Color = car.Color
            };
            var createdCar = await _carRepository.AddAsync(car);
            return CreatedAtAction(nameof(GetById), new { id = createdCar.Id }, createdCar);
        }

        [Authorize(Roles = Roles.Manager)]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Car car)
        {
            if (id != car.Id)
                return BadRequest();

            var updated = await _carRepository.UpdateAsync(car);
            if (!updated)
                return NotFound();

            return NoContent();
        }

        [Authorize(Roles = Roles.Manager)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _carRepository.DeleteAsync(id);
            if (!deleted)
                return NotFound();

            return NoContent();
        }
    }
}
