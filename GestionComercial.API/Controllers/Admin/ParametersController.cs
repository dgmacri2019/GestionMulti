using GestionComercial.API.Security;
using GestionComercial.Applications.Interfaces;
using GestionComercial.Applications.Notifications;
using GestionComercial.Domain.DTOs.Master.Configurations.PcParameters;
using GestionComercial.Domain.DTOs.Parameter;
using GestionComercial.Domain.Entities.Masters.Configuration;
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
    public class ParametersController : ControllerBase
    {
        private readonly IMasterService _masterService;
        private readonly IParametersNotifier _notifier;
        private readonly IParameterService _parameterService;

        public ParametersController(IMasterService masterService, IParametersNotifier notifier, IParameterService parameterService)
        {
            _masterService = masterService;
            _notifier = notifier;
            _parameterService = parameterService;
        }


        [HttpPost("{id:int}/notify")]
        public async Task<IActionResult> Notify(int id, [FromQuery] string nombre = "")
        {
            await _notifier.NotifyAsync(id, nombre, ChangeType.Updated);
            return Ok();
        }

        [HttpPost("AddGeneralParameterAsync")]
        public async Task<IActionResult> AddAsync([FromBody] GeneralParameter generalParameter)
        {
            GeneralResponse resultAdd = await _masterService.AddAsync(generalParameter);

            if (resultAdd.Success)
            {
                await _notifier.NotifyAsync(generalParameter.Id, "Parametro General", ChangeType.Created);

                return
                    Ok("Parametro general creado correctamente");
            }
            else return BadRequest(resultAdd.Message);
        }


        [HttpPost("UpdateGeneralParameterAsync")]
        public async Task<IActionResult> UpdateAsync([FromBody] GeneralParameter generalParameter)
        {
            GeneralResponse resultAdd = await _masterService.UpdateAsync(generalParameter);
            if (resultAdd.Success)
            {
                await _notifier.NotifyAsync(generalParameter.Id, "GeneralParameter", ChangeType.Updated);

                return Ok("Parametro general actualizado correctamente");
            }
            else return BadRequest(resultAdd.Message);
        }


        [HttpPost("GetAllGeneralParametersAsync")]
        public async Task<IActionResult> GetAllAsync()
        {
            IEnumerable<GeneralParameter> generalParameters = await _parameterService.GetAllGeneralParametersAsync();
            return Ok(generalParameters);
        }


        [HttpPost("GetGeneralParameterByIdAsync")]
        public async Task<IActionResult> GetByIdAsync([FromBody] ParameterFilterDto filter)
        {
            GeneralParameter? generalParameter = await _parameterService.GetGeneralParameterByIdAsync(filter.Id);
            if (generalParameter == null)
                return NotFound();

            return Ok(generalParameter);
        }





        [HttpPost("AddPcParameterAsync")]
        public async Task<IActionResult> AddPcParameterAsync([FromBody] PcParameter pcParameter)
        {
            GeneralResponse resultAdd = await _masterService.AddAsync(pcParameter);

            if (resultAdd.Success)
            {
                await _notifier.NotifyAsync(pcParameter.Id, "Parametro Pc", ChangeType.Created);

                return
                    Ok("Parametro Pc creado correctamente");
            }
            else return BadRequest(resultAdd.Message);
        }


        [HttpPost("UpdatePcParameterAsync")]
        public async Task<IActionResult> UpdatePcParameterAsync([FromBody] PcParameter pcParameter)
        {
            GeneralResponse resultAdd = await _masterService.UpdateAsync(pcParameter);
            if (resultAdd.Success)
            {
                await _notifier.NotifyAsync(pcParameter.Id, "PcParameter", ChangeType.Updated);

                return Ok("Parametro Pc actualizado correctamente");
            }
            else return BadRequest(resultAdd.Message);
        }


        [HttpPost("GetPcParameterAsync")]
        public async Task<IActionResult> GetAllPcParametersAsync([FromBody] ParameterFilterDto filter)
        {
            PcParameter? pcParameter = await _parameterService.GetPcParameterAsync(filter.PcName);
            return Ok(pcParameter);
        }


        [HttpPost("GetAllPcParametersAsync")]
        public async Task<IActionResult> GetAllPcParametersAsync()
        {
            IEnumerable<PurchaseAndSalesListViewModel> pcParameters = await _parameterService.GetAllPcParametersAsync();
            return Ok(pcParameters);
        }


        [HttpPost("GetPcParameterByIdAsync")]
        public async Task<IActionResult> GetPcParameterByIdAsync([FromBody] ParameterFilterDto filter)
        {
            PcParameter? pcParameter = await _parameterService.GetPcParameterByIdAsync(filter.Id);
            if (pcParameter == null)
                return NotFound();

            return Ok(pcParameter);
        }





    }
}
