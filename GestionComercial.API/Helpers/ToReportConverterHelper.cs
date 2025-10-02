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
            ClientViewModel client, List<SaleCondition> saleConditions, List<IvaCondition> ivaConditions)
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("es-AR");
            var response = new List<InvoiceReportViewModel>();
            string nombreCbe = string.Empty, discountText = "Bonif.:";
            decimal discountValue = 0m;

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
                discountValue = sale.SubTotal * sale.GeneralDiscount;
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
            });



            // Detalle de los items


            foreach (var itemDetail in sale.SaleDetails)
            {
                response.Add(new InvoiceReportViewModel
                {
                    IvaTotal = string.Format("{0:C2}", invoice.ImpTotalIVA),
                    DiscountText = discountText,
                    DiscountValue = string.Format("{0:C2}", discountValue),
                    Cantidad = itemDetail.Quantity.ToString(),
                    Descripcion = itemDetail.Description,
                    PrecioUni = string.Format("{0:C2}", itemDetail.Price),
                    SubTotalItem = string.Format("{0:C2}", itemDetail.SubTotal),
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
                });
                if (itemDetail.Discount != 0)
                {
                    string dicount = string.Format("{0:C2}", ((itemDetail.Quantity * itemDetail.Price) + (itemDetail.Quantity * itemDetail.Price * itemDetail.Tax.Rate) / 100) * itemDetail.Discount / 100);
                    string text = "Descuento";
                    string value = string.Format("{0}: {1}%", text, itemDetail.Discount * -1);

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
                    });
                }
            }

            // Detalle de los IVA
            if (invoice.InvoiceDetails != null)
                foreach (var ivaDetail in invoice.InvoiceDetails)
                {
                    if (ivaDetail.IvaId == 3)
                    {
                        response.Add(new InvoiceReportViewModel
                        {
                            IvaTotal = string.Format("{0:C2}", invoice.ImpTotalIVA),
                            DiscountText = discountText,
                            DiscountValue = string.Format("{0:C2}", discountValue),
                            Iva0 = string.Format("{0:C2}", ivaDetail.ImporteIva),
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
                            DireccionR = invoice.Client.Address,
                            TelefonoR = invoice.Client.Phone,
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
                        });
                    }
                    else if (ivaDetail.IvaId == 9)
                    {
                        response.Add(new InvoiceReportViewModel
                        {
                            IvaTotal = string.Format("{0:C2}", invoice.ImpTotalIVA),
                            DiscountText = discountText,
                            DiscountValue = string.Format("{0:C2}", discountValue),
                            Iva25 = string.Format("{0:C2}", ivaDetail.ImporteIva),
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
                        });
                    }
                    else if (ivaDetail.IvaId == 8)
                    {
                        response.Add(new InvoiceReportViewModel
                        {
                            IvaTotal = string.Format("{0:C2}", invoice.ImpTotalIVA),
                            DiscountText = discountText,
                            DiscountValue = string.Format("{0:C2}", discountValue),
                            Iva5 = string.Format("{0:C2}", ivaDetail.ImporteIva),
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
                        });
                    }
                    else if (ivaDetail.IvaId == 4)
                    {
                        response.Add(new InvoiceReportViewModel
                        {
                            IvaTotal = string.Format("{0:C2}", invoice.ImpTotalIVA),
                            DiscountText = discountText,
                            DiscountValue = string.Format("{0:C2}", discountValue),
                            Iva105 = string.Format("{0:C2}", ivaDetail.ImporteIva),
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
                        });
                    }
                    else if (ivaDetail.IvaId == 5)
                    {
                        response.Add(new InvoiceReportViewModel
                        {
                            IvaTotal = string.Format("{0:C2}", invoice.ImpTotalIVA),
                            DiscountText = discountText,
                            DiscountValue = string.Format("{0:C2}", discountValue),
                            Iva21 = string.Format("{0:C2}", ivaDetail.ImporteIva),
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
                        });
                    }
                    else if (ivaDetail.IvaId == 6)
                    {
                        response.Add(new InvoiceReportViewModel
                        {
                            IvaTotal = string.Format("{0:C2}", invoice.ImpTotalIVA),
                            DiscountText = discountText,
                            DiscountValue = string.Format("{0:C2}", discountValue),
                            Iva27 = string.Format("{0:C2}", ivaDetail.ImporteIva),
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
                        });
                    }
                }

            return response.ToList();
        }
    }
}
