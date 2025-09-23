using GestionComercial.API.Security;
using GestionComercial.Applications.Interfaces;
using GestionComercial.Applications.Notifications;
using GestionComercial.Domain.DTOs.User;
using GestionComercial.Domain.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.Eventing.Reader;
using static GestionComercial.Domain.Constant.Enumeration;

namespace GestionComercial.API.Controllers.Security
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    [AuthorizePermission]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IUsersNotifier _notifier;

        public UsersController(IUserService userService, IUsersNotifier notifier)
        {
            _userService = userService;
            _notifier = notifier;
        }




        [HttpPost("AddAsync")]
        public async Task<IActionResult> AddAsync(UserViewModel model)
        {
            IdentityResult resultAdd = await _userService.AddAsync(model);
            if (resultAdd.Succeeded)
            {
                await _notifier.NotifyAsync("Usuario Creado", model.FullName, ChangeType.Created);
                return Ok(new { message = "Usuario creado correctamente" });
            }
            else
                return BadRequest(resultAdd.Errors);
        }


        [HttpPost("DeleteAsync")]
        public async Task<IActionResult> DeleteAsync(string id)
        {
            IdentityResult resultDelete = await _userService.DeleteAsync(id);
            if (resultDelete.Succeeded)
            {
                await _notifier.NotifyAsync(id, "Usuario Borrado", ChangeType.Deleted);
                return Ok(new { message = "Usuario eliminado correctamente" });
            }
            else
                return BadRequest(resultDelete.Errors);
        }


        [HttpPost("UpdateAsync")]
        public async Task<IActionResult> UpdateAsync(UserViewModel model)
        {
            IdentityResult resultAdd = await _userService.UpdateAsync(model);
            if (resultAdd.Succeeded)
            {
                await _notifier.NotifyAsync("Usuario Actualizado", model.FullName, ChangeType.Updated);
                return Ok(new { message = "Usuario actualizado correctamente" });
            }
            else return BadRequest(resultAdd.Errors);
        }


        [HttpPost("ChangeRoleAsync")]
        public async Task<IActionResult> ChangeRoleAsync(UserViewModel model)
        {
            IdentityResult resultAdd = await _userService.ChangeRoleAsync(model);
            return resultAdd.Succeeded ? Ok(new { message = "Rol actualizado correctamente" }) : BadRequest(resultAdd.Errors);
        }


        [HttpPost("GetAllAsync")]
        public async Task<IActionResult> GetAllAsync(UserFilterDto model)
        {
            UserResponse userResponse = await _userService.GetAllAsync(model.Page, model.PageSize);
            return userResponse.Success ? Ok(userResponse) : BadRequest(userResponse.Message);
        }





        [HttpPost("GetByIdAsync")]
        public async Task<IActionResult> GetByIdAsync(UserFilterDto model)
        {
            UserViewModel? user = await _userService.GetByIdAsync(model.Id);
            return user == null ?
                NotFound()
                :
                Ok(user);
        }




    }
}
