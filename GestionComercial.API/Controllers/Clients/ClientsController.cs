using GestionComercial.API.Security;
using GestionComercial.Applications.Interfaces;
using GestionComercial.Domain.DTOs.Client;
using GestionComercial.Domain.Entities.Masters;
using GestionComercial.Domain.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GestionComercial.API.Controllers.Clients
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    [AuthorizePermission]
    public class ClientsController : ControllerBase
    {
        private readonly IClienService _clienService;

        public ClientsController(IClienService clienService)
        {
            _clienService = clienService;
        }


        [HttpPost("AddAsync")]
        public async Task<IActionResult> AddAsync([FromBody] Client client)
        {
            GeneralResponse resultAdd = await _clienService.AddAsync(client);
            return resultAdd.Success ?
                Ok("Cliente creado correctamente")
                :
                BadRequest(resultAdd.Message);
        }


        [HttpPost("UpdateAsync")]
        public async Task<IActionResult> UpdateAsync([FromBody] Client client)
        {
            GeneralResponse resultAdd = await _clienService.UpdateAsync(client);
            return resultAdd.Success ?
                Ok("Cliente actualizado correctamente")
                :
                BadRequest(resultAdd.Message);
        }

        [HttpPost("DeleteAsync")]
        public async Task<IActionResult> DeleteAsync([FromBody] ClientFilterDto filter)
        {
            GeneralResponse resultAdd = await _clienService.DeleteAsync(filter.Id);
            return resultAdd.Success ?
                Ok("Cliente borrado correctamente")
                :
                BadRequest(resultAdd.Message);
        }

        [HttpPost("GetAllAsync")]
        public async Task<IActionResult> GetAllAsync([FromBody] ClientFilterDto filter)
        {
            IEnumerable<ClientViewModel> articles = await _clienService.GetAllAsync(filter.IsEnabled, filter.IsDeleted);
            return Ok(articles);
        }


        [HttpPost("SearchToListAsync")]
        public async Task<IActionResult> SearchToListAsync([FromBody] ClientFilterDto filter)
        {
            IEnumerable<ClientViewModel> articles = await _clienService.SearchToListAsync(filter.Name, filter.IsEnabled, filter.IsDeleted);
            return Ok(articles);
        }

        [HttpPost("GetByIdAsync")]
        public async Task<IActionResult> GetByIdAsync([FromBody] ClientFilterDto filter)
        {
            ClientViewModel? article = await _clienService.GetByIdAsync(filter.Id, filter.IsEnabled, filter.IsDeleted);
            if (article == null)
                return NotFound();

            return Ok(article);
        }




    }
}
