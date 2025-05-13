using GestionComercial.API.Security;
using GestionComercial.Applications.Interfaces;
using GestionComercial.Domain.DTOs.Bank;
using GestionComercial.Domain.Entities.BoxAndBank;
using GestionComercial.Domain.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace GestionComercial.API.Controllers.BankAndBoxes
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    [AuthorizePermission]
    public class BanksController : ControllerBase
    {
        private readonly IBankService _bankService;

        public BanksController(IBankService bankService)
        {
            _bankService = bankService;
        }



        [HttpPost("AddAcreditationAsync")]
        public async Task<IActionResult> AddAcreditationAsync([FromBody] Acreditation acreditation)
        {
            GeneralResponse resultAdd = await _bankService.AddAcreditationAsync(acreditation);
            return resultAdd.Success ?
                Ok("Acreditación creada correctamente")
                :
                BadRequest(resultAdd.Message);
        }

        [HttpPost("AddBankAsync")]
        public async Task<IActionResult> AddBankAsync([FromBody] Bank bank)
        {
            GeneralResponse resultAdd = await _bankService.AddBankAsync(bank);
            return resultAdd.Success ?
                Ok("Banco creado correctamente")
                :
                BadRequest(resultAdd.Message);
        }

        [HttpPost("AddBankParamerAsync")]
        public async Task<IActionResult> AddBankParamerAsync([FromBody] BankParameter bankParameter)
        {
            GeneralResponse resultAdd = await _bankService.AddBankParamerAsync(bankParameter);
            return resultAdd.Success ?
                Ok("Parametro bancario creado correctamente")
                :
                BadRequest(resultAdd.Message);
        }

        [HttpPost("AddBoxAsync")]
        public async Task<IActionResult> AddBoxAsync([FromBody] Box box)
        {
            GeneralResponse resultAdd = await _bankService.AddBoxAsync(box);
            return resultAdd.Success ?
                Ok("Caja creada correctamente")
                :
                BadRequest(resultAdd.Message);
        }

        [HttpPost("AddDebitationAsync")]
        public async Task<IActionResult> AddDebitationAsync([FromBody] Debitation debitation)
        {
            GeneralResponse resultAdd = await _bankService.AddDebitationAsync(debitation);
            return resultAdd.Success ?
                Ok("Débito creado correctamente")
                :
                BadRequest(resultAdd.Message);
        }



        [HttpPost("UpdateAcreditationAsync")]
        public async Task<IActionResult> UpdateAcreditationAsync([FromBody] Acreditation acreditation)
        {
            GeneralResponse resultAdd = await _bankService.UpdateAcreditationAsync(acreditation);
            return resultAdd.Success ?
                Ok("Acreditación modificada correctamente")
                :
                BadRequest(resultAdd.Message);
        }

        [HttpPost("UpdateBankAsync")]
        public async Task<IActionResult> UpdateBankAsync([FromBody] Bank bank)
        {  
            GeneralResponse resultAdd = await _bankService.UpdateBankAsync(bank);
            return resultAdd.Success ?
                Ok("Banco modificado correctamente")
                :
                BadRequest(resultAdd.Message);
        }

        [HttpPost("UpdateBankParamerAsync")]
        public async Task<IActionResult> UpdateBankParamerAsync([FromBody] BankParameter bankParameter)
        {
            GeneralResponse resultAdd = await _bankService.UpdateBankParamerAsync(bankParameter);
            return resultAdd.Success ?
                Ok("Parametro bancario modificado correctamente")
                :
                BadRequest(resultAdd.Message);
        }

        [HttpPost("UpdateBoxAsync")]
        public async Task<IActionResult> UpdateBoxAsync([FromBody] Box box)
        {
            GeneralResponse resultAdd = await _bankService.UpdateBoxAsync(box);
            return resultAdd.Success ?
                Ok("Caja modificada correctamente")
                :
                BadRequest(resultAdd.Message);
        }

        [HttpPost("UpdateDebitationAsync")]
        public async Task<IActionResult> UpdateDebitationAsync([FromBody] Debitation debitation)
        {
            GeneralResponse resultAdd = await _bankService.UpdateDebitationAsync(debitation);
            return resultAdd.Success ?
                Ok("Débito modificado correctamente")
                :
                BadRequest(resultAdd.Message);
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
            IEnumerable<BankAndBoxViewModel> result = await _bankService.SearchBankAndBoxToListAsync(filter.Name, filter.IsEnabled, filter.IsDeleted);
            if (result == null)
                return NotFound();

            return Ok(result);
        }




    }
}
