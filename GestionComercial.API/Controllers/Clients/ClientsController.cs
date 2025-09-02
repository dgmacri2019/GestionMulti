using GestionComercial.API.Security;
using GestionComercial.Applications.Interfaces;
using GestionComercial.Applications.Notifications;
using GestionComercial.Domain.DTOs.Client;
using GestionComercial.Domain.Entities.Masters;
using GestionComercial.Domain.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static GestionComercial.Domain.Constant.Enumeration;

namespace GestionComercial.API.Controllers.Clients
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    [AuthorizePermission]
    public class ClientsController : ControllerBase
    {
        private readonly IClientService _clienService;
        private readonly IMasterService _masterService;
        private readonly IClientsNotifier _notifier;

        public ClientsController(IClientService clienService, IMasterService masterService, IClientsNotifier notifier)
        {
            _clienService = clienService;
            _masterService = masterService;
            _notifier = notifier;
        }

        [HttpPost("notify")]
        public async Task<IActionResult> Notify(int id)
        {
            await _notifier.NotifyAsync(id, "", ChangeType.Updated);
            return Ok();
        }

        [HttpPost("AddAsync")]
        public async Task<IActionResult> AddAsync([FromBody] Client client)
        {
            GeneralResponse resultAdd = await _masterService.AddAsync(client);

            if (resultAdd.Success)
            {
                await _notifier.NotifyAsync(client.Id, client.BusinessName, ChangeType.Created);

                return
                    Ok("Cliente creado correctamente");
            }
            else return BadRequest(resultAdd.Message);
        }


        [HttpPost("UpdateAsync")]
        public async Task<IActionResult> UpdateAsync([FromBody] Client client)
        {
            GeneralResponse resultAdd = await _masterService.UpdateAsync(client);
            if (resultAdd.Success)
            {
                await _notifier.NotifyAsync(client.Id, client.BusinessName, ChangeType.Updated);

                return Ok("Cliente actualizado correctamente");
            }
            else return BadRequest(resultAdd.Message);
        }

        [HttpPost("DeleteAsync")]
        public async Task<IActionResult> DeleteAsync([FromBody] ClientFilterDto filter)
        {
            ClientViewModel? client = await _clienService.GetByIdAsync(filter.Id);

            if (client == null)
                return BadRequest("No se reconoce el cliente");

            GeneralResponse resultAdd = await _clienService.DeleteAsync(client.Id);
            if (resultAdd.Success)
            {
                await _notifier.NotifyAsync(client.Id, client.BusinessName, ChangeType.Deleted);

                return Ok("Cliente borrado correctamente");
            }
            else return BadRequest(resultAdd.Message);
        }

        [HttpPost("GetAllAsync")]
        public async Task<IActionResult> GetAllAsync([FromBody] ClientFilterDto filter)
        {
            IEnumerable<ClientViewModel> clients = await _clienService.GetAllAsync();
            return Ok(clients);
        }


        //[HttpPost("SearchToListAsync")]
        //public async Task<IActionResult> SearchToListAsync([FromBody] ClientFilterDto filter)
        //{
        //    IEnumerable<ClientViewModel> articles = await _clienService.SearchToListAsync(filter.Name, filter.IsEnabled, filter.IsDeleted);
        //    return Ok(articles);
        //}

        [HttpPost("GetByIdAsync")]
        public async Task<IActionResult> GetByIdAsync([FromBody] ClientFilterDto filter)
        {
            ClientViewModel? client = await _clienService.GetByIdAsync(filter.Id);
            if (client == null)
                return NotFound();

            return Ok(client);
        }




    }
}
