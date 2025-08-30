using GestionComercial.API.Security;
using GestionComercial.Applications.Interfaces;
using GestionComercial.Applications.Notifications;
using GestionComercial.Applications.Services;
using GestionComercial.Domain.DTOs.Client;
using GestionComercial.Domain.DTOs.GeneralParameter;
using GestionComercial.Domain.Entities.Masters;
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
        public async Task<IActionResult> GetByIdAsync([FromBody] GeneralParameterFilterDto filter)
        {
            GeneralParameter? generalParameter = await _parameterService.GetGeneralParameterByIdAsync(filter.Id);
            if (generalParameter == null)
                return NotFound();

            return Ok(generalParameter);
        }





    }
}
