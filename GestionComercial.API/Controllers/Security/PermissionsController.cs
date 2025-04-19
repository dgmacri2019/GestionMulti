using GestionComercial.Applications.Interfaces;
using GestionComercial.Domain.DTOs;
using GestionComercial.Domain.Entities.Masters.Security;
using GestionComercial.Domain.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GestionComercial.API.Controllers.Security
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Developer, Administrator")] // Solo admins pueden gestionar roles
    public class PermissionsController : ControllerBase
    {
        private readonly IPermissionService _permissionService;

        public PermissionsController(IPermissionService permissionService)
        {
            _permissionService = permissionService;
        }


        [HttpPost("Add")]
        public IActionResult Add([FromBody] Permission permission)
        {
            GeneralResponse resultAdd = _permissionService.Add(permission);
            return resultAdd.Success ?
                 Ok("Permiso creado correctamente")
                :
                BadRequest(resultAdd.Message);

        }

        [HttpPost("AddAsync")]
        public async Task<IActionResult> AddAsync([FromBody] Permission permission)
        {
            GeneralResponse resultAdd = await _permissionService.AddAsync(permission);
            return resultAdd.Success ?
                Ok("Permiso creado correctamente")
                :
                BadRequest(resultAdd.Message);
        }




        [HttpPost("AddRolePermission")]
        public IActionResult AddRolePermission([FromBody] RolePermission rolePermission)
        {
            GeneralResponse resultAdd = _permissionService.AddRolePermission(rolePermission);
            return resultAdd.Success ?
                 Ok("Permiso creado correctamente")
                :
                BadRequest(resultAdd.Message);
        }

        [HttpPost("AddRolePermissionAsync")]
        public async Task<IActionResult> AddRolePermissionAsync([FromBody] RolePermission rolePermission)
        {
            GeneralResponse resultAdd = await _permissionService.AddRolePermissionAsync(rolePermission);
            return resultAdd.Success ?
                 Ok("Permiso creado correctamente")
                :
                BadRequest(resultAdd.Message);
        }



        [HttpPost("AddUserPermission")]
        public IActionResult AddUserPermission([FromBody] UserPermission userPermission)
        {
            GeneralResponse resultAdd = _permissionService.AddUserPermission(userPermission);
            return resultAdd.Success ?
                 Ok("Permiso creado correctamente")
                :
                BadRequest(resultAdd.Message);
        }

        [HttpPost("AddUserPermissionAsync")]
        public async Task<IActionResult> AddUserPermissionAsync([FromBody] UserPermission userPermission)
        {
            GeneralResponse resultAdd = await _permissionService.AddUserPermissionAsync(userPermission);
            return resultAdd.Success ?
                 Ok("Permiso creado correctamente")
                :
                BadRequest(resultAdd.Message);
        }



        [HttpGet("Delete/{id}")]
        public IActionResult Delete(int id)
        {
            Permission permission = _permissionService.GetById(id);
            if (permission != null)
            {
                GeneralResponse resultDelete = _permissionService.Delete(permission);
                if (resultDelete.Success)
                    return Ok(new { Message = $"Permiso {id} eliminado correctamente." });
                else
                    BadRequest(resultDelete.Message);
            }
            return NotFound();
        }

        [HttpGet("DeleteAsync/{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            Permission permission = await _permissionService.GetByIdAsync(id);
            if (permission != null)
            {
                GeneralResponse resultDelete = await _permissionService.DeleteAsync(permission);
                if (resultDelete.Success)
                    return Ok(new { Message = $"Permiso {id} eliminado correctamente." });
                else
                    BadRequest(resultDelete.Message);
            }
            return NotFound();
        }



        [HttpGet("DeleteRolePermission/{id}")]
        public IActionResult DeleteRolePermission(int id)
        {
            RolePermission rolePermission = _permissionService.GetRolePermissionById(id);
            if (rolePermission != null)
            {
                GeneralResponse resultDelete = _permissionService.DeleteRolePermission(rolePermission);
                if (resultDelete.Success)
                    return Ok(new { Message = $"Permiso {id} eliminado correctamente." });
                else
                    BadRequest(resultDelete.Message);
            }
            return NotFound();
        }

        [HttpGet("DeleteRolePermissionAsync/{id}")]
        public async Task<IActionResult> DeleteRolePermissionAsync(int id)
        {
            RolePermission rolePermission = await _permissionService.GetRolePermissionByIdAsync(id);
            if (rolePermission != null)
            {
                GeneralResponse resultDelete = await _permissionService.DeleteRolePermissionAsync(rolePermission);
                if (resultDelete.Success)
                    return Ok(new { Message = $"Permiso {id} eliminado correctamente." });
                else
                    BadRequest(resultDelete.Message);
            }
            return NotFound();
        }


        [HttpGet("DeleteUserPermission/{id}")]
        public IActionResult DeleteUserPermission(int id)
        {
            UserPermission userPermission = _permissionService.GetUserPermissionById(id);
            if (userPermission != null)
            {
                GeneralResponse resultDelete = _permissionService.DeleteUserPermission(userPermission);
                if (resultDelete.Success)
                    return Ok(new { Message = $"Permiso {id} eliminado correctamente." });
                else
                    BadRequest(resultDelete.Message);
            }
            return NotFound();
        }

        [HttpGet("DeleteUserPermissionAsync/{id}")]
        public async Task<IActionResult> DeleteUserPermissionAsync(int id)
        {
            UserPermission userPermission = await _permissionService.GetUserPermissionByIdAsync(id);
            if (userPermission != null)
            {
                GeneralResponse resultDelete = await _permissionService.DeleteUserPermissionAsync(userPermission);
                if (resultDelete.Success)
                    return Ok(new { Message = $"Permiso {id} eliminado correctamente." });
                else
                    BadRequest(resultDelete.Message);
            }
            return NotFound();
        }



        [HttpGet("GetAll")]
        public IActionResult GetAll()
        {
            IEnumerable<Permission> permissions = _permissionService.GetAll();
            return Ok(permissions);
        }

        [HttpGet("GetAllAsync")]
        public async Task<IActionResult> GetAllAsync()
        {
            IEnumerable<Permission> permissions = await _permissionService.GetAllAsync();
            return Ok(permissions);
        }



        [HttpGet("GetAllRolePermision")]
        public IActionResult GetAllRolePermision()
        {
            IEnumerable<RolePermission> rolePpermissions = _permissionService.GetAllRolePermision();
            return Ok(rolePpermissions);
        }

        [HttpGet("GetAllRolePermisionAsync")]
        public async Task<IActionResult> GetAllRolePermisionAsync()
        {
            IEnumerable<RolePermission> rolePermissions = await _permissionService.GetAllRolePermisionAsync();
            return Ok(rolePermissions);
        }



        [HttpGet("GetAllUserPermision")]
        public IActionResult GetAllUserPermision()
        {
            IEnumerable<UserPermission> userPermissions = _permissionService.GetAllUserPermision();
            return Ok(userPermissions);
        }

        [HttpGet("GetAllUserPermisionAsync")]
        public async Task<IActionResult> GetAllAsyncUserPermision()
        {
            IEnumerable<UserPermission> userPermissions = await _permissionService.GetAllUserPermisionAsync();
            return Ok(userPermissions);
        }



        [HttpGet("GetById/{id}")]
        public IActionResult GetById(int id)
        {
            Permission permission = _permissionService.GetById(id);
            if (permission == null)
                return NotFound();

            return Ok(permission);
        }

        [HttpGet("GetByIdAsync/{id}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            Permission permission = await _permissionService.GetByIdAsync(id);
            if (permission == null)
                return NotFound();

            return Ok(permission);
        }



        [HttpGet("GetRolePermissionById/{id}")]
        public IActionResult GetRolePermissionById(int id)
        {
            RolePermission rolePermission = _permissionService.GetRolePermissionById(id);
            if (rolePermission == null)
                return NotFound();

            return Ok(rolePermission);
        }

        [HttpGet("GetRolePermissionByIdAsync/{id}")]
        public async Task<IActionResult> GetRolePermissionByIdAsync(int id)
        {
            RolePermission rolePermission = await _permissionService.GetRolePermissionByIdAsync(id);
            if (rolePermission == null)
                return NotFound();

            return Ok(rolePermission);
        }



        [HttpGet("GetUserPermissionById/{id}")]
        public IActionResult GetUserPermissionById(int id)
        {
            UserPermission userPermission = _permissionService.GetUserPermissionById(id);
            if (userPermission == null)
                return NotFound();

            return Ok(userPermission);
        }

        [HttpGet("GetUserPermissionByIdAsync/{id}")]
        public async Task<IActionResult> GetUserPermissionByIdAsync(int id)
        {
            UserPermission userPermission = await _permissionService.GetUserPermissionByIdAsync(id);
            if (userPermission == null)
                return NotFound();

            return Ok(userPermission);
        }



        [HttpPost("Update")]
        public IActionResult Update([FromBody] UpdateGeneralModelDto model)
        {
            Permission permission = _permissionService.GetById(model.Id);
            if (permission != null)
            {
                permission.IsEnabled = model.IsEnabled;
                permission.IsDeleted = model.IsDeleted;
                permission.UpdateDate = DateTime.Now;
                permission.UpdateUser = model.UserName;

                GeneralResponse resultDelete = _permissionService.Update(permission);
                if (resultDelete.Success)
                    return Ok(new { Message = $"Permiso '{permission.Name}' actualizado correctamente." });
                else
                    BadRequest(resultDelete.Message);
            }
            return NotFound();
        }

        [HttpPost("UpdateAsync")]
        public async Task<IActionResult> UpdateAsync([FromBody] UpdateGeneralModelDto model)
        {
            Permission permission = await _permissionService.GetByIdAsync(model.Id);
            if (permission != null)
            {
                permission.IsEnabled = model.IsEnabled;
                permission.IsDeleted = model.IsDeleted;
                permission.UpdateDate = DateTime.Now;
                permission.UpdateUser = model.UserName;

                GeneralResponse resultDelete = await _permissionService.UpdateAsync(permission);
                if (resultDelete.Success)
                    return Ok(new { Message = $"Permiso '{permission.Name}' actualizado correctamente." });
                else
                    BadRequest(resultDelete.Message);
            }
            return NotFound();
        }



        [HttpPost("UpdateRolePermission")]
        public IActionResult UpdateRolePermission([FromBody] UpdateGeneralModelDto model)
        {
            RolePermission rolePermission = _permissionService.GetRolePermissionById(model.Id);
            if (rolePermission != null)
            {
                rolePermission.IsEnabled = model.IsEnabled;
                rolePermission.IsDeleted = model.IsDeleted;
                rolePermission.UpdateDate = DateTime.Now;
                rolePermission.UpdateUser = model.UserName;

                GeneralResponse resultDelete = _permissionService.UpdateRolePermission(rolePermission);
                if (resultDelete.Success)
                    return Ok(new { Message = $"Permiso actualizado correctamente." });
                else
                    BadRequest(resultDelete.Message);
            }
            return NotFound();
        }

        [HttpPost("UpdateRolePermissionAsync")]
        public async Task<IActionResult> UpdateRolePermissionAsync([FromBody] UpdateGeneralModelDto model)
        {
            RolePermission rolePermission = await _permissionService.GetRolePermissionByIdAsync(model.Id);
            if (rolePermission != null)
            {
                rolePermission.IsEnabled = model.IsEnabled;
                rolePermission.IsDeleted = model.IsDeleted;
                rolePermission.UpdateDate = DateTime.Now;
                rolePermission.UpdateUser = model.UserName;

                GeneralResponse resultDelete = await _permissionService.UpdateRolePermissionAsync(rolePermission);
                if (resultDelete.Success)
                    return Ok(new { Message = $"Permiso actualizado correctamente." });
                else
                    BadRequest(resultDelete.Message);
            }
            return NotFound();
        }



        [HttpPost("UpdateUserPermission")]
        public IActionResult UpdateUserPermission([FromBody] UpdateGeneralModelDto model)
        {
            UserPermission userPermission = _permissionService.GetUserPermissionById(model.Id);
            if (userPermission != null)
            {
                userPermission.IsEnabled = model.IsEnabled;
                userPermission.IsDeleted = model.IsDeleted;
                userPermission.UpdateDate = DateTime.Now;
                userPermission.UpdateUser = model.UserName;

                GeneralResponse resultDelete = _permissionService.UpdateUserPermission(userPermission);
                if (resultDelete.Success)
                    return Ok(new { Message = $"Permiso  actualizado correctamente." });
                else
                    BadRequest(resultDelete.Message);
            }
            return NotFound();
        }

        [HttpPost("UpdateUserPermissionAsync")]
        public async Task<IActionResult> UpdateUserPermissionAsync([FromBody] UpdateGeneralModelDto model)
        {
            UserPermission userPermission = await _permissionService.GetUserPermissionByIdAsync(model.Id);
            if (userPermission != null)
            {
                userPermission.IsEnabled = model.IsEnabled;
                userPermission.IsDeleted = model.IsDeleted;
                userPermission.UpdateDate = DateTime.Now;
                userPermission.UpdateUser = model.UserName;

                GeneralResponse resultDelete = await _permissionService.UpdateUserPermissionAsync(userPermission);
                if (resultDelete.Success)
                    return Ok(new { Message = $"Permiso actualizado correctamente." });
                else
                    BadRequest(resultDelete.Message);
            }
            return NotFound();
        }

    }
}
