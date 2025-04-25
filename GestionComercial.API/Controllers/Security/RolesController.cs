using GestionComercial.Applications.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GestionComercial.API.Controllers.Security
{
    [Route("api/roles")]
    [ApiController]
    [Authorize(Roles = "Developer, Administrator")] // Solo Developers y Admins pueden gestionar roles
    public class RolesController : ControllerBase
    {
        private readonly IRoleService _roleService;

        public RolesController(IRoleService roleService)
        {
            _roleService = roleService;
        }


        [HttpPost("AddAsync")]
        public async Task<IActionResult> AddAsync([FromBody] string roleName)
        {
            var result = await _roleService.AddAsync(roleName);
            if (!result) return BadRequest("El Rol ya existe");
            return Ok("Rol creado correctamente");
        }


        [HttpPost("DeleteAsync")]
        public async Task<IActionResult> DeleteAsync([FromBody] string roleId)
        {
            var result = await _roleService.DeleteAsync(roleId);
            if (!result) return NotFound("El Rol no existe");
            return Ok("Rol borrado correctamente");
        }


        [HttpGet("GetAll")]
        public IActionResult GetAll()
        {
            return Ok(_roleService.GetAll());
        }

        
        [HttpGet("GetAllAsync")]
        public async Task<IActionResult> GetAllAsync()
        {
            return Ok(await _roleService.GetAllAsync());
        }
    }
}