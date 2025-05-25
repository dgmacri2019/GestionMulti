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


        [HttpPost("AddAccountAsync")]
        public async Task<IActionResult> AddAccountAsync([FromBody] Account account)
        {
            GeneralResponse resultAdd = await _masterService.AddAsync(account);
            return resultAdd.Success ?
                Ok("Cuenta contable creada correctamente")
                :
                BadRequest(resultAdd.Message);
        }


        [HttpPost("UpdateAccountAsync")]
        public async Task<IActionResult> UpdateAccountAsync([FromBody] Account account)
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
            IEnumerable<AccountViewModel> accounts = await _accountService.GetAllAsync(filter.IsEnabled, filter.IsDeleted, filter.All);
            return Ok(accounts);
        }


        [HttpPost("SearchToListAsync")]
        public async Task<IActionResult> SearchToListAsync([FromBody] AccountFilterDto filter)
        {
            IEnumerable<AccountViewModel> accounts = await _accountService.SearchToListAsync(filter.Name, filter.IsEnabled, filter.IsDeleted);
            return Ok(accounts);
        }

        [HttpPost("GetByIdAsync")]
        public async Task<IActionResult> GetByIdAsync([FromBody] AccountFilterDto filter)
        {
            AccountViewModel? account = await _accountService.GetByIdAsync(filter.Id);
            if (account == null)
                return NotFound();

            return Ok(account);
        }


        [HttpPost("GetAllAccountTypesAsync")]
        public async Task<IActionResult> GetAllAccountTypesAsync([FromBody] AccountFilterDto filter)
        {
            IEnumerable<AccountType> accountTypes = await _accountService.GetAllAccountTypesAsync(filter.IsEnabled, filter.IsDeleted, filter.All);
            return Ok(accountTypes);
        }

        [HttpPost("GetAllAccountsAsync")]
        public async Task<IActionResult> GetAllAccountsAsync([FromBody] AccountFilterDto filter)
        {
            IEnumerable<Account> accounts = await _accountService.GetAllAccountsAsync(filter.IsEnabled, filter.IsDeleted, filter.All);
            return Ok(accounts);
        }

        [HttpPost("GetAccountGroup1Async")]
        public async Task<IActionResult> GetAccountGroup1Async([FromBody] AccountFilterDto filter)
        {
            IEnumerable<Account> accounts = await _accountService.GetAccountGroup1Async(filter.AccountType, filter.IsEnabled, filter.IsDeleted, filter.All);
            return Ok(accounts);
        }

        [HttpPost("GetAccountGroup2Async")]
        public async Task<IActionResult> GetAccountGroup2Async([FromBody] AccountFilterDto filter)
        {
            IEnumerable<Account> accounts = await _accountService.GetAccountGroup2Async(filter.AccountType, filter.AccountGroup1, filter.IsEnabled, filter.IsDeleted, filter.All);
            return Ok(accounts);
        }

        [HttpPost("GetAccountGroup3Async")]
        public async Task<IActionResult> GetAccountGroup3Async([FromBody] AccountFilterDto filter)
        {
            IEnumerable<Account> accounts = await _accountService.GetAccountGroup3Async(filter.AccountType, filter.AccountGroup1, filter.AccountGroup2, filter.IsEnabled,
                filter.IsDeleted, filter.All);
            return Ok(accounts);
        }

        [HttpPost("GetAccountGroup4Async")]
        public async Task<IActionResult> GetAccountGroup4Async([FromBody] AccountFilterDto filter)
        {
            IEnumerable<Account> accounts = await _accountService.GetAccountGroup4Async(filter.AccountType, filter.AccountGroup1, filter.AccountGroup2, filter.AccountGroup3,
                filter.IsEnabled, filter.IsDeleted, filter.All);
            return Ok(accounts);
        }

        [HttpPost("GetAccountGroup5Async")]
        public async Task<IActionResult> GetAccountGroup5Async([FromBody] AccountFilterDto filter)
        {
            IEnumerable<Account> accounts = await _accountService.GetAccountGroup5Async(filter.AccountType, filter.AccountGroup1, filter.AccountGroup2, filter.AccountGroup3,
                filter.AccountGroup4, filter.IsEnabled, filter.IsDeleted, filter.All);
            return Ok(accounts);
        }


    }
}
