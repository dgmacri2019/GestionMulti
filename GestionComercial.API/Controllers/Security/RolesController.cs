using GestionComercial.Applications.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GestionComercial.API.Controllers.Security
{
    [Route("api/roles")]
    [ApiController]
    [Authorize(Roles = "Developer, Administrator")] // Solo admins pueden gestionar roles
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
            if (!result) return BadRequest("Role already exists.");
            return Ok("Role created successfully.");
        }


        [HttpDelete("DeleteAsync/{roleId}")]
        public async Task<IActionResult> DeleteAsync(string roleId)
        {
            var result = await _roleService.DeleteAsync(roleId);
            if (!result) return NotFound("Role not found.");
            return Ok("Role deleted successfully.");
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