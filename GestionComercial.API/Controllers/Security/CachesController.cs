using GestionComercial.Applications.Interfaces;
using GestionComercial.Domain.DTOs.Client;
using GestionComercial.Domain.DTOs.Master.Configurations.Commerce;
using GestionComercial.Domain.DTOs.Master.Configurations.PcParameters;
using GestionComercial.Domain.DTOs.Parameter;
using GestionComercial.Domain.DTOs.PriceLists;
using GestionComercial.Domain.DTOs.Sale;
using GestionComercial.Domain.DTOs.Stock;
using GestionComercial.Domain.DTOs.User;
using GestionComercial.Domain.Entities.Masters;
using GestionComercial.Domain.Entities.Masters.Configuration;
using GestionComercial.Domain.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GestionComercial.API.Controllers.Security
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class CachesController : ControllerBase
    {
        private readonly string superToken = "kjzsbfjlhavljhVLjKjbKSKJFBSKdfgdg45645gfdgd##@|kdfhgDJFBKNBZKBjnlkui2jsdbfljkabfñ@#€sjkfnakjbiu#@@djbfkazbsvajhLLV254351";

        private readonly IClientService _clienService;
        private readonly IUserService _userService;
        private readonly IMasterClassService _masterClassService;
        private readonly IArticleService _artcicleService;
        private readonly ISalesService _saleService;
        private readonly IParameterService _parameterService;
        private readonly IInvoiceService _invoiceService;

        public CachesController(IClientService clientService, IUserService userService,
            IMasterClassService masterClassService, IArticleService artcicleService,
            ISalesService saleService, IParameterService parameterService, IInvoiceService invoiceService)
        {
            _clienService = clientService;
            _userService = userService;
            _masterClassService = masterClassService;
            _artcicleService = artcicleService;
            _saleService = saleService;
            _parameterService = parameterService;
            _invoiceService = invoiceService;
        }



        [HttpPost("clients/GetAllAsync")]
        public async Task<IActionResult> GetAllAsync([FromBody] ClientFilterDto filter)
        {
            ClientResponse clientResponse = await _clienService.GetAllAsync(filter.Page, filter.PageSize);
            return clientResponse.Success ? Ok(clientResponse) : BadRequest(clientResponse.Message);
        }

        [HttpPost("users/GetAllAsync")]
        public async Task<IActionResult> GetAllAsync(UserFilterDto model)
        {
            UserResponse userResponse = await _userService.GetAllAsync(model.Page, model.PageSize);
            return userResponse.Success ? Ok(userResponse) : BadRequest(userResponse.Message);
        }

        [HttpPost("masterclass/GetAllPriceListAsync")]
        public async Task<IActionResult> GetAllPriceListAsync([FromBody] PriceListFilterDto filter)
        {
            return Ok(await _masterClassService.GetAllPriceListAsync());
        }

        [HttpPost("masterclass/GetAllStatesAsync")]
        public async Task<IActionResult> GetAllStatesAsync([FromBody] PriceListFilterDto filter)
        {
            return Ok(await _masterClassService.GetAllStatesAsync(filter.IsEnabled, filter.IsDeleted));
        }

        [HttpPost("masterclass/GetAllDocumentTypesAsync")]
        public async Task<IActionResult> GetAllDocumentTypesAsync([FromBody] PriceListFilterDto filter)
        {
            return Ok(await _masterClassService.GetAllDocumentTypesAsync(filter.IsEnabled, filter.IsDeleted));
        }

        [HttpPost("masterclass/GetAllIvaConditionsAsync")]
        public async Task<IActionResult> GetAllIvaConditionsAsync([FromBody] PriceListFilterDto filter)
        {
            return Ok(await _masterClassService.GetAllIvaConditionsAsync(filter.IsEnabled, filter.IsDeleted));
        }

        [HttpPost("masterclass/GetAllSaleConditionsAsync")]
        public async Task<IActionResult> GetAllSaleConditionsAsync([FromBody] PriceListFilterDto filter)
        {
            return Ok(await _masterClassService.GetAllSaleConditionsAsync(filter.IsEnabled, filter.IsDeleted));
        }

        [HttpPost("masterclass/GetAllMeasuresAsync")]
        public async Task<IActionResult> GetAllMeasuresAsync([FromBody] PriceListFilterDto filter)
        {
            return Ok(await _masterClassService.GetAllMeasuresAsync(filter.IsEnabled, filter.IsDeleted));
        }

        [HttpPost("masterclass/GetAllTaxesAsync")]
        public async Task<IActionResult> GetAllTaxesAsync([FromBody] PriceListFilterDto filter)
        {
            return Ok(await _masterClassService.GetAllTaxesAsync(filter.IsEnabled, filter.IsDeleted));
        }

        [HttpPost("masterclass/GetAllCategoriesAsync")]
        public async Task<IActionResult> GetAllCategoriesAsync([FromBody] PriceListFilterDto filter)
        {
            return Ok(await _masterClassService.GetAllCategoriesAsync());
        }

        [HttpPost("masterclass/GetCommerceDataAsync")]
        public async Task<IActionResult> GetCommerceDataAsync([FromBody] PriceListFilterDto filter)
        {
            CommerceData? commerceData = await _masterClassService.GetCommerceDataAsync();
            return commerceData == null ? Ok(new CommerceData { Id = -1 }) : Ok(commerceData);
        }

        [HttpPost("masterclass/GetBillingAsync")]
        public async Task<IActionResult> GetBillingAsync([FromBody] PriceListFilterDto filter)
        {
            BillingViewModel? billing = await _masterClassService.GetBillingAsync();
            return billing == null ? Ok(new BillingViewModel { Id = -1 }) : Ok(billing);
        }

        [HttpPost("articles/GetAllAsync")]
        public async Task<IActionResult> GetAllAsync([FromBody] ArticleFilterDto filter)
        {
            ArticleResponse articleResponse = await _artcicleService.GetAllAsync(filter.Page, filter.PageSize);
            return articleResponse.Success ? Ok(articleResponse) : BadRequest(articleResponse.Message);
        }

        [HttpPost("sales/GetAllBySalePointAsync")]
        public async Task<IActionResult> GetAllBySalePointAsync([FromBody] SaleFilterDto filter)
        {
            SaleResponse sales = await _saleService.GetAllBySalePointAsync(filter.SalePoint, (DateTime)filter.SaleDate, filter.Page, filter.PageSize);
            SaleResponse lastSaleNumber = await _saleService.GetLastSaleNumber(filter.SalePoint);
            if (!sales.Success)
                return BadRequest(sales.Message);
            if (!lastSaleNumber.Success)
                return BadRequest(lastSaleNumber.Message);

            return Ok(new SaleResponse
            {
                Success = true,
                LastSaleNumber = lastSaleNumber.LastSaleNumber,
                SaleViewModels = sales.SaleViewModels,
            });
        }


        [HttpPost("parameters/GetGeneralParameterAsync")]
        public async Task<IActionResult> GetGeneralParameterAsync()
        {
            GeneralParameter? generalParameter = await _parameterService.GetGeneralParameterAsync();
            return Ok(generalParameter);
        }

        [HttpPost("parameters/GetPcParameterAsync")]
        public async Task<IActionResult> GetPcParameterAsync([FromBody] ParameterFilterDto filter)
        {
            PcParameter? pcParameter = await _parameterService.GetPcParameterAsync(filter.PcName);
            return Ok(pcParameter);
        }

        [HttpPost("parameters/GetAllPcPrinterParametersAsync")]
        public async Task<IActionResult> GetAllPcPrinterParametersAsync()
        {
            IEnumerable<PcPrinterParametersListViewModel> pcPrinterParameters = await _parameterService.GetAllPcPrinterParametersAsync();
            return Ok(pcPrinterParameters);
        }

        [HttpPost("parameters/GetPrinterParameterFromPcAsync")]
        public async Task<IActionResult> GetPrinterParameterFromPcAsync([FromBody] ParameterFilterDto filter)
        {
            PcPrinterParametersListViewModel? pcPrinterParameter = await _parameterService.GetPrinterParameterFromPcAsync(filter.PcName);
            return Ok(pcPrinterParameter);
        }

        [HttpPost("parameters/GetEmailParamaterAsync")]
        public async Task<IActionResult> GetEmailParamaterAsync()
        {
            EmailParameter? emailParameter = await _parameterService.GetEmailParameterAsync();
            return Ok(emailParameter);
        }

        [HttpPost("parameters/GetAllPcParametersAsync")]
        public async Task<IActionResult> GetAllPcParametersAsync()
        {
            IEnumerable<PcSalePointsListViewModel> pcParameters = await _parameterService.GetAllPcParametersAsync();
            return Ok(pcParameters);
        }

        [HttpPost("invoices/GetAllBySalePointAsync")]
        public async Task<IActionResult> GetAllInvoicesBySalePointAsync([FromBody] SaleFilterDto filter)
        {
            InvoiceResponse invoiceResponse = await _invoiceService.GetAllBySalePointAsync(filter.SalePoint, (DateTime)filter.SaleDate, filter.Page, filter.PageSize);
            if (!invoiceResponse.Success)
                return BadRequest(invoiceResponse.Message);
            return Ok(new InvoiceResponse
            {
                Success = true,
                Invoices = invoiceResponse.Invoices,
            });
        }
    }
}
