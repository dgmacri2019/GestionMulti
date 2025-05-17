using GestionComercial.API.Security;
using GestionComercial.Applications.Interfaces;
using GestionComercial.Domain.DTOs.Security;
using GestionComercial.Domain.Entities.Masters.Security;
using GestionComercial.Domain.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GestionComercial.API.Controllers.Security
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    [AuthorizePermission]
    public class PermissionsController : ControllerBase
    {
        private readonly IPermissionService _permissionService;
        private readonly IMasterService _masterService;


        public PermissionsController(IPermissionService permissionService, IMasterService masterService)
        {
            _permissionService = permissionService;
            _masterService = masterService;
        }


        [HttpPost("AddPermissionAsync")]
        public async Task<IActionResult> AddPermissionAsync([FromBody] Permission permission)
        {
            GeneralResponse resultAdd = await _masterService.AddAsync(permission);
            return resultAdd.Success ?
                Ok("Permiso creado correctamente")
                :
                BadRequest(resultAdd.Message);
        }

        [HttpPost("AddRolePermissionAsync")]
        public async Task<IActionResult> AddRolePermissionAsync([FromBody] RolePermission rolePermission)
        {
            GeneralResponse resultAdd = await _masterService.AddAsync(rolePermission);
            return resultAdd.Success ?
                 Ok("Permiso creado correctamente")
                :
                BadRequest(resultAdd.Message);
        }

        [HttpPost("AddUserPermissionAsync")]
        public async Task<IActionResult> AddUserPermissionAsync([FromBody] UserPermission userPermission)
        {
            GeneralResponse resultAdd = await _masterService.AddAsync(userPermission);
            return resultAdd.Success ?
                 Ok("Permiso creado correctamente")
                :
                BadRequest(resultAdd.Message);
        }



        [HttpPost("DeleteAsync")]
        public async Task<IActionResult> DeleteAsync([FromBody] SecurityFilterDto filter)
        {
            Permission permission = await _permissionService.GetByIdAsync(filter.Id);
            if (permission != null)
            {
                GeneralResponse resultDelete = await _masterService.DeleteAsync(permission);
                if (resultDelete.Success)
                    return Ok(new { Message = $"Permiso {filter.Id} eliminado correctamente." });
                else
                    BadRequest(resultDelete.Message);
            }
            return NotFound();
        }


        [HttpPost("DeleteRolePermissionAsync")]
        public async Task<IActionResult> DeleteRolePermissionAsync([FromBody] SecurityFilterDto filter)
        {
            RolePermission rolePermission = await _permissionService.GetRolePermissionByIdAsync(filter.Id);
            if (rolePermission != null)
            {
                GeneralResponse resultDelete = await _masterService.DeleteAsync(rolePermission);
                if (resultDelete.Success)
                    return Ok(new { Message = $"Permiso {filter.Id} eliminado correctamente." });
                else
                    BadRequest(resultDelete.Message);
            }
            return NotFound();
        }


        [HttpPost("DeleteUserPermissionAsync")]
        public async Task<IActionResult> DeleteUserPermissionAsync([FromBody] SecurityFilterDto filter)
        {
            UserPermission userPermission = await _permissionService.GetUserPermissionByIdAsync(filter.Id);
            if (userPermission != null)
            {
                GeneralResponse resultDelete = await _masterService.DeleteAsync(userPermission);
                if (resultDelete.Success)
                    return Ok(new { Message = $"Permiso {filter.Id} eliminado correctamente." });
                else
                    BadRequest(resultDelete.Message);
            }
            return NotFound();
        }



        [HttpPost("GetAllPermissionAsync")]
        public async Task<IActionResult> GetAllPermissionAsync([FromBody] SecurityFilterDto filter)
        {
            IEnumerable<Permission> permissions = await _permissionService.GetAllAsync(filter.IsEnabled, filter.IsDeleted);
            return Ok(permissions);
        }


        [HttpPost("GetAllRolePermisionAsync")]
        public async Task<IActionResult> GetAllRolePermisionAsync([FromBody] SecurityFilterDto filter)
        {
            IEnumerable<RolePermission> rolePermissions = await _permissionService.GetAllRolePermisionAsync(filter.IsEnabled, filter.IsDeleted);
            return Ok(rolePermissions);
        }


        [HttpPost("GetAllUserPermisionAsync")]
        public async Task<IActionResult> GetAllAsyncUserPermisionAsync([FromBody] SecurityFilterDto filter)
        {
            IEnumerable<UserPermission> userPermissions = await _permissionService.GetAllUserPermisionAsync(filter.IsEnabled, filter.IsDeleted);
            return Ok(userPermissions);
        }



        [HttpPost("GetPermissionByIdAsync")]
        public async Task<IActionResult> GetPermissionByIdAsync([FromBody] SecurityFilterDto filter)
        {
            Permission permission = await _permissionService.GetByIdAsync(filter.Id);
            if (permission == null)
                return NotFound();

            return Ok(permission);
        }

        [HttpPost("GetRolePermissionByIdAsync")]
        public async Task<IActionResult> GetRolePermissionByIdAsync([FromBody] SecurityFilterDto filter)
        {
            RolePermission rolePermission = await _permissionService.GetRolePermissionByIdAsync(filter.Id);
            if (rolePermission == null)
                return NotFound();

            return Ok(rolePermission);
        }

        [HttpPost("GetUserPermissionByIdAsync")]
        public async Task<IActionResult> GetUserPermissionByIdAsync([FromBody] SecurityFilterDto filter)
        {
            UserPermission userPermission = await _permissionService.GetUserPermissionByIdAsync(filter.Id);
            if (userPermission == null)
                return NotFound();

            return Ok(userPermission);
        }



        [HttpPost("UpdatePermissionAsync")]
        public async Task<IActionResult> UpdatePermissionAsync([FromBody] Permission model)
        {
            GeneralResponse resultAdd = await _masterService.UpdateAsync(model);
            return resultAdd.Success ?
                Ok("Permiso actualizado correctamente")
                :
                BadRequest(resultAdd.Message);
        }



        [HttpPost("UpdateRolePermissionAsync")]
        public async Task<IActionResult> UpdateRolePermissionAsync([FromBody] RolePermission model)
        {
            GeneralResponse resultAdd = await _masterService.UpdateAsync(model);
            return resultAdd.Success ?
                Ok("Permiso actualizado correctamente")
                :
                BadRequest(resultAdd.Message);
        }


        [HttpPost("UpdateUserPermissionAsync")]
        public async Task<IActionResult> UpdateUserPermissionAsync([FromBody] UserPermission model)
        {
            GeneralResponse resultAdd = await _masterService.UpdateAsync(model);
            return resultAdd.Success ?
                Ok("Permiso actualizado correctamente")
                :
                BadRequest(resultAdd.Message);
        }

    }
}
