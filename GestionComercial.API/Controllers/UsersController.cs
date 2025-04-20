using GestionComercial.Applications.Interfaces;
using GestionComercial.Domain.Entities.Masters;
using Microsoft.AspNetCore.Mvc;

namespace GestionComercial.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
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


       
        [HttpPost("CreateAsync")]
        public async Task<IActionResult> CreateAsync(User user)
        {
            await _userService.AddAsync(user);
            return CreatedAtAction(nameof(GetById), new { id = user.Id }, user);
        }
        
        [HttpPost("Create")]
        public IActionResult Create(User user)
        {
            _userService.Add(user);
            return CreatedAtAction(nameof(GetById), new { id = user.Id }, user);
        }



        [HttpPut("UpdateAsync/{id}")]
        public async Task<IActionResult> UpdateAsync(string id, User user)
        {
            if (id != user.Id) return BadRequest();
            await _userService.UpdateAsync(user);
            return NoContent();
        }
        
        [HttpPut("Update/{id}")]
        public IActionResult Update(string id, User user)
        {
            if (id != user.Id) return BadRequest();
            _userService.Update(user);
            return NoContent();
        }



        [HttpDelete("DeleteAsync/{id}")]
        public async Task<IActionResult> DeleteAsync(string id)
        {
            await _userService.DeleteAsync(id);
            return NoContent();
        }

        [HttpDelete("Delete/{id}")]
        public IActionResult Delete(string id)
        {
            _userService.Delete(id);
            return NoContent();
        }
    }
}
