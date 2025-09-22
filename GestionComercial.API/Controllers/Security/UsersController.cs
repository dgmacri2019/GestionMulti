using GestionComercial.API.Security;
using GestionComercial.Applications.Interfaces;
using GestionComercial.Domain.DTOs.User;
using GestionComercial.Domain.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace GestionComercial.API.Controllers.Security
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    [AuthorizePermission]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }




        [HttpPost("AddAsync")]
        public async Task<IActionResult> AddAsync(UserViewModel model)
        {
            IdentityResult resultAdd = await _userService.AddAsync(model);
            return resultAdd.Succeeded ? Ok(new { message = "Usuario creado correctamente" }) : BadRequest(resultAdd.Errors);
        }


        [HttpPost("DeleteAsync")]
        public async Task<IActionResult> DeleteAsync(string id)
        {
            IdentityResult resultDelete = await _userService.DeleteAsync(id);
            return resultDelete.Succeeded ? Ok(new { message = "Usuario eliminado correctamente" }) : BadRequest(resultDelete.Errors);
        }


        [HttpPost("UpdateAsync")]
        public async Task<IActionResult> UpdateAsync(UserViewModel model)
        {
            IdentityResult resultAdd = await _userService.UpdateAsync(model);
            return resultAdd.Succeeded ? Ok(new { message = "Usuario actualizado correctamente" }) : BadRequest(resultAdd.Errors);
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
