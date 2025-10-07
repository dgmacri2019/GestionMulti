using Afip.PublicServices.Interfaces;
using GestionComercial.API.Helpers;
using GestionComercial.API.NamedPipe;
using GestionComercial.API.Security;
using GestionComercial.Applications.Interfaces;
using GestionComercial.Applications.Notifications;
using GestionComercial.Contract.Responses;
using GestionComercial.Contract.ViewModels;
using GestionComercial.Domain.DTOs.Client;
using GestionComercial.Domain.DTOs.Master.Configurations.Commerce;
using GestionComercial.Domain.DTOs.Sale;
using GestionComercial.Domain.Entities.Afip;
using GestionComercial.Domain.Entities.Masters;
using GestionComercial.Domain.Entities.Sales;
using GestionComercial.Domain.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static GestionComercial.Domain.Constant.Enumeration;

namespace GestionComercial.API.Controllers.Sales
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    [AuthorizePermission]
    public class InvoicesController : ControllerBase
    {
        private readonly IWSFEHomologacionService _wSFEHomologacion;
        private readonly IInvoicesNotifier _notifierInvoices;
        private readonly IInvoiceService _invoiceService;
        private readonly IMasterService _masterService;
        private readonly IMasterClassService _masterClassService;
        private readonly IClientService _clientService;
        private readonly ISalesService _saleService;


        public InvoicesController(IWSFEHomologacionService wSFEHomologacionService, IInvoicesNotifier notifierInvoices,
            IInvoiceService invoiceService, IMasterService masterService, IMasterClassService masterClassService,
            IClientService clientService, ISalesService saleService)
        {
            _wSFEHomologacion = wSFEHomologacionService;
            _notifierInvoices = notifierInvoices;
            _invoiceService = invoiceService;
            _masterService = masterService;
            _masterClassService = masterClassService;
            _clientService = clientService;
            _saleService = saleService;
        }

        [HttpPost("AddAsync")]
        public async Task<IActionResult> AddAsync([FromBody] SaleFilterDto filter)
        {
            try
            {
                SaleResponse saleResponse = await _saleService.GetByIdAsync(filter.Id);

                if (saleResponse.Success)
                {
                    SaleViewModel sale = saleResponse.SaleViewModel;

                    CommerceData? commerceData = await _masterClassService.GetCommerceDataAsync();
                    if (commerceData == null)
                        return BadRequest(new InvoiceResponse { Success = false, Message = "No se puede emitir la factura porque los datos del comercio no se pueden leer" });
                    ClientViewModel? client = await _clientService.GetByIdAsync(sale.ClientId);
                    if (client == null)
                        return BadRequest(new InvoiceResponse { Success = false, Message = "No se puede emitir la factura porque los datos del cliente no se pueden leer" });
                    BillingViewModel? billing = await _masterClassService.GetBillingAsync();
                    if (billing == null)
                        return BadRequest(new InvoiceResponse { Success = false, Message = "No se puede emitir la factura porque los datos del comercio (Facturación) no se pueden leer" });
                    IEnumerable<IvaCondition> ivaConditions = await _masterClassService.GetAllIvaConditionsAsync(true, false);
                    if (ivaConditions == null || ivaConditions.Count() == 0)
                        return BadRequest(new InvoiceResponse { Success = false, Message = "No se puede emitir la factura porque los datos de condiciones de IVA no se pueden leer" });
                    IvaCondition? ivaCondition = ivaConditions.Where(ic => ic.Id == client.IvaConditionId).FirstOrDefault();
                    if (ivaCondition == null)
                        return BadRequest(new InvoiceResponse { Success = false, Message = "No se puede emitir la factura porque los datos de condicion de IVA no se pueden leer" });
                    IEnumerable<DocumentType> documentTypes = await _masterClassService.GetAllDocumentTypesAsync(true, false);
                    if (documentTypes == null || documentTypes.Count() == 0)
                        return BadRequest(new InvoiceResponse { Success = false, Message = "No se puede emitir la factura porque los datos de tipos de documento no se pueden leer" });
                    DocumentType? documentType = documentTypes.Where(dt => dt.Id == client.DocumentTypeId).FirstOrDefault();
                    if (documentType == null)
                        return BadRequest(new InvoiceResponse { Success = false, Message = "No se puede emitir la factura porque los datos de tipo de documento no se pueden leer" });

                    if (!billing.UseWSDL)
                        return BadRequest(new InvoiceResponse { Success = false, Message = "El servicio de facturación electrónica no se encuentra habilitado.\n Debe habilitarlo desde la sección Configuracion - Datos Fiscales" });
                    IEnumerable<Tax> taxes = await _masterClassService.GetAllTaxesAsync(true, false);
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
                        return BadRequest(new InvoiceResponse { Success = false, Message = "No se puede emitir factura por la condición de iva declarada" });

                    DateTime date = sale.SaleDate.Date < DateTime.Now.Date.AddDays(-5) ? DateTime.Now : sale.SaleDate;

                    InvoiceResponse invoiceResponse = await _invoiceService.FindBySaleIdAsync(sale.Id, compTypeId);
                    if (!invoiceResponse.Success)
                        return BadRequest(new InvoiceResponse { Success = false, Message = invoiceResponse.Message });
                    Invoice invoice;
                    if (invoiceResponse.Invoice == null)
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
                            ImpTotalIVA = commerceData.IvaConditionId == 2 || commerceData.IvaConditionId == 3 ? 0 : Convert.ToDouble(sale.TotalIVA21 + sale.TotalIVA105 + sale.TotalIVA27 + sale.TotalIVA25 + sale.TotalIVA5),
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
                            InvoiceDetails = commerceData.IvaConditionId == 2 || commerceData.IvaConditionId == 3 ? null :
                            [
                                new InvoiceDetail
                                {
                                    CreateDate = sale.CreateDate,
                                    CreateUser = sale.CreateUser,
                                    IsDeleted = false,
                                    IsEnabled = true,
                                    IvaId = 3,
                                    BaseImpIva = Convert.ToDouble(sale.BaseImp0),
                                    ImporteIva = Convert.ToDouble(0),
                                },
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
                                },
                                new InvoiceDetail
                                {
                                    CreateDate = sale.CreateDate,
                                    CreateUser = sale.CreateUser,
                                    IsDeleted = false,
                                    IsEnabled = true,
                                    IvaId = 8,
                                    BaseImpIva = Convert.ToDouble(sale.BaseImp5),
                                    ImporteIva = Convert.ToDouble(sale.TotalIVA5),
                                },
                                new InvoiceDetail
                                {
                                    CreateDate = sale.CreateDate,
                                    CreateUser = sale.CreateUser,
                                    IsDeleted = false,
                                    IsEnabled = true,
                                    IvaId = 9,
                                    BaseImpIva = Convert.ToDouble(sale.BaseImp25),
                                    ImporteIva = Convert.ToDouble(sale.TotalIVA25),
                                }
                            ],
                        };

                        GeneralResponse invoiceAddResponse = await _masterService.AddAsync(invoice);

                        if (!invoiceAddResponse.Success)
                            return BadRequest(new InvoiceResponse { Success = false, Message = invoiceAddResponse.Message });

                        await _notifierInvoices.NotifyAsync(invoice.Id, "Factura Creada", ChangeType.Created);
                    }
                    else
                        invoice = invoiceResponse.Invoice;
                    //InvoiceResponse resultAfip = billing.UseHomologacion ?
                    //    await _wSFEHomologacion.SolicitarCAEAsync(invoice, 0)
                    //    :
                    //    await _wSFEProduccion.SolicitarCAEAsync(invoice, 0);

                    if (string.IsNullOrEmpty(invoice.CAE))
                    {
                        InvoiceResponse resultAfip = await _wSFEHomologacion.SolicitarCAEAsync(invoice, 0);

                        if (resultAfip.Success)
                        {
                            invoice.CAE = resultAfip.CAE;
                            invoice.CompNro = resultAfip.CompNro;
                            invoice.FechaVtoCAE = resultAfip.FechaVtoCAE;
                            invoice.FechaProceso = resultAfip.FechaProceso;

                            GeneralResponse resultUpdateInvoice = await _masterService.UpdateAsync(invoice);
                            if (!resultUpdateInvoice.Success)
                                return BadRequest(new InvoiceResponse { Success = false, Message = resultUpdateInvoice.Message });
                            await _notifierInvoices.NotifyAsync(invoice.Id, "Factura Actualizada", ChangeType.Updated);
                        }
                        else
                            return BadRequest(new InvoiceResponse
                            {
                                Success = false,
                                Message = resultAfip.Message,
                            });
                    }

                    IEnumerable<SaleCondition> saleConditions = await _masterClassService.GetAllSaleConditionsAsync(true, false);

                    List<InvoiceReportViewModel> model = ToReportConverterHelper
                        .ToInvoiceReport(sale, invoice, commerceData, client, saleConditions.ToList(), ivaConditions.ToList(), taxes.ToList());
                    FacturaViewModel factura = new()
                    {
                        CAE = invoice.CAE,
                        CompNro = invoice.CompNro,
                        CompTypeId = invoice.CompTypeId,
                        Cuit = invoice.Cuit,
                        DocNro = invoice.ClientDocNro,
                        DocType = invoice.ClientDocType,
                        ImpTotal = invoice.ImpTotal,
                        InvoiceDate = invoice.InvoiceDate,
                        PtoVenta = invoice.PtoVenta,
                        LogoByte = commerceData.LogoByteArray,
                        Leyenda = client.LegendInvoices,
                    };
                    ReportClient reportClient = new();

                    ReportResponse reportResponse = await reportClient.GenerateInvoicePdfAsync(model, factura);

                    return reportResponse.Success ?
                        Ok(new InvoiceResponse
                        {
                            Success = true,
                            Message = "Factura generada correctamente",
                            Bytes = reportResponse.Bytes
                        })
                        :
                        BadRequest(new InvoiceResponse
                        {
                            Success = false,
                            Message = reportResponse.Message,
                        });





                    // else
                    //     return BadRequest(new SaleResponse { Success = false, Message = "Ya se genero esa factura" });
                }
                else return BadRequest(new InvoiceResponse { Success = false, Message = $"No se encontro la venta que desea facturar el error fue:\n{saleResponse.Message}" });

            }
            catch (Exception ex)
            {
                return BadRequest(new InvoiceResponse
                {
                    Success = false,
                    Message = ex.Message,
                });
            }
        }


        [HttpPost("GetAllAsync")]
        public async Task<IActionResult> GetAllAsync([FromBody] SaleFilterDto filter)
        {
            InvoiceResponse invoiceResponse = await _invoiceService.GetAllAsync(filter.Page, filter.PageSize);
            return invoiceResponse.Success ? Ok(invoiceResponse) : BadRequest(invoiceResponse.Message);
        }


        [HttpPost("GetAllBySalePointAsync")]
        public async Task<IActionResult> GetAllBySalePointAsync([FromBody] SaleFilterDto filter)
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



        [HttpPost("GetByIdAsync")]
        public async Task<IActionResult> GetByIdAsync([FromBody] SaleFilterDto filter)
        {
            InvoiceResponse invoice = await _invoiceService.FindByIdAsync(filter.Id);
            return invoice.Success ? Ok(invoice) : BadRequest(invoice.Message);
        }
    }
}
