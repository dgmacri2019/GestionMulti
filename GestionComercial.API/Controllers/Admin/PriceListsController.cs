using GestionComercial.API.Security;
using GestionComercial.Applications.Interfaces;
using GestionComercial.Domain.DTOs.PriceLists;
using GestionComercial.Domain.Entities.Masters;
using GestionComercial.Domain.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GestionComercial.API.Controllers.Admin
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    [AuthorizePermission]
    public class PriceListsController : ControllerBase
    {
        private readonly IPriceListService _priceListService;


        public PriceListsController(IPriceListService priceListService)
        {
            _priceListService = priceListService;
        }





        [HttpPost("AddAsync")]
        public async Task<IActionResult> AddAsync([FromBody] PriceList priceList)
        {
            GeneralResponse resultAdd = await _priceListService.AddAsync(priceList);
            return resultAdd.Success ?
                Ok("Lista de precios creada correctamente")
                :
                BadRequest(resultAdd.Message);
        }


        [HttpPost("UpdateAsync")]
        public async Task<IActionResult> UpdateAsync([FromBody] PriceList priceList)
        {
            GeneralResponse resultAdd = await _priceListService.UpdateAsync(priceList);
            return resultAdd.Success ?
                Ok("Lista de precios actualizada correctamente")
                :
                BadRequest(resultAdd.Message);
        }

        [HttpPost("DeleteAsync")]
        public async Task<IActionResult> DeleteAsync([FromBody] PriceListFilterDto filter)
        {
            GeneralResponse resultAdd = await _priceListService.DeleteAsync(filter.Id);
            return resultAdd.Success ?
                Ok("Lista de precios borrada correctamente")
                :
                BadRequest(resultAdd.Message);
        }

        [HttpPost("GetAllAsync")]
        public async Task<IActionResult> GetAllAsync([FromBody] PriceListFilterDto filter)
        {
            return Ok(await _priceListService.GetAllAsync(filter.IsEnabled, filter.IsDeleted));
        }


        [HttpPost("SearchToListAsync")]
        public async Task<IActionResult> SearchToListAsync([FromBody] PriceListFilterDto filter)
        {
            return Ok(await _priceListService.SearchToListAsync(filter.Description, filter.IsEnabled, filter.IsDeleted));
        }

        [HttpPost("GetByIdAsync")]
        public async Task<IActionResult> GetByIdAsync([FromBody] PriceListFilterDto filter)
        {
            PriceListViewModel? article = await _priceListService.GetByIdAsync(filter.Id, filter.IsEnabled, filter.IsDeleted);
            return article == null ? NotFound() : Ok(article);

            if (article == null)
                return NotFound();

            return Ok(article);
        }


    }
}
