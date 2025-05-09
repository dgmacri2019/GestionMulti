using GestionComercial.API.Security;
using GestionComercial.Applications.Interfaces;
using GestionComercial.Domain.DTOs.Provider;
using GestionComercial.Domain.Entities.Masters;
using GestionComercial.Domain.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GestionComercial.API.Controllers.Providers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    [AuthorizePermission]
    public class ProvidersController : ControllerBase
    {
        private readonly IProviderService _providerService;

        public ProvidersController(IProviderService providerService)
        {
            _providerService = providerService;
        }


        [HttpPost("AddAsync")]
        public async Task<IActionResult> AddAsync([FromBody] Provider provider)
        {
            GeneralResponse resultAdd = await _providerService.AddAsync(provider);
            return resultAdd.Success ?
                Ok("Proveedor creado correctamente")
                :
            BadRequest(resultAdd.Message);
        }


        [HttpPost("UpdateAsync")]
        public async Task<IActionResult> UpdateAsync([FromBody] Provider provider)
        {
            GeneralResponse resultAdd = await _providerService.UpdateAsync(provider);
            return resultAdd.Success ?
                Ok("Proveedor actualizado correctamente")
                :
                BadRequest(resultAdd.Message);
        }

        [HttpPost("DeleteAsync")]
        public async Task<IActionResult> DeleteAsync([FromBody] ProviderFilterDto filter)
        {
            GeneralResponse resultAdd = await _providerService.DeleteAsync(filter.Id);
            return resultAdd.Success ?
                Ok("Proveedor borrado correctamente")
                :
                BadRequest(resultAdd.Message);
        }

        [HttpPost("GetAllAsync")]
        public async Task<IActionResult> GetAllAsync([FromBody] ProviderFilterDto filter)
        {
            IEnumerable<ProviderViewModel> articles = await _providerService.GetAllAsync(filter.IsEnabled, filter.IsDeleted);
            return Ok(articles);
        }

        [HttpPost("SearchToListAsync")]
        public async Task<IActionResult> SearchToListAsync([FromBody] ProviderFilterDto filter)
        {
            IEnumerable<ProviderViewModel> articles = await _providerService.SearchToListAsync(filter.Name, filter.IsEnabled, filter.IsDeleted);
            return Ok(articles);
        }

        [HttpPost("GetByIdAsync")]
        public async Task<IActionResult> GetByIdAsync([FromBody] ProviderFilterDto filter)
        {
            ProviderViewModel? article = await _providerService.GetByIdAsync(filter.Id, filter.IsEnabled, filter.IsDeleted);
            if (article == null)
                return NotFound();

            return Ok(article);
        }

    }
}
