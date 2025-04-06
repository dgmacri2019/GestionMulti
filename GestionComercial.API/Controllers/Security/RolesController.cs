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


        [HttpPost("create")]
        public async Task<IActionResult> CreateRole([FromBody] string roleName)
        {
            var result = await _roleService.CreateRoleAsync(roleName);
            if (!result) return BadRequest("Role already exists.");
            return Ok("Role created successfully.");
        }


        [HttpDelete("delete/{roleId}")]
        public async Task<IActionResult> DeleteRole(string roleId)
        {
            var result = await _roleService.DeleteRoleAsync(roleId);
            if (!result) return NotFound("Role not found.");
            return Ok("Role deleted successfully.");
        }


        [HttpGet("list")]
        public IActionResult GetRoles()
        {
            return Ok(_roleService.GetRoles());
        }
    }
}