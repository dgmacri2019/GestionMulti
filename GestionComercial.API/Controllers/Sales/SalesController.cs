using Afip.PublicServices.Interfaces;
using GestionComercial.API.Security;
using GestionComercial.Applications.Interfaces;
using GestionComercial.Applications.Notifications;
using GestionComercial.Domain.DTOs.Client;
using GestionComercial.Domain.DTOs.Master.Configurations.Commerce;
using GestionComercial.Domain.DTOs.Sale;
using GestionComercial.Domain.Entities.Afip;
using GestionComercial.Domain.Entities.Masters;
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
        private readonly IMasterClassService _masterClassService;
        private readonly IClientService _clientService;
        private readonly ISalesNotifier _notifierSales;
        private readonly IArticlesNotifier _notifierArticles;
        private readonly IClientsNotifier _notifierClients;
        private readonly IWSFEHomologacionService _wSFEHomologacion;


        public SalesController(ISalesService saleService, IMasterService masterService,
            ISalesNotifier notifierSales, IArticlesNotifier notifierArticles, IClientsNotifier notifierClients,
            IMasterClassService masterClassService, IClientService clientService, IWSFEHomologacionService wSFEHomologacion)
        {
            _saleService = saleService;
            _masterService = masterService;
            _notifierSales = notifierSales;
            _notifierArticles = notifierArticles;
            _notifierClients = notifierClients;
            _masterClassService = masterClassService;
            _clientService = clientService;
            _wSFEHomologacion = wSFEHomologacion;
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

                return
                    Ok(resultAdd);
            }
            else return BadRequest(resultAdd);
        }

        [HttpPost("AddInvoiceAsync")]
        public async Task<IActionResult> AddInvoiceAsync([FromBody] SaleFilterDto filter)
        {
            try
            {
                SaleResponse saleResponse = await _saleService.GetByIdAsync(filter.Id);

                if (saleResponse.Success)
                {
                    SaleViewModel sale = saleResponse.SaleViewModel;

                    CommerceData? commerceData = await _masterClassService.GetCommerceDataAsync();
                    if (commerceData == null)
                        return BadRequest(new SaleResponse { Success = false, Message = "No se puede emitir la factura porque los datos del comercio no se pueden leer" });
                    ClientViewModel? client = await _clientService.GetByIdAsync(sale.ClientId);
                    if (client == null)
                        return BadRequest(new SaleResponse { Success = false, Message = "No se puede emitir la factura porque los datos del cliente no se pueden leer" });
                    BillingViewModel? billing = await _masterClassService.GetBillingAsync();
                    if (billing == null)
                        return BadRequest(new SaleResponse { Success = false, Message = "No se puede emitir la factura porque los datos del comercio (Facturación) no se pueden leer" });
                    IEnumerable<IvaCondition> ivaConditions = await _masterClassService.GetAllIvaConditionsAsync(true, false);
                    if (ivaConditions == null || ivaConditions.Count() == 0)
                        return BadRequest(new SaleResponse { Success = false, Message = "No se puede emitir la factura porque los datos de condiciones de IVA no se pueden leer" });
                    IvaCondition? ivaCondition = ivaConditions.Where(ic => ic.Id == client.IvaConditionId).FirstOrDefault();
                    if (ivaCondition == null)
                        return BadRequest(new SaleResponse { Success = false, Message = "No se puede emitir la factura porque los datos de condicion de IVA no se pueden leer" });
                    IEnumerable<DocumentType> documentTypes = await _masterClassService.GetAllDocumentTypesAsync(true, false);
                    if (documentTypes == null || documentTypes.Count() == 0)
                        return BadRequest(new SaleResponse { Success = false, Message = "No se puede emitir la factura porque los datos de tipos de documento no se pueden leer" });
                    DocumentType? documentType = documentTypes.Where(dt => dt.Id == client.DocumentTypeId).FirstOrDefault();
                    if (documentType == null)
                        return BadRequest(new SaleResponse { Success = false, Message = "No se puede emitir la factura porque los datos de tipo de documento no se pueden leer" });

                    if (!billing.UseWSDL)
                        return BadRequest(new SaleResponse { Success = false, Message = "El servicio de facturación electrónica no se encuentra habilitado.\n Debe habilitarlo desde la sección Configuracion - Datos Fiscales" });

                    int compTypeId;
                    if (commerceData.IvaConditionId == 1)
                    {
                        if (client.IvaConditionId == 1 || client.IvaConditionId == 2)
                            compTypeId = billing.EmitInvoiceM ? 51 : 1;
                        else
                            compTypeId = 6;
                    }
                    else if (commerceData.IvaConditionId == 2 || commerceData.IvaConditionId == 3)
                        compTypeId = 11;
                    else
                        return BadRequest(new SaleResponse { Success = false, Message = "No se puede emitir factura por la condición de iva declarada" });

                    DateTime date = sale.SaleDate.Date < DateTime.Now.Date.AddDays(-5) ? DateTime.Now : sale.SaleDate;

                    Invoice? invoice = await _saleService.FindInvoiceBySaleIdAsync(sale.Id, compTypeId);
                    if (invoice == null)
                    {
                        invoice = new()
                        {

                            CreateDate = sale.CreateDate,
                            CreateUser = sale.CreateUser,
                            IsDeleted = false,
                            IsEnabled = true,
                            Concepto = billing.Concept,
                            SaleId = sale.Id,
                            ClientId = sale.ClientId,
                            PtoVenta = sale.SalePoint,
                            ImpTotal = Convert.ToDouble(sale.Total),
                            ImpNeto = commerceData.IvaConditionId == 2 || commerceData.IvaConditionId == 3 ? Convert.ToDouble(sale.Total) : Convert.ToDouble(sale.SubTotal),
                            ImpTotalIVA = commerceData.IvaConditionId == 2 || commerceData.IvaConditionId == 3 ? 0 : Convert.ToDouble(sale.TotalIVA21 + sale.TotalIVA105 + sale.TotalIVA27),
                            ImpTotalConc = Convert.ToDouble(0),
                            CompTypeId = compTypeId,
                            ClientDocNro = Convert.ToInt64(client.DocumentNumber),
                            ClientDocType = documentType.AfipId,
                            ReceptorIvaId = ivaCondition.AfipId,
                            InvoiceDate = string.Format("{0:yyyyMMdd}", date),
                            ServDesde = string.Format("{0:yyyyMMdd}", date),
                            ServHasta = string.Format("{0:yyyyMMdd}", date),
                            VtoPago = string.Format("{0:yyyyMMdd}", date),
                            Cuit = commerceData.CUIT,
                            Alias = commerceData.Alias,
                            CBU = commerceData.CBU,
                            InternalTax = Convert.ToDouble(sale.InternalTax),
                            IvaConditionId = commerceData.IvaConditionId,
                            InvoiceDetails = commerceData.IvaConditionId == 2 || commerceData.IvaConditionId == 3? null : 
                            [
                                new InvoiceDetail
                                {
                                    CreateDate = sale.CreateDate,
                                    CreateUser = sale.CreateUser,
                                    IsDeleted = false,
                                    IsEnabled = true,
                                    IvaId = 4,
                                    BaseImpIva = Convert.ToDouble(sale.BaseImp105),
                                    ImporteIva = Convert.ToDouble(sale.TotalIVA105),
                                },
                                new InvoiceDetail
                                {
                                    CreateDate = sale.CreateDate,
                                    CreateUser = sale.CreateUser,
                                    IsDeleted = false,
                                    IsEnabled = true,
                                    IvaId = 5,
                                    BaseImpIva = Convert.ToDouble(sale.BaseImp21),
                                    ImporteIva = Convert.ToDouble(sale.TotalIVA21),
                                },
                                new InvoiceDetail
                                {
                                    CreateDate = sale.CreateDate,
                                    CreateUser = sale.CreateUser,
                                    IsDeleted = false,
                                    IsEnabled = true,
                                    IvaId = 6,
                                    BaseImpIva = Convert.ToDouble(sale.BaseImp27),
                                    ImporteIva = Convert.ToDouble(sale.TotalIVA27),
                                }
                                ],
                        };


                        //InvoiceResponse resultAfip = billing.UseHomologacion ?
                        //    await _wSFEHomologacion.SolicitarCAEAsync(invoice, 0)
                        //    :
                        //    await _wSFEProduccion.SolicitarCAEAsync(invoice, 0);

                        InvoiceResponse resultAfip = await _wSFEHomologacion.SolicitarCAEAsync(invoice, 0);

                        if (resultAfip.Success)
                        {
                            invoice.CAE = resultAfip.CAE;
                            invoice.CompNro = resultAfip.CompNro;
                            invoice.FechaVtoCAE = resultAfip.FechaVtoCAE;
                            invoice.FechaProceso = resultAfip.FechaProceso;

                            GeneralResponse invoiceAddResponse = await _masterService.AddAsync(invoice);

                            if (!invoiceAddResponse.Success)
                                return BadRequest(new SaleResponse { Success = false, Message = invoiceAddResponse.Message });
                            else
                                return Ok(new SaleResponse { Success = true, Message = "Factura generada correctamente" });
                        }
                        return BadRequest(new SaleResponse
                        {
                            Success = false,
                            Message = resultAfip.Message,
                        });

                    }
                    else
                        return BadRequest(new SaleResponse { Success = false, Message = "Ya se genero esa factura" });
                }
                else return BadRequest(new SaleResponse { Success = false, Message = $"No se encontro la venta que desea facturar el error fue:\n{saleResponse.Message}" });

            }
            catch (Exception ex)
            {
                return BadRequest(new SaleResponse
                {
                    Success = false,
                    Message = ex.Message,
                });
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
