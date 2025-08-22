using GestionComercial.API.Security;
using GestionComercial.Applications.Interfaces;
using GestionComercial.Applications.Notifications;
using GestionComercial.Domain.DTOs.Provider;
using GestionComercial.Domain.Entities.Masters;
using GestionComercial.Domain.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static GestionComercial.Domain.Constant.Enumeration;

namespace GestionComercial.API.Controllers.Providers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    [AuthorizePermission]
    public class ProvidersController : ControllerBase
    {
        private readonly IProviderService _providerService;
        private readonly IMasterService _masterService;
        private readonly IProvidersNotifier _notifier;


        public ProvidersController(IProviderService providerService, IMasterService masterService, IProvidersNotifier notifier)
        {
            _providerService = providerService;
            _masterService = masterService;
            _notifier = notifier;
        }

        [HttpPost("{id:int}/notify")]
        public async Task<IActionResult> Notify(int id, [FromQuery] string nombre = "")
        {
            await _notifier.NotifyAsync(id, nombre, ChangeType.Updated);
            return Ok();
        }


        [HttpPost("AddAsync")]
        public async Task<IActionResult> AddAsync([FromBody] Provider provider)
        {
            GeneralResponse resultAdd = await _masterService.AddAsync(provider);
            if (resultAdd.Success)
            {
                await _notifier.NotifyAsync(provider.Id, provider.BusinessName, ChangeType.Created);

                return
                Ok("Proveedor creado correctamente");
            }
            else return BadRequest(resultAdd.Message);
        }


        [HttpPost("UpdateAsync")]
        public async Task<IActionResult> UpdateAsync([FromBody] Provider provider)
        {
            GeneralResponse resultAdd = await _masterService.UpdateAsync(provider);
            if (resultAdd.Success)
            {
                await _notifier.NotifyAsync(provider.Id, provider.BusinessName, ChangeType.Updated);

                return Ok("Proveedor actualizado correctamente");
            }
            else return BadRequest(resultAdd.Message);
        }

        [HttpPost("DeleteAsync")]
        public async Task<IActionResult> DeleteAsync([FromBody] ProviderFilterDto filter)
        {
            ProviderViewModel? provider = await _providerService.GetByIdAsync(filter.Id);
            if (provider == null)
                return BadRequest("No se reconoce el proveedor");

            GeneralResponse resultAdd = await _providerService.DeleteAsync(filter.Id);
            if (resultAdd.Success)
            {
                await _notifier.NotifyAsync(provider.Id, provider.BusinessName, ChangeType.Deleted);

                return Ok("Proveedor borrado correctamente");
            }
            else return BadRequest(resultAdd.Message);
        }

        [HttpPost("GetAllAsync")]
        public async Task<IActionResult> GetAllAsync([FromBody] ProviderFilterDto filter)
        {
            IEnumerable<ProviderViewModel> providers = await _providerService.GetAllAsync();
            return Ok(providers);
        }

        //[HttpPost("SearchToListAsync")]
        //public async Task<IActionResult> SearchToListAsync([FromBody] ProviderFilterDto filter)
        //{
        //    IEnumerable<ProviderViewModel> articles = await _providerService.SearchToListAsync(filter.Name, filter.IsEnabled, filter.IsDeleted);
        //    return Ok(articles);
        //}

        [HttpPost("GetByIdAsync")]
        public async Task<IActionResult> GetByIdAsync([FromBody] ProviderFilterDto filter)
        {
            ProviderViewModel? provider = await _providerService.GetByIdAsync(filter.Id);
            if (provider == null)
                return NotFound();

            return Ok(provider);
        }

    }
}
