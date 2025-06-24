using API.DTOs;
using API.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;

        public AuthController(UserManager<ApplicationUser> userManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _configuration = configuration;
        }

        [HttpPost("signup")]
        public async Task<IActionResult> SignUp([FromBody] UserRegistrationDto registrationDto)
        {
            var user = new ApplicationUser
            {
                UserName = registrationDto.Username,
                Email = registrationDto.Email
            };

            var result = await _userManager.CreateAsync(user, registrationDto.Password);

            if (result.Succeeded)
            {
                return Ok(new { success = true });
            }

            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            return BadRequest(new { success = false, message = errors });
        }
    }
}