using CrystalDecisions.CrystalReports.Engine;
using Reports.Helpers;
using Reports.PublicServices.Interfaces;
using Reports.Responses;
using Reports.ViewModels;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Reports.PublicServices.Services
{
    public class InvoiceReport : IInvoiceReport
    {

        public InvoiceReport()
        {

        }




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
                        {
                            letraCbe = "A";
                            cbeRptName = "Factura A.rpt";
                            break;
                        }
                    case 2:
                        {
                            letraCbe = "A";
                            cbeRptName = "Factura A.rpt";
                            break;
                        }
                    case 3:
                        {
                            letraCbe = "A";
                            cbeRptName = "Factura A.rpt";
                            break;
                        }
                    case 4:
                        {
                            letraCbe = "A";
                            cbeRptName = "Recibo A.rpt";
                            break;
                        }
                    case 5:
                        {
                            letraCbe = "A";
                            cbeRptName = "Factura A.rpt";
                            break;
                        }
                    case 6:
                        {
                            letraCbe = "B";
                            cbeRptName = "Factura B.rpt";
                            break;
                        }
                    case 7:
                        {
                            letraCbe = "B";
                            cbeRptName = "Factura B.rpt";
                            break;
                        }
                    case 8:
                        {
                            letraCbe = "B";
                            cbeRptName = "Factura B.rpt";
                            break;
                        }
                    case 9:
                        {
                            letraCbe = "B";
                            cbeRptName = "Recibo B.rpt";
                            break;
                        }
                    case 10:
                        {
                            letraCbe = "B";
                            cbeRptName = "Factura B.rpt";
                            break;
                        }
                    case 11:
                        {
                            letraCbe = "C";
                            cbeRptName = "Factura B.rpt";
                            break;
                        }
                    case 12:
                        {
                            letraCbe = "C";
                            cbeRptName = "Factura B.rpt";
                            break;
                        }
                    case 13:
                        {
                            letraCbe = "C";
                            cbeRptName = "Factura B.rpt";
                            break;
                        }
                    case 15:
                        {
                            letraCbe = "C";
                            cbeRptName = "Recibo B.rpt";
                            break;
                        }
                    case 51:
                        {
                            letraCbe = "M";
                            cbeRptName = "Factura A.rpt";
                            break;
                        }
                    case 52:
                        {
                            letraCbe = "M";
                            cbeRptName = "Factura A.rpt";
                            break;
                        }
                    case 53:
                        {
                            letraCbe = "M";
                            cbeRptName = "Factura A.rpt";
                            break;
                        }
                    case 54:
                        {
                            letraCbe = "M";
                            cbeRptName = "Factura A.rpt";
                            break;
                        }
                    case 201:
                        {
                            letraCbe = "A";
                            cbeRptName = "MiPyMEs A.rpt";
                            break;
                        }
                    case 202:
                        {
                            letraCbe = "A";
                            cbeRptName = "MiPyMEs A.rpt";
                            break;
                        }
                    case 203:
                        {
                            letraCbe = "A";
                            cbeRptName = "MiPyMEs A.rpt";
                            break;
                        }
                    case 206:
                        {
                            letraCbe = "B";
                            cbeRptName = "MiPyMEs B.rpt";
                            break;
                        }
                    case 207:
                        {
                            letraCbe = "B";
                            cbeRptName = "MiPyMEs B.rpt";
                            break;
                        }
                    case 208:
                        {
                            letraCbe = "B";
                            cbeRptName = "MiPyMEs B.rpt";
                            break;
                        }
                    case 211:
                        {
                            letraCbe = "C";
                            cbeRptName = "MiPyMEs B.rpt";
                            break;
                        }
                    case 212:
                        {
                            letraCbe = "C";
                            cbeRptName = "MiPyMEs B.rpt";
                            break;
                        }
                    case 213:
                        {
                            letraCbe = "C";
                            cbeRptName = "MiPyMEs B.rpt";
                            break;
                        }
                    default:
                        {
                            break;
                        }
                }


                reportName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Reports", "Invoices", cbeRptName);
                PaperSize _paperSize = new PaperSize
                {
                    PaperName = "Custom",
                    RawKind = 9,
                };

                ReportDocument reportDocument = new ReportDocument();
                reportDocument.Load(reportName);

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
                             }).ToList();
                reportDocument.SetDataSource(query);

                Stream stream = reportDocument.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                stream.Seek(0, SeekOrigin.Begin);

                using (MemoryStream ms = new MemoryStream())
                {
                    await stream.CopyToAsync(ms);
                    response.Bytes = ms.ToArray();
                }

                response.Success = true;
                return response;
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                return response;
            }
        }
    }
}
