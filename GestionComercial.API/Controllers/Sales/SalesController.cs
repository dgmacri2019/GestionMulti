using GestionComercial.API.Helpers;
using GestionComercial.API.NamedPipe;
using GestionComercial.API.Security;
using GestionComercial.Applications.Interfaces;
using GestionComercial.Applications.Notifications;
using GestionComercial.Contract.Responses;
using GestionComercial.Contract.ViewModels;
using GestionComercial.Domain.DTOs.Client;
using GestionComercial.Domain.DTOs.Sale;
using GestionComercial.Domain.Entities.Afip;
using GestionComercial.Domain.Entities.Masters;
using GestionComercial.Domain.Entities.Sales;
using GestionComercial.Domain.Helpers;
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
        private readonly IMasterClassService _masterClassService;
        private readonly IClientService _clientService;


        public SalesController(ISalesService saleService, IMasterService masterService,
            ISalesNotifier notifierSales, IArticlesNotifier notifierArticles, IClientsNotifier notifierClients,
            IMasterClassService masterClassService, IClientService clientService)
        {
            _saleService = saleService;
            _masterService = masterService;
            _notifierSales = notifierSales;
            _notifierArticles = notifierArticles;
            _notifierClients = notifierClients;
            _masterClassService = masterClassService;
            _clientService = clientService;
        }



        [HttpPost("AddAsync")]
        public async Task<IActionResult> AddAsync([FromBody] Sale sale)
        {
            SaleResponse resultAdd = await _saleService.AddAsync(sale);

            if (resultAdd.Success)
            {
                List<int> articlesId = [];

                await _notifierSales.NotifyAsync(sale.Id, "Venta Creada", ChangeType.Created);
                await _notifierClients.NotifyAsync([sale.ClientId], "Venta Creada", ChangeType.Updated);
                foreach (var saleDetail in sale.SaleDetails)
                    articlesId.Add(saleDetail.ArticleId);
                if (articlesId.Count > 0)
                    await _notifierArticles.NotifyAsync(articlesId, "Venta Creada", ChangeType.Updated);


                if (!sale.GenerateInvoice)
                {
                    CommerceData? commerceData = await _masterClassService.GetCommerceDataAsync();
                    if (commerceData == null)
                        return BadRequest(new SaleResponse { Success = false, Message = "No se puede emitir la proforma porque los datos del comercio no se pueden leer" });
                    ClientViewModel? client = await _clientService.GetByIdAsync(sale.ClientId);
                    if (client == null)
                        return BadRequest(new SaleResponse { Success = false, Message = "No se puede emitir la proforma porque los datos del cliente no se pueden leer" });
                    IEnumerable<IvaCondition> ivaConditions = await _masterClassService.GetAllIvaConditionsAsync(true, false);
                    if (ivaConditions == null || ivaConditions.Count() == 0)
                        return BadRequest(new SaleResponse { Success = false, Message = "No se puede emitir la proforma porque los datos de condiciones de IVA no se pueden leer" });
                    IvaCondition? ivaCondition = ivaConditions.Where(ic => ic.Id == client.IvaConditionId).FirstOrDefault();
                    if (ivaCondition == null)
                        return BadRequest(new SaleResponse { Success = false, Message = "No se puede emitir la proforma porque los datos de condicion de IVA no se pueden leer" });
                    IEnumerable<Tax> taxes = await _masterClassService.GetAllTaxesAsync(true, false);

                    IEnumerable<SaleCondition> saleConditions = await _masterClassService.GetAllSaleConditionsAsync(true, false);
                    List<InvoiceReportViewModel> model = ToReportConverterHelper
                        .ToSaleReport(sale, commerceData, client, saleConditions.ToList(), ivaConditions.ToList(), taxes.ToList());
                    FacturaViewModel factura = new()
                    {
                        LogoByte = commerceData.LogoByteArray,
                        Leyenda = client.LegendBudget,
                    };
                    ReportClient reportClient = new();

                    ReportResponse reportResponse = await reportClient.GenerateSalePdfAsync(model, factura);
                    if (!reportResponse.Success)
                        return
                          BadRequest(new SaleResponse
                          {
                              Success = false,
                              Message = reportResponse.Message,
                          });
                    resultAdd.Bytes = reportResponse.Bytes;
                }
                return
                    Ok(resultAdd);
            }
            else return BadRequest(resultAdd);
        }



        [HttpPost("PrintAsync")]
        public async Task<IActionResult> PrintAsync([FromBody] SaleFilterDto filter)
        {
            if (filter.Id == 0)
                return BadRequest(new SaleResponse { Success = false, Message = "Debe informar la venta" });
            SaleResponse response = new() { Success = false };
            try
            {
                SaleResponse saleResponse = await _saleService.GetByIdAsync(filter.Id);
                if (!saleResponse.Success)
                    return BadRequest(saleResponse);
                if (saleResponse.SaleViewModel == null)
                    return BadRequest(new SaleResponse { Success = false, Message = "No se encontro la venta" });

                Sale sale = ConverterHelper.ToSale(saleResponse.SaleViewModel, false);


                CommerceData? commerceData = await _masterClassService.GetCommerceDataAsync();
                if (commerceData == null)
                    return BadRequest(new SaleResponse { Success = false, Message = "No se puede emitir la proforma porque los datos del comercio no se pueden leer" });
                ClientViewModel? client = await _clientService.GetByIdAsync(sale.ClientId);
                if (client == null)
                    return BadRequest(new SaleResponse { Success = false, Message = "No se puede emitir la proforma porque los datos del cliente no se pueden leer" });
                IEnumerable<IvaCondition> ivaConditions = await _masterClassService.GetAllIvaConditionsAsync(true, false);
                if (ivaConditions == null || ivaConditions.Count() == 0)
                    return BadRequest(new SaleResponse { Success = false, Message = "No se puede emitir la proforma porque los datos de condiciones de IVA no se pueden leer" });
                IvaCondition? ivaCondition = ivaConditions.Where(ic => ic.Id == client.IvaConditionId).FirstOrDefault();
                if (ivaCondition == null)
                    return BadRequest(new SaleResponse { Success = false, Message = "No se puede emitir la proforma porque los datos de condicion de IVA no se pueden leer" });
                IEnumerable<Tax> taxes = await _masterClassService.GetAllTaxesAsync(true, false);

                IEnumerable<SaleCondition> saleConditions = await _masterClassService.GetAllSaleConditionsAsync(true, false);
                List<InvoiceReportViewModel> model = ToReportConverterHelper
                    .ToSaleReport(sale, commerceData, client, saleConditions.ToList(), ivaConditions.ToList(), taxes.ToList());
                FacturaViewModel factura = new()
                {
                    LogoByte = commerceData.LogoByteArray,
                    Leyenda = client.LegendBudget,
                };
                ReportClient reportClient = new();

                ReportResponse reportResponse = await reportClient.GenerateSalePdfAsync(model, factura);
                if (!reportResponse.Success)
                    return
                      BadRequest(new SaleResponse
                      {
                          Success = false,
                          Message = reportResponse.Message,
                      });
                response.Bytes = reportResponse.Bytes;
                response.Success = true;
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                return BadRequest(response);
            }

        }



        [HttpPost("AnullAsync")]
        public async Task<IActionResult> AnullAsync([FromBody] SaleFilterDto filter)
        {
            if (filter.Id == 0)
                return BadRequest(new SaleResponse { Success = false, Message = "Debe informar la venta" });
            SaleResponse response = new() { Success = false };
            try
            {
                SaleResponse saleResponse = await _saleService.GetByIdAsync(filter.Id);
                if (!saleResponse.Success)
                    return BadRequest(saleResponse);
                if (saleResponse.SaleViewModel == null)
                    return BadRequest(new SaleResponse { Success = false, Message = "No se encontro la venta" });

                Sale sale = ConverterHelper.ToSale(saleResponse.SaleViewModel, false);


                CommerceData? commerceData = await _masterClassService.GetCommerceDataAsync();
                if (commerceData == null)
                    return BadRequest(new SaleResponse { Success = false, Message = "No se puede emitir la proforma porque los datos del comercio no se pueden leer" });
                ClientViewModel? client = await _clientService.GetByIdAsync(sale.ClientId);
                if (client == null)
                    return BadRequest(new SaleResponse { Success = false, Message = "No se puede emitir la proforma porque los datos del cliente no se pueden leer" });
                IEnumerable<IvaCondition> ivaConditions = await _masterClassService.GetAllIvaConditionsAsync(true, false);
                if (ivaConditions == null || ivaConditions.Count() == 0)
                    return BadRequest(new SaleResponse { Success = false, Message = "No se puede emitir la proforma porque los datos de condiciones de IVA no se pueden leer" });
                IvaCondition? ivaCondition = ivaConditions.Where(ic => ic.Id == client.IvaConditionId).FirstOrDefault();
                if (ivaCondition == null)
                    return BadRequest(new SaleResponse { Success = false, Message = "No se puede emitir la proforma porque los datos de condicion de IVA no se pueden leer" });
                IEnumerable<Tax> taxes = await _masterClassService.GetAllTaxesAsync(true, false);

                IEnumerable<SaleCondition> saleConditions = await _masterClassService.GetAllSaleConditionsAsync(true, false);


                SaleResponse resultAnull = await _saleService.AnullAsync(sale, filter.UserName);

                if (!resultAnull.Success)
                    return BadRequest(resultAnull);

                List<int> articlesId = [];

                await _notifierSales.NotifyAsync(sale.Id, "Venta Creada", ChangeType.Created);
                await _notifierClients.NotifyAsync([sale.ClientId], "Venta Creada", ChangeType.Updated);
                foreach (var saleDetail in sale.SaleDetails)
                    articlesId.Add(saleDetail.ArticleId);
                if (articlesId.Count > 0)
                    await _notifierArticles.NotifyAsync(articlesId, "Venta Creada", ChangeType.Updated);


                List<InvoiceReportViewModel> model = ToReportConverterHelper
                    .ToSaleReport(sale, commerceData, client, saleConditions.ToList(), ivaConditions.ToList(), taxes.ToList(),
                    "Nota de Crédito Proforma");
                FacturaViewModel factura = new()
                {
                    LogoByte = commerceData.LogoByteArray,
                    Leyenda = client.LegendBudget,
                };
                ReportClient reportClient = new();

                ReportResponse reportResponse = await reportClient.GenerateSalePdfAsync(model, factura);
                if (!reportResponse.Success)
                    return
                      BadRequest(new SaleResponse
                      {
                          Success = false,
                          Message = reportResponse.Message,
                      });
                response.Bytes = reportResponse.Bytes;
                response.Success = true;
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                return BadRequest(response);
            }

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
            SaleResponse saleResponse = await _saleService.GetAllAsync(filter.Page, filter.PageSize);
            return saleResponse.Success ? Ok(saleResponse) : BadRequest(saleResponse.Message);
        }


        [HttpPost("GetAllBySalePointAsync")]
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



        [HttpPost("GetByIdAsync")]
        public async Task<IActionResult> GetByIdAsync([FromBody] SaleFilterDto filter)
        {
            SaleResponse sale = await _saleService.GetByIdAsync(filter.Id);
            return sale.Success ? Ok(sale) : BadRequest(sale.Message);
        }

    }
}
