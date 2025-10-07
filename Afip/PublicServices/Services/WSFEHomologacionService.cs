using Afip.PublicServices.Interfaces;
using GestionComercial.Applications.Interfaces;
using GestionComercial.Domain.Entities.Sales;
using GestionComercial.Domain.Response;
using System.Globalization;
using WSFEHomologacion;
using static WSFEHomologacion.ServiceSoapClient;

namespace Afip.PublicServices.Services
{
    public class WSFEHomologacionService : IWSFEHomologacionService
    {
        #region Attributes

        private readonly ILoginCMSHomologacionService _loginCMS;
        private readonly ISalesService _salesService;
        private readonly IInvoiceService _invoicesService;

        private readonly ServiceSoapClient _serviceSoapClient;

        #endregion


        #region Private Properties
        private FEAuthRequest? FEAuthRequest { get; set; }




        #endregion



        #region Contructor
        public WSFEHomologacionService(ILoginCMSHomologacionService loginCMS, ISalesService salesService, IInvoiceService invoicesService)
        {
            _loginCMS = loginCMS;
            _salesService = salesService;
            _serviceSoapClient = new ServiceSoapClient(EndpointConfiguration.ServiceSoap);
            _invoicesService = invoicesService;
        }

        #endregion

        #region Public Methods

        public async Task<InvoiceResponse> ConsultarComprobanteAsync(long cbteNro, int ptoVta, int cbteTipo)
        {
            try
            {
                InvoiceResponse response = new() { Success = false };

                FECompConsultarResponse? fECompConsultarResponse = await _serviceSoapClient.FECompConsultarAsync(FEAuthRequest, new FECompConsultaReq
                {
                    CbteNro = cbteNro,
                    PtoVta = ptoVta,
                    CbteTipo = cbteTipo,
                });

                if (fECompConsultarResponse.Body.FECompConsultarResult.Errors != null)
                {
                    bool first = true;
                    foreach (var error in fECompConsultarResponse.Body.FECompConsultarResult.Errors)
                    {
                        if (first)
                        {
                            response.Message += string.Format("Error: Código {0}, {1}", error.Code, error.Msg);
                            first = false;
                        }
                        else
                            response.Message += string.Format("\n Código {0}, {1}", error.Code, error.Msg);
                    }
                    response.Success = false;
                    return response;
                }

                if (fECompConsultarResponse.Body.FECompConsultarResult.Events != null)
                {
                    bool first = true;
                    foreach (var events in fECompConsultarResponse.Body.FECompConsultarResult.Events)
                    {
                        if (first)
                        {
                            response.Message += string.Format("Evento: Código {0}, {1}", events.Code, events.Msg);
                            first = false;
                        }
                        else
                            response.Message += string.Format("\n Código {0}, {1}", events.Code, events.Msg);
                    }
                    response.Success = false;
                    return response;
                }

                DateTime dt;
                bool convertDatetime = DateTime.TryParseExact(fECompConsultarResponse.Body.FECompConsultarResult.ResultGet.FchProceso, "yyyyMMddHHmmss", CultureInfo.InvariantCulture, DateTimeStyles.None, out dt);

                if (fECompConsultarResponse != null && fECompConsultarResponse.Body.FECompConsultarResult != null
                    && fECompConsultarResponse.Body.FECompConsultarResult.Errors == null
                    && fECompConsultarResponse.Body.FECompConsultarResult.Events == null)
                {
                    response.Invoice = new Invoice
                    {
                        Concepto = fECompConsultarResponse.Body.FECompConsultarResult.ResultGet.Concepto,
                        ClientDocType = fECompConsultarResponse.Body.FECompConsultarResult.ResultGet.DocTipo,
                        ClientDocNro = fECompConsultarResponse.Body.FECompConsultarResult.ResultGet.DocNro,
                        InvoiceDate = fECompConsultarResponse.Body.FECompConsultarResult.ResultGet.CbteFch,
                        PtoVenta = fECompConsultarResponse.Body.FECompConsultarResult.ResultGet.PtoVta,
                        CompNro = fECompConsultarResponse.Body.FECompConsultarResult.ResultGet.CbteDesde,
                        ImpTotal = fECompConsultarResponse.Body.FECompConsultarResult.ResultGet.ImpTotal,
                        ImpTotalConc = fECompConsultarResponse.Body.FECompConsultarResult.ResultGet.ImpTotConc,
                        ImpNeto = fECompConsultarResponse.Body.FECompConsultarResult.ResultGet.ImpNeto,
                        InternalTax = fECompConsultarResponse.Body.FECompConsultarResult.ResultGet.ImpTrib,
                        ImpTotalIVA = fECompConsultarResponse.Body.FECompConsultarResult.ResultGet.ImpIVA,
                        ServDesde = fECompConsultarResponse.Body.FECompConsultarResult.ResultGet.FchServDesde,
                        ServHasta = fECompConsultarResponse.Body.FECompConsultarResult.ResultGet.FchServHasta,
                        CAE = fECompConsultarResponse.Body.FECompConsultarResult.ResultGet.CodAutorizacion,
                        FechaVtoCAE = fECompConsultarResponse.Body.FECompConsultarResult.ResultGet.FchVto,
                        CompTypeId = fECompConsultarResponse.Body.FECompConsultarResult.ResultGet.CbteTipo,
                        FechaProceso = convertDatetime ? string.Format("{0:dd/MM/yyyy HH:mm:ss}", dt) : string.Empty,
                        ReceptorIvaId = fECompConsultarResponse.Body.FECompConsultarResult.ResultGet.CondicionIVAReceptorId,
                        VtoPago = fECompConsultarResponse.Body.FECompConsultarResult.ResultGet.FchVtoPago,
                    };
                }

                response.Success = true;
                return response;


            }
            catch (Exception ex)
            {
                return new InvoiceResponse
                {
                    Success = false,
                    Message = ex.Message,
                };
            }

        }

        public async Task<InvoiceResponse> GetLastCbteAsync(int ptoVta, int cbteTipo)
        {
            try
            {
                InvoiceResponse response = new() { Success = false };

                FECompUltimoAutorizadoResponse result = await _serviceSoapClient.FECompUltimoAutorizadoAsync(FEAuthRequest, ptoVta, cbteTipo);

                if (result.Body.FECompUltimoAutorizadoResult.Errors != null)
                {
                    bool first = true;
                    foreach (var error in result.Body.FECompUltimoAutorizadoResult.Errors)
                    {
                        if (first)
                        {
                            response.Message += string.Format("Error: Código {0}, {1}", error.Code, error.Msg);
                            first = false;
                        }
                        else
                            response.Message += string.Format("\n Código {0}, {1}", error.Code, error.Msg);
                    }
                    response.Success = false;
                    return response;
                }

                if (result.Body.FECompUltimoAutorizadoResult.Events != null)
                {
                    bool first = true;
                    foreach (var events in result.Body.FECompUltimoAutorizadoResult.Events)
                    {
                        if (first)
                        {
                            response.Message += string.Format("Evento: Código {0}, {1}", events.Code, events.Msg);
                            first = false;
                        }
                        else
                            response.Message += string.Format("\n Código {0}, {1}", events.Code, events.Msg);
                    }
                    response.Success = false;
                    return response;
                }

                response.LastCbte = result.Body.FECompUltimoAutorizadoResult.CbteNro;
                response.Success = true;
                return response;
            }
            catch (Exception ex)
            {
                return new InvoiceResponse
                {
                    Success = false,
                    Message = ex.Message,
                };
            }


        }

        public async Task<InvoiceResponse> SolicitarCAEAsync(Invoice invoice, int invoiceAnularId)
        {
            try
            {
                AfipLoginResponse resultLogin = await _loginCMS.LogInWSFEAsync();
                if (!resultLogin.Success)
                    return new InvoiceResponse
                    {
                        Success = false,
                        Message = resultLogin.Message
                    };
                FEAuthRequest = (FEAuthRequest)resultLogin.Object;

                InvoiceResponse response = new() { Success = false };
                //Datos de la respuesta
                FECAESolicitarResponse respuestaCae = new();
                //En la cabecera esta la respuesta de la solicitud
                FECAECabResponse cabecera = new();


                FECAEDetRequest detalles;

                Tributo[] tributo = new Tributo[1];
                List<AlicIva> alicIvas = [];

                FECAERequest feCAEReq = new()
                {
                    FeDetReq = new FECAEDetRequest[1],
                    FeCabReq = new FECAECabRequest
                    {
                        CantReg = 1,
                        CbteTipo = invoice.CompTypeId,
                        PtoVta = invoice.PtoVenta,
                    },
                };


                InvoiceResponse resultLastCbte = await GetLastCbteAsync(invoice.PtoVenta, invoice.CompTypeId);
                if (!resultLastCbte.Success)
                {
                    if (resultLastCbte.LastCbte == -1)
                        return new InvoiceResponse
                        {
                            Success = false,
                            ErrorCode = 101,
                            Message = "Error AFIP Numero de factura",
                        };
                    else
                        return resultLastCbte;
                }

                long cbteDesdeHasta = resultLastCbte.LastCbte + 1;
                if (invoice.InvoiceDetails != null)
                    foreach (InvoiceDetail invoiceDetail in invoice.InvoiceDetails)
                        if (invoiceDetail.ImporteIva > 0 || (invoiceDetail.IvaId == 3 && invoiceDetail.BaseImpIva > 0))
                        {
                            alicIvas.Add(new AlicIva
                            {
                                Id = invoiceDetail.IvaId,
                                BaseImp = invoiceDetail.BaseImpIva,
                                Importe = invoiceDetail.ImporteIva,
                            });
                        }

                if (invoice.InternalTax != 0)
                {
                    tributo[0] = new Tributo
                    {
                        Id = 4,
                        Alic = 0,
                        BaseImp = 0,
                        Importe = Convert.ToDouble(invoice.InternalTax),
                        Desc = "Impuestos Internos",
                    };
                }


                if (invoice.CompTypeId == 3 || invoice.CompTypeId == 8 || invoice.CompTypeId == 13 || invoice.CompTypeId == 53
                    || invoice.CompTypeId == 203 || invoice.CompTypeId == 208 || invoice.CompTypeId == 213)
                {
                    if (invoiceAnularId == 0)
                        return new InvoiceResponse
                        {
                            Success = false,
                            Message = "Debe informar la factura que desea anular",
                            ErrorCode = 101,
                        };
                    InvoiceResponse invoiceAnular = await _invoicesService.FindByIdAsync(invoiceAnularId);
                    if (!invoiceAnular.Success)
                        return invoiceAnular;
                    if (invoiceAnular.Invoice == null)
                        return new InvoiceResponse
                        {
                            Success = false,
                            Message = "No se encuentra la factura a anular",
                            ErrorCode = 101,
                        };

                    CbteAsoc[] cbteAsoc =
                    [
                        new CbteAsoc
                        {
                            CbteFch = invoiceAnular.Invoice.InvoiceDate,
                            Cuit = invoiceAnular.Invoice.Cuit.ToString(),
                            Nro = invoiceAnular.Invoice.CompNro,
                            PtoVta = invoiceAnular.Invoice.PtoVenta,
                            Tipo = invoiceAnular.Invoice.CompTypeId
                        },
                    ];
                    if (invoice.CompTypeId == 201 || invoice.CompTypeId == 206 || invoice.CompTypeId == 211)
                    {
                        detalles = new FECAEDetRequest
                        {
                            Concepto = invoice.Concepto,
                            DocTipo = invoice.ClientDocType,
                            DocNro = invoice.ClientDocNro,
                            CbteDesde = cbteDesdeHasta,
                            CbteHasta = cbteDesdeHasta,
                            CbteFch = invoice.InvoiceDate,
                            ImpTotal = invoice.ImpTotal,
                            ImpTotConc = invoice.ImpTotalConc,
                            ImpNeto = invoice.ImpNeto,
                            ImpOpEx = 0.00,
                            ImpTrib = 0.00,
                            ImpIVA = invoice.ImpTotalIVA,
                            FchServDesde = invoice.Concepto == 1 ? string.Empty : invoice.ServDesde,
                            FchServHasta = invoice.Concepto == 1 ? string.Empty : invoice.ServHasta,
                            FchVtoPago = invoice.Concepto == 1 ? string.Empty : invoice.VtoPago,
                            MonId = "PES",
                            MonCotiz = 1,
                            Tributos = null,
                            Iva = invoice.CompTypeId == 211 ? null : alicIvas.ToArray(),
                            CbtesAsoc = cbteAsoc,
                            CondicionIVAReceptorId = invoice.ReceptorIvaId,
                        };
                    }
                    else
                    {
                        detalles = new FECAEDetRequest
                        {
                            Concepto = invoice.Concepto,
                            DocTipo = invoice.ClientDocType,
                            DocNro = invoice.ClientDocNro,
                            CbteDesde = cbteDesdeHasta,
                            CbteHasta = cbteDesdeHasta,
                            CbteFch = invoice.InvoiceDate,
                            ImpTotal = invoice.ImpTotal,
                            ImpTotConc = invoice.ImpTotalConc,
                            ImpNeto = invoice.ImpNeto,
                            ImpOpEx = 0.00,
                            ImpTrib = 0.00,
                            ImpIVA = invoice.ImpTotalIVA,
                            FchServDesde = invoice.Concepto == 1 ? string.Empty : invoice.ServDesde,
                            FchServHasta = invoice.Concepto == 1 ? string.Empty : invoice.ServHasta,
                            FchVtoPago = invoice.Concepto == 1 ? string.Empty : invoice.VtoPago,
                            MonId = "PES",
                            MonCotiz = 1,
                            Tributos = null,
                            Iva = invoice.CompTypeId == 13 ? null : alicIvas.ToArray(),
                            CbtesAsoc = cbteAsoc,
                            CondicionIVAReceptorId = invoice.ReceptorIvaId,
                        };
                    }
                }
                else if (invoice.CompTypeId == 201 || invoice.CompTypeId == 206 || invoice.CompTypeId == 211)
                {
                    Opcional[] opcional =
                    [
                        new Opcional
                        {
                            Id = "2101",
                            Valor = invoice.CBU
                        },
                        new Opcional
                        {
                            Id = "2102",
                            Valor = invoice.Alias
                        },
                        new Opcional
                        {
                            Id = "27",
                            Valor = "SCA"
                        },
                    ];
                    detalles = new FECAEDetRequest
                    {
                        Concepto = invoice.Concepto,
                        DocTipo = invoice.ClientDocType,
                        DocNro = invoice.ClientDocNro,
                        CbteDesde = cbteDesdeHasta,
                        CbteHasta = cbteDesdeHasta,
                        CbteFch = invoice.InvoiceDate,
                        ImpTotal = invoice.ImpTotal,
                        ImpTotConc = invoice.ImpTotalConc,
                        ImpNeto = invoice.ImpNeto,
                        ImpOpEx = 0.00,
                        ImpTrib = 0.00,
                        ImpIVA = invoice.ImpTotalIVA,
                        FchServDesde = invoice.Concepto == 1 ? string.Empty : invoice.ServDesde,
                        FchServHasta = invoice.Concepto == 1 ? string.Empty : invoice.ServHasta,
                        FchVtoPago = invoice.Concepto == 1 ? string.Empty : invoice.VtoPago,
                        MonId = "PES",
                        MonCotiz = 1,
                        Tributos = null,
                        Iva = invoice.CompTypeId == 211 ? null : alicIvas.ToArray(),
                        Opcionales = opcional,
                        CondicionIVAReceptorId = invoice.ReceptorIvaId,
                    };
                }
                else
                {
                    detalles = new FECAEDetRequest
                    {
                        Concepto = invoice.Concepto,
                        DocTipo = invoice.ClientDocType,
                        DocNro = invoice.ClientDocNro,
                        CbteDesde = cbteDesdeHasta,
                        CbteHasta = cbteDesdeHasta,
                        CbteFch = invoice.InvoiceDate,
                        ImpTotal = invoice.ImpTotal,
                        ImpTotConc = invoice.ImpTotalConc,
                        ImpNeto = invoice.ImpNeto,
                        ImpOpEx = 0.00,
                        ImpTrib = 0.00,
                        ImpIVA = invoice.ImpTotalIVA,
                        FchServDesde = invoice.Concepto == 1 ? string.Empty : invoice.ServDesde,
                        FchServHasta = invoice.Concepto == 1 ? string.Empty : invoice.ServHasta,
                        FchVtoPago = invoice.Concepto == 1 ? string.Empty : invoice.VtoPago,
                        MonId = "PES",
                        MonCotiz = 1,
                        Tributos = null,
                        Iva = invoice.CompTypeId == 11 ? null : alicIvas.ToArray(),
                        CondicionIVAReceptorId = invoice.ReceptorIvaId,
                    };
                }



                feCAEReq.FeDetReq[0] = detalles;

                //Solicita CAE
                respuestaCae = await _serviceSoapClient.FECAESolicitarAsync(FEAuthRequest, feCAEReq);

                if (respuestaCae.Body.FECAESolicitarResult.FeCabResp != null && respuestaCae.Body.FECAESolicitarResult.FeCabResp.CantReg > 0)
                {
                    response.CAE = respuestaCae.Body.FECAESolicitarResult.FeDetResp[0].CAE;
                    response.FechaVtoCAE = respuestaCae.Body.FECAESolicitarResult.FeDetResp[0].CAEFchVto;
                    response.CompNro = respuestaCae.Body.FECAESolicitarResult.FeDetResp[0].CbteHasta;
                    response.FechaProceso = respuestaCae.Body.FECAESolicitarResult.FeCabResp.FchProceso;
                    response.Resultado = respuestaCae.Body.FECAESolicitarResult.FeDetResp[0].Resultado;
                }

                if (respuestaCae.Body.FECAESolicitarResult.FeDetResp[0].Observaciones != null
                    && respuestaCae.Body.FECAESolicitarResult.FeDetResp[0].Observaciones.Count() > 0)
                {
                    foreach (var observacion in respuestaCae.Body.FECAESolicitarResult.FeDetResp[0].Observaciones)
                        response.Message = $"Observacion:{observacion.Code}. {observacion.Msg}\n";
                }

                if (respuestaCae.Body.FECAESolicitarResult.Errors != null
                    && respuestaCae.Body.FECAESolicitarResult.Errors.Count() > 0)
                {
                    foreach (var error in respuestaCae.Body.FECAESolicitarResult.Errors)
                        response.Message = $"Error:{error.Code}. {error.Msg}\n";
                }


                if (respuestaCae.Body.FECAESolicitarResult.FeCabResp != null)
                {
                    cabecera.Cuit = respuestaCae.Body.FECAESolicitarResult.FeCabResp.Cuit;
                    cabecera.CantReg = respuestaCae.Body.FECAESolicitarResult.FeCabResp.CantReg;
                    cabecera.CbteTipo = respuestaCae.Body.FECAESolicitarResult.FeCabResp.CbteTipo;
                    cabecera.FchProceso = respuestaCae.Body.FECAESolicitarResult.FeCabResp.FchProceso;
                    cabecera.PtoVta = respuestaCae.Body.FECAESolicitarResult.FeCabResp.PtoVta;
                    cabecera.Resultado = respuestaCae.Body.FECAESolicitarResult.FeCabResp.Resultado;
                }

                response.Success = response.Resultado == "A";
                return response;
            }
            catch (Exception ex)
            {
                return new InvoiceResponse
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }


        #endregion


        #region Private Methods

        private async Task LoginWSFEAsync()
        {


        }

        #endregion

    }
}
