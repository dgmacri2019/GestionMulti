using GestionComercial.API.Security;
using GestionComercial.Applications.Interfaces;
using GestionComercial.Applications.Notifications;
using GestionComercial.Domain.DTOs.PriceLists;
using GestionComercial.Domain.Entities.Afip;
using GestionComercial.Domain.Entities.Masters;
using GestionComercial.Domain.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static GestionComercial.Domain.Constant.Enumeration;

namespace GestionComercial.API.Controllers.Admin
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    [AuthorizePermission]
    public class MasterClassController : ControllerBase
    {
        private readonly IMasterClassService _masterClassService;
        private readonly IMasterService _masterService;
        private readonly IMasterClassNotifier _notifier;

        public MasterClassController(IMasterClassService masterClassService, IMasterService masterService, IMasterClassNotifier notifier)
        {
            _masterClassService = masterClassService;
            _masterService = masterService;
            _notifier = notifier;
        }

        [HttpPost("AddStateAsync")]
        public async Task<IActionResult> AddStateAsync([FromBody] State state)
        {
            GeneralResponse resultAdd = await _masterService.AddAsync(state);
            if (resultAdd.Success)
            {
                await _notifier.NotifyAsync(state.Id, "Provencia Creada", ChangeType.Created);

                return
                    Ok("Provincia creada correctamente");
            }
            else return BadRequest(resultAdd.Message);
        }

        [HttpPost("UpdateStateAsync")]
        public async Task<IActionResult> UpdateStateAsync([FromBody] State state)
        {
            GeneralResponse resultAdd = await _masterService.UpdateAsync(state);
            if (resultAdd.Success)
            {
                await _notifier.NotifyAsync(state.Id, "Pronvincia Actualizada", ChangeType.Updated);

                return
                    Ok("Provincia actualizada correctamente");
            }
            else return BadRequest(resultAdd.Message);
        }



        [HttpPost("AddDocumentTypeAsync")]
        public async Task<IActionResult> AddDocumentTypeAsync([FromBody] DocumentType documentType)
        {
            GeneralResponse resultAdd = await _masterService.AddAsync(documentType);
            if (resultAdd.Success)
            {
                await _notifier.NotifyAsync(documentType.Id, "Tipo de documento", ChangeType.Created);

                return
                    Ok("Tipo de decumento creado correctamente");
            }
            else return BadRequest(resultAdd.Message);
        }

        [HttpPost("UpdateDocumentTypeAsync")]
        public async Task<IActionResult> UpdateDocumentTypeAsync([FromBody] DocumentType documentType)
        {
            GeneralResponse resultAdd = await _masterService.UpdateAsync(documentType);
            if (resultAdd.Success)
            {
                await _notifier.NotifyAsync(documentType.Id, "Tipo de documento actualizado", ChangeType.Updated);

                return
                    Ok("Tipo de documento actualizado correctamente");
            }
            else return BadRequest(resultAdd.Message);
        }



        [HttpPost("AddIvaConditionAsync")]
        public async Task<IActionResult> AddIvaConditionAsync([FromBody] IvaCondition ivaCondition)
        {
            GeneralResponse resultAdd = await _masterService.AddAsync(ivaCondition);
            if (resultAdd.Success)
            {
                await _notifier.NotifyAsync(ivaCondition.Id, "Tipo de iva crado", ChangeType.Created);

                return
                    Ok("Tipo de iva creado correctamente");
            }
            else return BadRequest(resultAdd.Message);
        }

        [HttpPost("UpdateIvaConditionAsync")]
        public async Task<IActionResult> UpdateIvaConditionAsync([FromBody] IvaCondition ivaCondition)
        {
            GeneralResponse resultAdd = await _masterService.UpdateAsync(ivaCondition);
            if (resultAdd.Success)
            {
                await _notifier.NotifyAsync(ivaCondition.Id, "Condición de venta actualizada", ChangeType.Updated);

                return
                    Ok("Condición de venta actualizada correctamente");
            }
            else return BadRequest(resultAdd.Message);
        }



        [HttpPost("AddSaleConditionAsync")]
        public async Task<IActionResult> AddSaleConditionAsync([FromBody] SaleCondition saleCondition)
        {
            GeneralResponse resultAdd = await _masterService.AddAsync(saleCondition);
            if (resultAdd.Success)
            {
                await _notifier.NotifyAsync(saleCondition.Id, "Condición de venta creada", ChangeType.Created);

                return
                    Ok("Condición de venta creada correctamente");
            }
            else return BadRequest(resultAdd.Message);
        }

        [HttpPost("UpdateSaleConditionAsync")]
        public async Task<IActionResult> UpdateSaleConditionAsync([FromBody] SaleCondition saleCondition)
        {
            GeneralResponse resultAdd = await _masterService.UpdateAsync(saleCondition);
            if (resultAdd.Success)
            {
                await _notifier.NotifyAsync(saleCondition.Id, "Condición de venta actualizada", ChangeType.Updated);

                return
                    Ok("Condición de venta actualizada correctamente");
            }
            else return BadRequest(resultAdd.Message);
        }








        [HttpPost("GetAllStatesAsync")]
        public async Task<IActionResult> GetAllStatesAsync([FromBody] PriceListFilterDto filter)
        {
            return Ok(await _masterClassService.GetAllStatesAsync(filter.IsEnabled, filter.IsDeleted));
        }

        [HttpPost("GetAllDocumentTypesAsync")]
        public async Task<IActionResult> GetAllDocumentTypesAsync([FromBody] PriceListFilterDto filter)
        {
            return Ok(await _masterClassService.GetAllDocumentTypesAsync(filter.IsEnabled, filter.IsDeleted));
        }

        [HttpPost("GetAllIvaConditionsAsync")]
        public async Task<IActionResult> GetAllIvaConditionsAsync([FromBody] PriceListFilterDto filter)
        {
            return Ok(await _masterClassService.GetAllIvaConditionsAsync(filter.IsEnabled, filter.IsDeleted));
        }

        [HttpPost("GetAllSaleConditionsAsync")]
        public async Task<IActionResult> GetAllSaleConditionsAsync([FromBody] PriceListFilterDto filter)
        {
            return Ok(await _masterClassService.GetAllSaleConditionsAsync(filter.IsEnabled, filter.IsDeleted));
        }

    }
}
