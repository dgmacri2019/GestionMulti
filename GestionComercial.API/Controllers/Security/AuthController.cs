using GestionComercial.Applications.Interfaces;
using GestionComercial.Domain.DTOs;
using GestionComercial.Domain.Entities.Masters;
using GestionComercial.Domain.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace GestionComercial.API.Controllers.Security
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly UserManager<User> _userManager;

        public AuthController(IAuthService authService, UserManager<User> userManager)
        {
            _authService = authService;
            _userManager = userManager;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
        {
            LoginResponse resultLogin = await _authService.Authenticate(request.UserName, request.Password);
            if (!resultLogin.Success)
                return BadRequest(resultLogin.Message);
            if (resultLogin.Success && resultLogin.Token == null)
                return Unauthorized(new { message = "Invalid username or password" });

            return Ok(new { resultLogin.Token });
        }

        [HttpPost("register")]
        [Authorize(Roles = "Developer, Administrator")] // Solo admins pueden gestionar roles
        public async Task<IActionResult> Register([FromBody] RegisterDto model)
        {
            User user = new User
            {
                UserName = model.UserName,
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
            };
            IdentityResult result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
                return BadRequest(result.Errors);

            await _userManager.AddToRoleAsync(user, model.Role);

            return Ok(new { message = "User created successfully!" });
        }
    }
}