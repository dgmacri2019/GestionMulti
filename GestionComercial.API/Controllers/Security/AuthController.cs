using GestionComercial.Applications.Interfaces;
using GestionComercial.Domain.DTOs;
using GestionComercial.Domain.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GestionComercial.API.Controllers.Security
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;

        public AuthController(IUserService userService)
        {
            _userService = userService;
        }


        [HttpPost("LoginAsync")]
        [AllowAnonymous]
        public async Task<IActionResult> LoginAsync([FromBody] LoginRequestDto request)
        {
            LoginResponse resultLogin = await _userService.LoginAsync(request.UserName, request.Password);
            if (!resultLogin.Success)
                return BadRequest(resultLogin.Message);
            if (resultLogin.Success && resultLogin.Token == null)
                return Unauthorized(new { message = "Usuario o contraseña inválidos" });

            return Ok(new { resultLogin.Token });
        }
    }
}