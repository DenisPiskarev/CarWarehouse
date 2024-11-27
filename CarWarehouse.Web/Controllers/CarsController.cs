using CarWarehouse.DAL.Models;
using Microsoft.AspNetCore.Mvc;
using CarWarehouse.BLL.Enums;
using Microsoft.AspNetCore.Authorization;
using CarWarehouse.DAL.Interfaces;
using CarWarehouse.Web.ViewModels;
using AutoMapper;

namespace CarWarehouse.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class CarsController : ControllerBase
    {
        private readonly ICarRepository _carRepository;
        private readonly IMapper _mapper;

        public CarsController(ICarRepository carRepository, IMapper mapper)
        {
            _carRepository = carRepository;
            _mapper = mapper;
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

            var carViewModel = _mapper.Map<CarViewModel>(car);

            return Ok(carViewModel);
        }

        [Authorize(Roles = Roles.Manager)]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CarViewModel car)
        {
            var createdCar = await _carRepository.AddAsync(_mapper.Map<Car>(car));
            var createdCarViewModel = _mapper.Map<CarViewModel>(createdCar);
            return Ok(createdCarViewModel);
        }

        [Authorize(Roles = Roles.Manager)]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] CarViewModel carViewModel)
        {
            if (id == null)
                return BadRequest();

            var carToUpdate = _mapper.Map<Car>(carViewModel);

            var updated = await _carRepository.UpdateAsync(carToUpdate);
            if (!updated)
                return NotFound();

            var updatedCar = await _carRepository.GetByIdAsync(id);
            var updatedCarViewModel = _mapper.Map<CarViewModel>(updatedCar);

            return Ok(updatedCarViewModel);
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
