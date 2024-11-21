using CarWarehouse.BLL.Enums;
using CarWarehouse.BLL.Interfaces;
using CarWarehouse.BLL.ViewModel;
using CarWarehouse.DAL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CarWarehouse.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly SignInManager<User> _signInManager;

        public UsersController(IUserRepository userRepository, UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userRepository = userRepository;
            _signInManager = signInManager;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null) 
                return NotFound();

            return Ok(user);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var users = await _userRepository.GetAllAsync();
            return Ok(users);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] RegisterRequest request)
        {
            var user = new User
            {
                Email = request.Email,
                UserName = request.Email,
                FullName = request.FullName
            };

            var result = await _userRepository.CreateAsync(user, request.Password);
  

            return CreatedAtAction(nameof(GetById), new { id = user.Id }, user);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] RegisterRequest request)
        {
            var existingUser = await _userRepository.GetByIdAsync(id);
            if (existingUser == null)
            {
                return NotFound(new { Message = $"User with ID {id} not found." });
            }

            existingUser.Email = request.Email;
            existingUser.UserName = request.Email;
            existingUser.FullName = request.FullName;

            var result = await _userRepository.UpdateAsync(existingUser);

            return NoContent();
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null) 
                return NotFound();

            var result = await _userRepository.DeleteAsync(user);

            return NoContent();
        }

    }
}
