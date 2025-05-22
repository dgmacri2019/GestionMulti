using GestionComercial.API.Security;
using GestionComercial.Applications.Interfaces;
using GestionComercial.Domain.DTOs.Accounts;
using GestionComercial.Domain.Entities.AccountingBook;
using GestionComercial.Domain.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GestionComercial.API.Controllers.Accounts
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    [AuthorizePermission]
    public class AccountsController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly IMasterService _masterService;

        public AccountsController(IAccountService accountService, IMasterService masterService)
        {
            _accountService = accountService;
            _masterService = masterService;
        }


        [HttpPost("AddAsync")]
        public async Task<IActionResult> AddAsync([FromBody] Account account)
        {
            GeneralResponse resultAdd = await _masterService.AddAsync(account);
            return resultAdd.Success ?
                Ok("Cuenta contable creada correctamente")
                :
                BadRequest(resultAdd.Message);
        }


        [HttpPost("UpdateAsync")]
        public async Task<IActionResult> UpdateAsync([FromBody] Account account)
        {
            GeneralResponse resultAdd = await _masterService.UpdateAsync(account);
            return resultAdd.Success ?
                Ok("Cuenta contable actualizada correctamente")
                :
                BadRequest(resultAdd.Message);
        }


        [HttpPost("GetAllAsync")]
        public async Task<IActionResult> GetAllAsync([FromBody] AccountFilterDto filter)
        {
            IEnumerable<AccountViewModel> articles = await _accountService.GetAllAsync(filter.IsEnabled, filter.IsDeleted, filter.All);
            return Ok(articles);
        }


        [HttpPost("SearchToListAsync")]
        public async Task<IActionResult> SearchToListAsync([FromBody] AccountFilterDto filter)
        {
            IEnumerable<AccountViewModel> articles = await _accountService.SearchToListAsync(filter.Name, filter.IsEnabled, filter.IsDeleted);
            return Ok(articles);
        }

        [HttpPost("GetByIdAsync")]
        public async Task<IActionResult> GetByIdAsync([FromBody] AccountFilterDto filter)
        {
            AccountViewModel? article = await _accountService.GetByIdAsync(filter.Id);
            if (article == null)
                return NotFound();

            return Ok(article);
        }

    }
}
