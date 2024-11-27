using CarWarehouse.BLL.Enums;
using CarWarehouse.BLL.DTO;
using CarWarehouse.DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using CarWarehouse.Web.ViewModels;
using Microsoft.EntityFrameworkCore;
using AutoMapper;

namespace CarWarehouse.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = Roles.Administrator)]
    public class UsersController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;

        public UsersController(UserManager<User> userManager, IMapper mapper)
        {
            _userManager = userManager;
            _mapper = mapper;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == id);
            if (user == null)
            {
                return NotFound(new { Message = $"User with ID {id} not found." });
            }

            var userViewModel = _mapper.Map<UserViewModel>(user);
            return Ok(userViewModel);
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var users = _userManager.Users.ToList();

            var userViewModels = users.Select(user => _mapper.Map<UserViewModel>(user));
            return Ok(userViewModels);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] UserViewModel request)
        {
            var user = _mapper.Map<User>(request);

            var result = await _userManager.CreateAsync(user, request.Password);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            await _userManager.AddToRoleAsync(user, Roles.User);
            var createdUserViewModel = _mapper.Map<UserViewModel>(user);

            return Ok(createdUserViewModel);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UserViewModel request)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == id);
            if (user == null)
            {
                return NotFound(new { Message = $"User with ID {id} not found." });
            }

            user.Email = request.Email;
            user.UserName = request.Email;
            user.FullName = request.FullName;

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == id);
            if (user == null)
            {
                return NotFound(new { Message = $"User with ID {id} not found." });
            }

            var result = await _userManager.DeleteAsync(user);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            return NoContent();
        }

        [HttpPost("edit-role")]
        public async Task<IActionResult> EditRole([FromBody] EditRoleViewModel request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.Role))
            {
                return BadRequest(new { message = "Invalid request data" });
            }

            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == request.Id);

            if (user == null)
            {
                return NotFound(new { message = $"User with ID {request.Id} not found" });
            }

            var result = await _userManager.AddToRoleAsync(user, request.Role);

            if (!result.Succeeded)
            {
                return BadRequest(new { message = "Failed to add role", errors = result.Errors });
            }

            var userViewModel = _mapper.Map<UserViewModel>(user);

            return Ok(new { message = $"Role {request.Role} successfully added to user {request.Id}" });
        }
    }
}
