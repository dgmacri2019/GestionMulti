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
        public async Task<IActionResult> AddAsync(UserDto model)
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
        public async Task<IActionResult> UpdateAsync(UserDto model)
        {
            IdentityResult resultAdd = await _userService.UpdateAsync(model);
            return resultAdd.Succeeded ? Ok(new { message = "Usuario actualizado correctamente" }) : BadRequest(resultAdd.Errors);
        }


        [HttpPost("ChangeRoleAsync")]
        public async Task<IActionResult> ChangeRoleAsync(UserDto model)
        {
            IdentityResult resultAdd = await _userService.ChangeRoleAsync(model);
            return resultAdd.Succeeded ? Ok(new { message = "Rol actualizado correctamente" }) : BadRequest(resultAdd.Errors);
        }


        [HttpGet("GetAllAsync")]
        public async Task<IActionResult> GetAllAsync()
        {
            return Ok(await _userService.GetAllAsync());
        }

        [HttpGet("GetAll")]
        public IActionResult GetAll()
        {
            return Ok(_userService.GetAll());
        }



        [HttpGet("GetByIdAsync/{id}")]
        public async Task<IActionResult> GetByIdAsync(string id)
        {
            User user = await _userService.GetByIdAsync(id);
            if (user == null) return NotFound();
            return Ok(user);
        }

        [HttpGet("GetById/{id}")]
        public IActionResult GetById(string id)
        {
            User user = _userService.GetById(id);
            if (user == null) return NotFound();
            return Ok(user);
        }




    }
}
