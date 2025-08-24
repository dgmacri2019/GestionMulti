using GestionComercial.API.Security;
using GestionComercial.Applications.Interfaces;
using GestionComercial.Applications.Notifications;
using GestionComercial.Domain.DTOs.Banks;
using GestionComercial.Domain.Entities.BoxAndBank;
using GestionComercial.Domain.Entities.Masters;
using GestionComercial.Domain.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static GestionComercial.Domain.Constant.Enumeration;

namespace GestionComercial.API.Controllers.BankAndBoxes
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    [AuthorizePermission]
    public class BanksController : ControllerBase
    {
        private readonly IBankService _bankService;
        private readonly IMasterService _masterService;
        private readonly IBoxAndBanksNotifier _notifier;
        private readonly IBankParametersNotifier _notifierBankParameter;

        public BanksController(IBankService bankService, IMasterService masterService, IBoxAndBanksNotifier notifier, 
            IBankParametersNotifier notifierBankParameter)
        {
            _bankService = bankService;
            _masterService = masterService;
            _notifier = notifier;
            _notifierBankParameter = notifierBankParameter;
        }

        [HttpPost("{id:int}/notify")]
        public async Task<IActionResult> Notify(int id, [FromQuery] string nombre = "")
        {
            await _notifier.NotifyAsync(id, nombre, ChangeType.Updated);
            return Ok();
        }

        [HttpPost("AddAcreditationAsync")]
        public async Task<IActionResult> AddAcreditationAsync([FromBody] Acreditation acreditation)
        {
            GeneralResponse resultAdd = await _masterService.AddAsync(acreditation);
            if (resultAdd.Success)
            {
                await _notifier.NotifyAsync(acreditation.Id, "Acreditación", ChangeType.Created);

                return Ok("Acreditación creada correctamente");
            }
            else return BadRequest(resultAdd.Message);
        }

        [HttpPost("AddBankAsync")]
        public async Task<IActionResult> AddBankAsync([FromBody] Bank bank)
        {
            GeneralResponse resultAdd = await _masterService.AddAsync(bank);
            if (resultAdd.Success)
            {
                await _notifier.NotifyAsync(bank.Id, bank.BankName, ChangeType.Created);

                return Ok("Banco creado correctamente");
            }
            else return BadRequest(resultAdd.Message);
        }

        [HttpPost("AddBankParamerAsync")]
        public async Task<IActionResult> AddBankParamerAsync([FromBody] BankParameter bankParameter)
        {
            GeneralResponse resultAdd = await _masterService.AddAsync(bankParameter);
            if (resultAdd.Success)
            {
                await _notifierBankParameter.NotifyAsync(bankParameter.Id, "Parámetro Bancario", ChangeType.Created);

                return Ok("Parametro bancario creado correctamente");
            }
            else return BadRequest(resultAdd.Message);
        }

        [HttpPost("AddBoxAsync")]
        public async Task<IActionResult> AddBoxAsync([FromBody] Box box)
        {
            GeneralResponse resultAdd = await _masterService.AddAsync(box);
            if (resultAdd.Success)
            {
                await _notifier.NotifyAsync(box.Id, box.BoxName, ChangeType.Created);

                return Ok("Caja creada correctamente");
            }
            else return BadRequest(resultAdd.Message);
        }

        [HttpPost("AddDebitationAsync")]
        public async Task<IActionResult> AddDebitationAsync([FromBody] Debitation debitation)
        {
            GeneralResponse resultAdd = await _masterService.AddAsync(debitation);
            if (resultAdd.Success)
            {
                await _notifier.NotifyAsync(debitation.Id, "Débito", ChangeType.Created);

                return Ok("Débito creado correctamente");
            }
            else return BadRequest(resultAdd.Message);
        }



        [HttpPost("UpdateAcreditationAsync")]
        public async Task<IActionResult> UpdateAcreditationAsync([FromBody] Acreditation acreditation)
        {
            GeneralResponse resultAdd = await _masterService.UpdateAsync(acreditation);
            if (resultAdd.Success)
            {
                await _notifier.NotifyAsync(acreditation.Id, "Acreditacion", ChangeType.Created);

                return Ok("Acreditación modificada correctamente");
            }
            else return BadRequest(resultAdd.Message);
        }

        [HttpPost("UpdateBankAsync")]
        public async Task<IActionResult> UpdateBankAsync([FromBody] Bank bank)
        {
            GeneralResponse resultAdd = await _masterService.UpdateAsync(bank);
            if (resultAdd.Success)
            {
                await _notifier.NotifyAsync(bank.Id, bank.BankName, ChangeType.Created);

                return Ok("Banco modificado correctamente");
            }
            else return BadRequest(resultAdd.Message);
        }

        [HttpPost("UpdateBankParameterAsync")]
        public async Task<IActionResult> UpdateBankParameterAsync([FromBody] BankParameter bankParameter)
        {
            GeneralResponse resultAdd = await _masterService.UpdateAsync(bankParameter);
            if (resultAdd.Success)
            {
                await _notifierBankParameter.NotifyAsync(bankParameter.Id, "Parámetro Bancario", ChangeType.Created);

                return Ok("Parametro bancario modificado correctamente");
            }
            else return BadRequest(resultAdd.Message);
        }

        [HttpPost("UpdateBoxAsync")]
        public async Task<IActionResult> UpdateBoxAsync([FromBody] Box box)
        {
            GeneralResponse resultAdd = await _masterService.UpdateAsync(box);
            if (resultAdd.Success)
            {
                await _notifier.NotifyAsync(box.Id, box.BoxName, ChangeType.Created);

                return Ok("Caja modificada correctamente");
            }
            else return BadRequest(resultAdd.Message);
        }

        [HttpPost("UpdateDebitationAsync")]
        public async Task<IActionResult> UpdateDebitationAsync([FromBody] Debitation debitation)
        {
            GeneralResponse resultAdd = await _masterService.UpdateAsync(debitation);
            if (resultAdd.Success)
            {
                await _notifier.NotifyAsync(debitation.Id, "Débito", ChangeType.Created);

                return Ok("Débito modificado correctamente");
            }
            else return BadRequest(resultAdd.Message);
        }



        [HttpPost("GetBankByIdAsync")]
        public async Task<IActionResult> GetBankByIdAsync([FromBody] BankFilterDto filter)
        {
            BankViewModel? result = await _bankService.GetBankByIdAsync(filter.Id);
            if (result == null)
                return NotFound();

            return Ok(result);
        }

        [HttpPost("GetBoxByIdAsync")]
        public async Task<IActionResult> GetBoxByIdAsync([FromBody] BankFilterDto filter)
        {
            BoxViewModel? result = await _bankService.GetBoxByIdAsync(filter.Id);
            if (result == null)
                return NotFound();

            return Ok(result);
        }

        [HttpPost("SearchBankAndBoxToListAsync")]
        public async Task<IActionResult> SearchBankAndBoxToListAsync([FromBody] BankFilterDto filter)
        {
            IEnumerable<BankAndBoxViewModel> result = await _bankService.SearchBankAndBoxToListAsync();
            if (result == null)
                return NotFound();

            return Ok(result);
        }


        [HttpPost("SearchBankParameterToListAsync")]
        public async Task<IActionResult> SearchBankParameterToListAsync([FromBody] BankFilterDto filter)
        {
            IEnumerable<BankParameterViewModel> result = await _bankService.SearchBankParameterToListAsync();
            if (result == null)
                return NotFound();

            return Ok(result);
        }

        [HttpPost("GetBankParameterByIdAsync")]
        public async Task<IActionResult> GetBankParameterByIdAsync([FromBody] BankFilterDto filter)
        {
            BankParameterViewModel? result = await _bankService.GetBankParameterByIdAsync(filter.Id);
            if (result == null)
                return NotFound();

            return Ok(result);
        }


    }
}
