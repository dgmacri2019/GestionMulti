using CrystalDecisions.CrystalReports.Engine;
using GestionComercial.Contract.Responses;
using GestionComercial.Contract.ViewModels;
using GestionComercial.ReportServiceHost.Helpers;
using GestionComercial.Contract.Interfaces;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace GestionComercial.ReportServiceHost.Services
{
    public class ReportService : IReportService
    {
        public async Task<ReportResponse> GenerateInvoicePDFAsync(List<InvoiceReportViewModel> model, FacturaViewModel factura)
        {
            ReportResponse response = new ReportResponse { Success = false };
            try
            {
                string cbeRptName = string.Empty, letraCbe = string.Empty, pdfPath = string.Empty, name = string.Empty, reportName = string.Empty;

                ReportResponse resultGenerateQrCode = ImageHelper.GenerateQRCodeToByteArray(new QrDataViewModel
                {
                    ver = 1,
                    fecha = string.Format("{0:yyyy-MM-dd}", DateTime.ParseExact(factura.InvoiceDate, "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None)),
                    cuit = factura.Cuit,
                    ptoVta = factura.PtoVenta,
                    tipoCmp = factura.CompTypeId,
                    nroCmp = factura.CompNro,
                    importe = Convert.ToDecimal(factura.ImpTotal),
                    moneda = "PES",
                    ctz = 1,
                    tipoDocRec = factura.DocType,
                    nroDocRec = factura.DocNro,
                    tipoCodAut = "E",
                    codAut = Convert.ToInt64(factura.CAE),
                });
                if (!resultGenerateQrCode.Success)
                    return resultGenerateQrCode;

                byte[] qrCode = resultGenerateQrCode.Bytes;

                switch (factura.CompTypeId)
                {
                    case 1:                        
                    case 2:                        
                    case 3:                       
                    case 5:
                        letraCbe = "A";
                        cbeRptName = "Factura A.rpt";
                        break;
                    case 4:
                        letraCbe = "A";
                        cbeRptName = "Recibo A.rpt";
                        break;
                   
                   
                    case 6:                       
                    case 7:                       
                    case 8:                        
                    case 10:
                        letraCbe = "B";
                        cbeRptName = "Factura B.rpt";
                        break;
                    case 9:
                        letraCbe = "B";
                        cbeRptName = "Recibo B.rpt";
                        break;
                    


                    case 11:                       
                    case 12:                        
                    case 13:
                        letraCbe = "C";
                        cbeRptName = "Factura B.rpt";
                        break;
                    case 15:
                        letraCbe = "C";
                        cbeRptName = "Recibo B.rpt";
                        break;

                    case 51:
                    case 52:
                    case 53:
                    case 54:
                        letraCbe = "M";
                        cbeRptName = "Factura A.rpt";
                        break;

                    case 201:
                    case 202:
                    case 203:
                        letraCbe = "A";
                        cbeRptName = "MiPyMEs A.rpt";
                        break;

                    case 206:
                    case 207:
                    case 208:
                        letraCbe = "B";
                        cbeRptName = "MiPyMEs B.rpt";
                        break;


                    case 211:
                    case 212:
                    case 213:
                        letraCbe = "C";
                        cbeRptName = "MiPyMEs B.rpt";
                        break;
                    

                    default:
                        letraCbe = "B";
                        cbeRptName = "Factura B.rpt";
                        break;
                }

                reportName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Reports", "Invoices", cbeRptName);
                PaperSize _paperSize = new PaperSize
                {
                    PaperName = "Custom",
                    RawKind = 9,
                };

                var query = (from r in model.ToList()
                             select new
                             {
                                 r.CAE,
                                 r.Cantidad,
                                 r.CdoCbe,
                                 r.CondicionIvaR,
                                 r.CondicionVenta,
                                 r.CuitE,
                                 r.CuitR,
                                 r.Descripcion,
                                 r.DireccionE,
                                 r.DireccionR,
                                 r.EmailE,
                                 r.EmailR,
                                 r.FechaDesde,
                                 r.FechaEmision,
                                 r.FechaHasta,
                                 r.FechaInicio,
                                 r.FechaVtoCAE,
                                 r.FechaVtoPago,
                                 r.IIBB,
                                 r.Iva0,
                                 r.Iva105,
                                 r.Iva21,
                                 r.Iva25,
                                 r.Iva27,
                                 r.Iva5,
                                 r.NombreCbe,
                                 r.NroCbe,
                                 r.PrecioUni,
                                 r.PtoVenta,
                                 r.RazonSocialE,
                                 r.RazonSocialR,
                                 r.SubTotal,
                                 r.SubTotalItem,
                                 r.TelefonoE,
                                 r.TelefonoR,
                                 r.Total,
                                 r.CBU,
                                 r.Alias,
                                 r.Ajuste,
                                 r.CondicionIvaE,
                                 r.IvaTotal,
                                 LetraCbe = letraCbe,
                                 r.DiscountValue,
                                 r.DiscountText,
                                 QrBytes = qrCode,
                                 factura.LogoByte,
                                 factura.Leyenda,
                             }).ToList();

                // 📌 Generar reporte
                using (var reportDocument = new ReportDocument())
                {
                    reportDocument.Load(reportName);
                    reportDocument.SetDataSource(query);

                    using (var stream = reportDocument.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat))
                    using (var ms = new MemoryStream())
                    {
                        await stream.CopyToAsync(ms);
                        response.Bytes = ms.ToArray();
                    }
                }
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
            }

            return response;

        }
    
        public async Task<ReportResponse> GenerateSalePDFAsync(List<InvoiceReportViewModel> model, FacturaViewModel factura)
        {
            ReportResponse response = new ReportResponse { Success = false };
            try
            {
                string cbeRptName = "Proforma.rpt", letraCbe = "X", pdfPath = string.Empty, name = string.Empty, reportName = string.Empty;
      
               
                reportName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Reports", "Sales", cbeRptName);
                PaperSize _paperSize = new PaperSize
                {
                    PaperName = "Custom",
                    RawKind = 9,
                };

                var query = (from r in model.ToList()
                             select new
                             {
                                 r.CAE,
                                 r.Cantidad,
                                 r.CdoCbe,
                                 r.CondicionIvaR,
                                 r.CondicionVenta,
                                 r.CuitE,
                                 r.CuitR,
                                 r.Descripcion,
                                 r.DireccionE,
                                 r.DireccionR,
                                 r.EmailE,
                                 r.EmailR,
                                 r.FechaDesde,
                                 r.FechaEmision,
                                 r.FechaHasta,
                                 r.FechaInicio,
                                 r.FechaVtoCAE,
                                 r.FechaVtoPago,
                                 r.IIBB,
                                 r.Iva0,
                                 r.Iva105,
                                 r.Iva21,
                                 r.Iva25,
                                 r.Iva27,
                                 r.Iva5,
                                 r.NombreCbe,
                                 r.NroCbe,
                                 r.PrecioUni,
                                 r.PtoVenta,
                                 r.RazonSocialE,
                                 r.RazonSocialR,
                                 r.SubTotal,
                                 r.SubTotalItem,
                                 r.TelefonoE,
                                 r.TelefonoR,
                                 r.Total,
                                 r.CBU,
                                 r.Alias,
                                 r.Ajuste,
                                 r.CondicionIvaE,
                                 r.IvaTotal,
                                 LetraCbe = letraCbe,
                                 r.DiscountValue,
                                 r.DiscountText,
                                 factura.LogoByte,
                                 factura.Leyenda,
                             }).ToList();

                // 📌 Generar reporte
                using (var reportDocument = new ReportDocument())
                {
                    reportDocument.Load(reportName);
                    reportDocument.SetDataSource(query);

                    using (var stream = reportDocument.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat))
                    using (var ms = new MemoryStream())
                    {
                        await stream.CopyToAsync(ms);
                        response.Bytes = ms.ToArray();
                    }
                }
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
            }

            return response;

        }
    }
}
