using GestionComercial.API.Security;
using GestionComercial.Applications.Interfaces;
using GestionComercial.Applications.Notifications;
using GestionComercial.Domain.DTOs.Sale;
using GestionComercial.Domain.Entities.Sales;
using GestionComercial.Domain.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static GestionComercial.Domain.Constant.Enumeration;

namespace GestionComercial.API.Controllers.Sales
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    [AuthorizePermission]
    public class SalesController : ControllerBase
    {
        private readonly ISalesService _saleService;
        private readonly IMasterService _masterService;
        private readonly ISalesNotifier _notifierSales;
        private readonly IArticlesNotifier _notifierArticles;
        private readonly IClientsNotifier _notifierClients;


        public SalesController(ISalesService saleService, IMasterService masterService, 
            ISalesNotifier notifierSales, IArticlesNotifier notifierArticles, IClientsNotifier notifierClients)
        {
            _saleService = saleService;
            _masterService = masterService;
            _notifierSales = notifierSales;
            _notifierArticles = notifierArticles;
            _notifierClients = notifierClients;
        }

        [HttpPost("notify")]
        public async Task<IActionResult> Notify(int id)
        {
            await _notifierSales.NotifyAsync(id, "VentasActualizados", ChangeType.Updated);
            return Ok();
        }




        [HttpPost("AddAsync")]
        public async Task<IActionResult> AddAsync([FromBody] Sale sale)
        {
            //return BadRequest(new SaleResponse { Success = false, Message = "Mensaje de prueba de error" });

            SaleResponse resultAdd = await _saleService.AddAsync(sale);

            if (resultAdd.Success)
            {
                await _notifierSales.NotifyAsync(sale.Id, "Venta Creada", ChangeType.Created);
                await _notifierArticles.NotifyAsync(sale.Id, "Venta Creada", ChangeType.Updated);
                await _notifierClients.NotifyAsync(sale.Id, "Venta Creada", ChangeType.Updated);

                return
                    Ok(resultAdd);
            }
            else return BadRequest(resultAdd);
        }


        [HttpPost("UpdateAsync")]
        public async Task<IActionResult> UpdateAsync([FromBody] Sale sale)
        {
            GeneralResponse resultAdd = await _masterService.UpdateAsync(sale);
            if (resultAdd.Success)
            {
                await _notifierSales.NotifyAsync(sale.Id, sale.Client.BusinessName, ChangeType.Updated);

                return Ok("Venta actualizada correctamente");
            }
            else return BadRequest(resultAdd.Message);
        }



        [HttpPost("GetAllAsync")]
        public async Task<IActionResult> GetAllAsync([FromBody] SaleFilterDto filter)
        {
            IEnumerable<SaleViewModel> sales = await _saleService.GetAllAsync();
            return Ok(sales);
        }


        [HttpPost("GetAllBySalePointAsync")]
        public async Task<IActionResult> GetAllBySalePointAsync([FromBody] SaleFilterDto filter)
        {
            SaleResponse saleResponse = new()
            {
                Success = true
            };
            try
            {
                IEnumerable<SaleViewModel> sales = await _saleService.GetAllBySalePointAsync(filter.SalePoint, (DateTime)filter.SaleDate);
                int lastSaleNumber = await _saleService.GetLastSaleNumber(filter.SalePoint);
                saleResponse.LastSaleNumber = lastSaleNumber;
                saleResponse.SaleViewModels = [.. sales];

                return Ok(saleResponse);
            }
            catch (Exception ex)
            {
                saleResponse.Success = false;
                saleResponse.Message = ex.Message;
                return BadRequest(saleResponse);
            }
        }



        [HttpPost("GetByIdAsync")]
        public async Task<IActionResult> GetByIdAsync([FromBody] SaleFilterDto filter)
        {
            SaleViewModel? sale = await _saleService.GetByIdAsync(filter.Id);
            if (sale == null)
                return NotFound();

            return Ok(sale);
        }

    }
}
