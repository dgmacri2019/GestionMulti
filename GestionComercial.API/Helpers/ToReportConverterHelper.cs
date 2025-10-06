using GestionComercial.Contract.ViewModels;
using GestionComercial.Domain.DTOs.Client;
using GestionComercial.Domain.DTOs.Sale;
using GestionComercial.Domain.Entities.Afip;
using GestionComercial.Domain.Entities.Masters;
using GestionComercial.Domain.Entities.Sales;
using System.Globalization;

namespace GestionComercial.API.Helpers
{
    internal class ToReportConverterHelper
    {
        internal static List<InvoiceReportViewModel> ToInvoiceReport(SaleViewModel sale, Invoice invoice, CommerceData commerceData,
            ClientViewModel client, List<SaleCondition> saleConditions, List<IvaCondition> ivaConditions, List<Tax> taxes)
        {
            try
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo("es-AR");
                var response = new List<InvoiceReportViewModel>();
                string nombreCbe = string.Empty, discountText = "Bonif.:";
                decimal discountValue = 0m;

                bool ivaDiscriminado = commerceData.IvaConditionId == 1
               && (client.IvaConditionId == 1 || client.IvaConditionId == 2);

                if (!ivaDiscriminado)
                    sale.SubTotal = sale.SaleDetails.Sum(s => s.TotalItem);

                if (invoice.CompTypeId == 1 || invoice.CompTypeId == 6 || invoice.CompTypeId == 11 || invoice.CompTypeId == 51)
                    nombreCbe = "Factura";
                else if (invoice.CompTypeId == 201 || invoice.CompTypeId == 206 || invoice.CompTypeId == 211)
                    nombreCbe = "Factura de Crédito MiPyMEs";
                else if (invoice.CompTypeId == 2 || invoice.CompTypeId == 7 || invoice.CompTypeId == 12 || invoice.CompTypeId == 52)
                    nombreCbe = "Nota de Débito";
                else if (invoice.CompTypeId == 202 || invoice.CompTypeId == 207 || invoice.CompTypeId == 212)
                    nombreCbe = "Nota de Débito MiPyMEs";
                else if (invoice.CompTypeId == 3 || invoice.CompTypeId == 8 || invoice.CompTypeId == 13 || invoice.CompTypeId == 53)
                    nombreCbe = "Nota de Crédito";
                else if (invoice.CompTypeId == 203 || invoice.CompTypeId == 208 || invoice.CompTypeId == 213)
                    nombreCbe = "Nota de Crédito MiPyMEs";

                if (sale.GeneralDiscount != 0)
                {
                    discountText += string.Format(" {0:N0}%", sale.GeneralDiscount);
                    discountValue = sale.SubTotal * sale.GeneralDiscount / 100;
                }
                double iva0 = 0, iva25 = 0, iva5 = 0, iva105 = 0, iva21 = 0, iva27 = 0;

                // Detalle de los IVA
                if (invoice.InvoiceDetails != null)
                    foreach (var ivaDetail in invoice.InvoiceDetails)
                        switch (ivaDetail.IvaId)
                        {
                            case 3:
                                iva0 = ivaDetail.ImporteIva;
                                break;
                            case 4:
                                iva105 = ivaDetail.ImporteIva;
                                break;
                            case 5:
                                iva21 = ivaDetail.ImporteIva;
                                break;
                            case 6:
                                iva27 = ivaDetail.ImporteIva;
                                break;
                            case 8:
                                iva5 = ivaDetail.ImporteIva;
                                break;
                            case 9:
                                iva25 = ivaDetail.ImporteIva;
                                break;
                            default:
                                break;
                        }



                response.Add(new InvoiceReportViewModel
                {
                    IvaTotal = string.Format("{0:C2}", invoice.ImpTotalIVA),
                    DiscountText = discountText,
                    DiscountValue = string.Format("{0:C2}", discountValue),
                    CBU = invoice.CBU,
                    Alias = invoice.Alias,
                    CuitE = invoice.Cuit.ToString(),
                    RazonSocialE = commerceData.BusinessName,
                    IIBB = commerceData.IIBB,
                    EmailE = commerceData.Email,
                    DireccionE = commerceData.Address,
                    CondicionIvaE = ivaConditions.FirstOrDefault(iv => iv.Id == invoice.IvaConditionId).Description,
                    FechaInicio = string.Format("{0:dd/MM/yyyy}", commerceData.ActivityStartDate),
                    TelefonoE = commerceData.Phone,
                    RazonSocialR = client.BusinessName,
                    CuitR = invoice.ClientDocNro.ToString(),
                    DireccionR = client.Address,
                    TelefonoR = client.Phone,
                    CondicionIvaR = ivaConditions.FirstOrDefault(iv => iv.Id == client.IvaConditionId).Description,
                    EmailR = client.Email,
                    CondicionVenta = sale.PaidOut ? "Contado" : "Cuenta Corriente",
                    PtoVenta = invoice.PtoVenta.ToString(),
                    NroCbe = invoice.CompNro.ToString(),
                    FechaDesde = !string.IsNullOrEmpty(invoice.ServDesde) ? string.Format("{0:dd/MM/yyyy}", DateTime.ParseExact(invoice.ServDesde, "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None)) : string.Format("{0:dd/MM/yyyy}", DateTime.Now),
                    FechaHasta = !string.IsNullOrEmpty(invoice.ServHasta) ? string.Format("{0:dd/MM/yyyy}", DateTime.ParseExact(invoice.ServHasta, "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None)) : string.Format("{0:dd/MM/yyyy}", DateTime.Now),
                    FechaVtoPago = !string.IsNullOrEmpty(invoice.VtoPago) ? string.Format("{0:dd/MM/yyyy}", DateTime.ParseExact(invoice.VtoPago, "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None)) : string.Format("{0:dd/MM/yyyy}", DateTime.Now),
                    SubTotal = string.Format("{0:C2}", sale.SubTotal),
                    Total = string.Format("{0:C2}", invoice.ImpTotal),
                    Ajuste = string.Format("{0:C2}", invoice.Ajust),
                    CdoCbe = invoice.CompTypeId.ToString(),
                    NombreCbe = nombreCbe,
                    FechaEmision = string.Format("{0:dd/MM/yyyy}", DateTime.ParseExact(invoice.InvoiceDate, "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None)),
                    CAE = invoice.CAE,
                    FechaVtoCAE = string.Format("{0:dd/MM/yyyy}", DateTime.ParseExact(invoice.FechaVtoCAE, "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None)),
                    Leyenda = client.LegendInvoices,
                    Iva0 = string.Format("{0:C2}", iva0),
                    Iva105 = string.Format("{0:C2}", iva105),
                    Iva21 = string.Format("{0:C2}", iva21),
                    Iva25 = string.Format("{0:C2}", iva25),
                    Iva27 = string.Format("{0:C2}", iva27),
                    Iva5 = string.Format("{0:C2}", iva5),
                });


                // Detalle de los items

                foreach (var itemDetail in sale.SaleDetails)
                {
                    decimal price = 0, subtotal = 0, total = 0;
                    if (!ivaDiscriminado)
                    {
                        Tax tax = taxes.Where(t => t.Id == itemDetail.TaxId).FirstOrDefault();
                        price = itemDetail.Price + (itemDetail.Price * tax.Rate / 100);
                    }
                    else
                        price = itemDetail.Price;


                    subtotal = price * itemDetail.Quantity;
                    total = subtotal - (subtotal * itemDetail.Discount / 100);

                    response.Add(new InvoiceReportViewModel
                    {
                        IvaTotal = string.Format("{0:C2}", invoice.ImpTotalIVA),
                        DiscountText = discountText,
                        DiscountValue = string.Format("{0:C2}", discountValue),
                        Cantidad = itemDetail.Quantity.ToString(),
                        Descripcion = itemDetail.Description,
                        PrecioUni = string.Format("{0:C2}", price),
                        SubTotalItem = string.Format("{0:C2}", subtotal),
                        CBU = invoice.CBU,
                        Alias = invoice.Alias,
                        CuitE = invoice.Cuit.ToString(),
                        RazonSocialE = commerceData.BusinessName,
                        IIBB = commerceData.IIBB,
                        EmailE = commerceData.Email,
                        DireccionE = commerceData.Address,
                        FechaInicio = string.Format("{0:dd/MM/yyyy}", commerceData.ActivityStartDate),
                        TelefonoE = commerceData.Phone,
                        RazonSocialR = client.BusinessName,
                        CuitR = invoice.ClientDocNro.ToString(),
                        DireccionR = client.Address,
                        TelefonoR = client.Phone,
                        CondicionIvaR = ivaConditions.FirstOrDefault(iv => iv.Id == invoice.IvaConditionId).Description,
                        EmailR = client.Email,
                        CondicionVenta = sale.PaidOut ? "Contado" : "Cuenta Corriente",
                        PtoVenta = invoice.PtoVenta.ToString(),
                        NroCbe = invoice.CompNro.ToString(),
                        FechaDesde = !string.IsNullOrEmpty(invoice.ServDesde) ? string.Format("{0:dd/MM/yyyy}", DateTime.ParseExact(invoice.ServDesde, "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None)) : string.Format("{0:dd/MM/yyyy}", DateTime.Now),
                        FechaHasta = !string.IsNullOrEmpty(invoice.ServHasta) ? string.Format("{0:dd/MM/yyyy}", DateTime.ParseExact(invoice.ServHasta, "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None)) : string.Format("{0:dd/MM/yyyy}", DateTime.Now),
                        FechaVtoPago = !string.IsNullOrEmpty(invoice.VtoPago) ? string.Format("{0:dd/MM/yyyy}", DateTime.ParseExact(invoice.VtoPago, "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None)) : string.Format("{0:dd/MM/yyyy}", DateTime.Now),
                        SubTotal = string.Format("{0:C2}", sale.SubTotal),
                        Total = string.Format("{0:C2}", invoice.ImpTotal),
                        Ajuste = string.Format("{0:C2}", invoice.Ajust),
                        CdoCbe = invoice.CompTypeId.ToString(),
                        NombreCbe = nombreCbe,
                        FechaEmision = string.Format("{0:dd/MM/yyyy}", DateTime.ParseExact(invoice.InvoiceDate, "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None)),
                        CAE = invoice.CAE,
                        FechaVtoCAE = string.Format("{0:dd/MM/yyyy}", DateTime.ParseExact(invoice.FechaVtoCAE, "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None)),
                        Iva0 = string.Format("{0:C2}", iva0),
                        Iva105 = string.Format("{0:C2}", iva105),
                        Iva21 = string.Format("{0:C2}", iva21),
                        Iva25 = string.Format("{0:C2}", iva25),
                        Iva27 = string.Format("{0:C2}", iva27),
                        Iva5 = string.Format("{0:C2}", iva5),
                        Leyenda = client.LegendInvoices,
                    });
                    if (itemDetail.Discount != 0)
                    {
                        string dicount = string.Format("{0:C2}", subtotal * itemDetail.Discount / 100);
                        string text = "Descuento";
                        string value = string.Format("{0}: {1}%", text, itemDetail.Discount);

                        response.Add(new InvoiceReportViewModel
                        {
                            IvaTotal = string.Format("{0:C2}", invoice.ImpTotalIVA),
                            DiscountText = discountText,
                            DiscountValue = string.Format("{0:C2}", discountValue),
                            Cantidad = "1",
                            Descripcion = value,
                            PrecioUni = dicount,    //,string.Format("{0:C2}", itemDetail.Price),
                            SubTotalItem = dicount, //string.Format("{0:C2}", itemDetail.SubTotal),
                            CBU = invoice.CBU,
                            Alias = invoice.Alias,
                            CuitE = invoice.Cuit.ToString(),
                            RazonSocialE = commerceData.BusinessName,
                            IIBB = commerceData.IIBB,
                            EmailE = commerceData.Email,
                            DireccionE = commerceData.Address,
                            FechaInicio = string.Format("{0:dd/MM/yyyy}", commerceData.ActivityStartDate),
                            TelefonoE = commerceData.Phone,
                            RazonSocialR = client.BusinessName,
                            CuitR = invoice.ClientDocNro.ToString(),
                            DireccionR = client.Address,
                            TelefonoR = client.Phone,
                            CondicionIvaR = ivaConditions.FirstOrDefault(iv => iv.Id == invoice.IvaConditionId).Description,
                            EmailR = client.Email,
                            CondicionVenta = sale.PaidOut ? "Contado" : "Cuenta Corriente",
                            PtoVenta = invoice.PtoVenta.ToString(),
                            NroCbe = invoice.CompNro.ToString(),
                            FechaDesde = !string.IsNullOrEmpty(invoice.ServDesde) ? string.Format("{0:dd/MM/yyyy}", DateTime.ParseExact(invoice.ServDesde, "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None)) : string.Format("{0:dd/MM/yyyy}", DateTime.Now),
                            FechaHasta = !string.IsNullOrEmpty(invoice.ServHasta) ? string.Format("{0:dd/MM/yyyy}", DateTime.ParseExact(invoice.ServHasta, "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None)) : string.Format("{0:dd/MM/yyyy}", DateTime.Now),
                            FechaVtoPago = !string.IsNullOrEmpty(invoice.VtoPago) ? string.Format("{0:dd/MM/yyyy}", DateTime.ParseExact(invoice.VtoPago, "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None)) : string.Format("{0:dd/MM/yyyy}", DateTime.Now),
                            SubTotal = string.Format("{0:C2}", sale.SubTotal),
                            Total = string.Format("{0:C2}", invoice.ImpTotal),
                            Ajuste = string.Format("{0:C2}", invoice.Ajust),
                            CdoCbe = invoice.CompTypeId.ToString(),
                            NombreCbe = nombreCbe,
                            FechaEmision = string.Format("{0:dd/MM/yyyy}", DateTime.ParseExact(invoice.InvoiceDate, "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None)),
                            CAE = invoice.CAE,
                            FechaVtoCAE = string.Format("{0:dd/MM/yyyy}", DateTime.ParseExact(invoice.FechaVtoCAE, "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None)),
                            Iva0 = string.Format("{0:C2}", iva0),
                            Iva105 = string.Format("{0:C2}", iva105),
                            Iva21 = string.Format("{0:C2}", iva21),
                            Iva25 = string.Format("{0:C2}", iva25),
                            Iva27 = string.Format("{0:C2}", iva27),
                            Iva5 = string.Format("{0:C2}", iva5),
                            Leyenda = client.LegendInvoices,
                        });
                    }
                }

                return response.ToList();
            }
            catch (Exception)
            {
                return new List<InvoiceReportViewModel>();
            }
        }

        internal static List<InvoiceReportViewModel> ToSaleReport(Sale sale, CommerceData commerceData,
            ClientViewModel client, List<SaleCondition> saleConditions, List<IvaCondition> ivaConditions,
            List<Tax> taxes)
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("es-AR");
            var response = new List<InvoiceReportViewModel>();
            string nombreCbe = "Proforma", discountText = "Bonif.:";
            decimal discountValue = 0m;

            sale.SubTotal = sale.SaleDetails.Sum(s => s.TotalItem);

            if (sale.GeneralDiscount != 0)
            {
                discountText += string.Format(" {0:N0}%", sale.GeneralDiscount);
                discountValue = sale.SubTotal * sale.GeneralDiscount / 100;
            }

            response.Add(new InvoiceReportViewModel
            {
                DiscountText = discountText,
                DiscountValue = string.Format("{0:C2}", discountValue),
                CuitE = commerceData.CUIT.ToString(),
                RazonSocialE = commerceData.BusinessName,
                IIBB = commerceData.IIBB,
                EmailE = commerceData.Email,
                DireccionE = commerceData.Address,
                CondicionIvaE = ivaConditions.FirstOrDefault(iv => iv.Id == commerceData.IvaConditionId).Description,
                FechaInicio = string.Format("{0:dd/MM/yyyy}", commerceData.ActivityStartDate),
                TelefonoE = commerceData.Phone,
                RazonSocialR = client.BusinessName,
                CuitR = client.DocumentNumber.ToString(),
                DireccionR = client.Address,
                TelefonoR = client.Phone,
                CondicionIvaR = ivaConditions.FirstOrDefault(iv => iv.Id == client.IvaConditionId).Description,
                EmailR = client.Email,
                CondicionVenta = sale.PaidOut ? "Contado" : "Cuenta Corriente",
                PtoVenta = sale.SalePoint.ToString(),
                NroCbe = sale.SaleNumber.ToString(),
                SubTotal = string.Format("{0:C2}", sale.SubTotal),
                Total = string.Format("{0:C2}", sale.Total),
                CdoCbe = 999.ToString(),
                NombreCbe = nombreCbe,
                FechaEmision = string.Format("{0:dd/MM/yyyy}", sale.SaleDate),
                Leyenda = client.LegendBudget,
            });



            // Detalle de los items


            foreach (var itemDetail in sale.SaleDetails)
            {
                decimal price = 0, subtotal = 0, total = 0;
                Tax tax = taxes.Where(t => t.Id == itemDetail.TaxId).FirstOrDefault();

                price = itemDetail.Price + (itemDetail.Price * tax.Rate / 100);
                subtotal = price * itemDetail.Quantity;
                total = subtotal - (subtotal * itemDetail.Discount / 100);

                response.Add(new InvoiceReportViewModel
                {
                    DiscountText = discountText,
                    DiscountValue = string.Format("{0:C2}", discountValue),
                    Cantidad = itemDetail.Quantity.ToString(),
                    Descripcion = itemDetail.Description,
                    PrecioUni = string.Format("{0:C2}", price),
                    SubTotalItem = string.Format("{0:C2}", subtotal),
                    RazonSocialE = commerceData.BusinessName,
                    IIBB = commerceData.IIBB,
                    EmailE = commerceData.Email,
                    DireccionE = commerceData.Address,
                    FechaInicio = string.Format("{0:dd/MM/yyyy}", commerceData.ActivityStartDate),
                    TelefonoE = commerceData.Phone,
                    RazonSocialR = client.BusinessName,
                    DireccionR = client.Address,
                    TelefonoR = client.Phone,
                    EmailR = client.Email,
                    CondicionVenta = sale.PaidOut ? "Contado" : "Cuenta Corriente",
                    SubTotal = string.Format("{0:C2}", sale.SubTotal),
                    NombreCbe = nombreCbe,
                    FechaEmision = string.Format("{0:dd/MM/yyyy}", sale.SaleDate),
                    CondicionIvaR = ivaConditions.FirstOrDefault(iv => iv.Id == client.IvaConditionId).Description,
                    Total = string.Format("{0:C2}", sale.Total),
                    CdoCbe = 999.ToString(),
                    CuitE = commerceData.CUIT.ToString(),
                    CuitR = client.DocumentNumber.ToString(),
                    PtoVenta = sale.SalePoint.ToString(),
                    NroCbe = sale.SaleNumber.ToString(),
                    Leyenda = client.LegendBudget,

                });
                if (itemDetail.Discount != 0)
                {
                    string dicount = string.Format("{0:C2}", subtotal * itemDetail.Discount / 100);
                    string text = "Descuento";
                    string value = string.Format("{0}: {1}%", text, itemDetail.Discount);

                    response.Add(new InvoiceReportViewModel
                    {
                        DiscountText = discountText,
                        DiscountValue = string.Format("{0:C2}", discountValue),
                        Cantidad = "1",
                        Descripcion = value,
                        PrecioUni = dicount,    //,string.Format("{0:C2}", itemDetail.Price),
                        SubTotalItem = dicount, //string.Format("{0:C2}", itemDetail.SubTotal),
                        RazonSocialE = commerceData.BusinessName,
                        IIBB = commerceData.IIBB,
                        EmailE = commerceData.Email,
                        DireccionE = commerceData.Address,
                        FechaInicio = string.Format("{0:dd/MM/yyyy}", commerceData.ActivityStartDate),
                        TelefonoE = commerceData.Phone,
                        RazonSocialR = client.BusinessName,
                        DireccionR = client.Address,
                        TelefonoR = client.Phone,
                        EmailR = client.Email,
                        CondicionVenta = sale.PaidOut ? "Contado" : "Cuenta Corriente",
                        SubTotal = string.Format("{0:C2}", sale.SubTotal),
                        NombreCbe = nombreCbe,
                        FechaEmision = string.Format("{0:dd/MM/yyyy}", sale.SaleDate),
                        CondicionIvaR = ivaConditions.FirstOrDefault(iv => iv.Id == client.IvaConditionId).Description,
                        Total = string.Format("{0:C2}", sale.Total),
                        CdoCbe = 999.ToString(),
                        CuitE = commerceData.CUIT.ToString(),
                        CuitR = client.DocumentNumber.ToString(),
                        PtoVenta = sale.SalePoint.ToString(),
                        NroCbe = sale.SaleNumber.ToString(),
                        Leyenda = client.LegendBudget,
                    });
                }
            }



            return response.ToList();
        }
    }
}
