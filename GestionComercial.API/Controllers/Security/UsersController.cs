using GestionComercial.API.Security;
using GestionComercial.Applications.Interfaces;
using GestionComercial.Domain.DTOs.User;
using GestionComercial.Domain.Entities.Masters;
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
        public async Task<IActionResult> AddAsync(UserFilterDto model)
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
        public async Task<IActionResult> UpdateAsync(UserFilterDto model)
        {
            IdentityResult resultAdd = await _userService.UpdateAsync(model);
            return resultAdd.Succeeded ? Ok(new { message = "Usuario actualizado correctamente" }) : BadRequest(resultAdd.Errors);
        }


        [HttpPost("ChangeRoleAsync")]
        public async Task<IActionResult> ChangeRoleAsync(UserFilterDto model)
        {
            IdentityResult resultAdd = await _userService.ChangeRoleAsync(model);
            return resultAdd.Succeeded ? Ok(new { message = "Rol actualizado correctamente" }) : BadRequest(resultAdd.Errors);
        }


        [HttpPost("GetAllAsync")]
        public async Task<IActionResult> GetAllAsync(UserFilterDto model)
        {
            return Ok(await _userService.GetAllAsync(model));
        }


        [HttpPost("SearchToListAsync")]
        public async Task<IActionResult> SearchToListAsync(UserFilterDto model)
        {
            return Ok(await _userService.SearchToListAsync(model));
        }




        [HttpPost("GetByIdAsync")]
        public async Task<IActionResult> GetByIdAsync(UserFilterDto model)
        {
            UserViewModel? user = await _userService.GetByIdAsync(model);
            if (user == null) return NotFound();
            return Ok(user);
        }




    }
}
