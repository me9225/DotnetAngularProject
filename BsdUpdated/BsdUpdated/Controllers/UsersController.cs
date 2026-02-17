using BsdUpdated.DTOs;
using BsdUpdated.Models;
using BsdUpdated.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Cors;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using BsdUpdated.IServices;
using BsdUpdated.Data;

namespace BsdUpdated.Controllers
{
    [ApiController]
    [AllowAnonymous]
    //[EnableCors("AllowSpecificOrigin")]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly SaleContext _context;
        private readonly IUserService _service;
        private readonly ILogger<UsersController>  _logger;

        public UsersController(SaleContext context, IUserService service, ILogger<UsersController> logger)
        {
            _context = context;
            _service = service;
            _logger = logger;
        }

        [HttpPost("register")]
        public async Task<IActionResult> UserRegister([FromBody] CreateUserDto dto)
        {
            var (success, token, error) = await _service.UserRegister(dto);
            if (!success) { 
                _logger.LogWarning("User registration failed: {Error}", error);
                return BadRequest(new { error }); 
            }

            return Created(string.Empty, new { token });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var (success, token, error) = await _service.LoginAsync(dto);
            if (!success)
            {
                _logger.LogWarning("User login failed: {Error}", error);
                return BadRequest(new { error });
            }

            return Ok(new { token });
        }
    }
}