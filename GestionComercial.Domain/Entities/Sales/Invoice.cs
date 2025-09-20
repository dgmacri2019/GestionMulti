using GestionComercial.Domain.Entities.Afip;
using GestionComercial.Domain.Entities.Masters;
using System.Text.Json.Serialization;

namespace GestionComercial.Domain.Entities.Sales
{
    public class Invoice : CommonEntity
    {
        public int ClientId { get; set; }

        public int SaleId { get; set; }

        public long Cuit { get; set; }

        public int IvaConditionId { get; set; }

        public int ReceptorIvaId { get; set; }

        public int PtoVenta { get; set; }

        public int CompTypeId { get; set; }

        public long CompNro { get; set; }

        public string? InvoiceDate { get; set; }

        public int Concepto { get; set; }

        public string? ServDesde { get; set; }

        public string? ServHasta { get; set; }

        public string? VtoPago { get; set; }

        public int ClientDocType { get; set; }

        public long ClientDocNro { get; set; }

        public double ImpTotal { get; set; }

        public double ImpTotalConc { get; set; }

        public double ImpNeto { get; set; }

        public double ImpTotalIVA { get; set; }

        public string? CBU { get; set; }

        public string? Alias { get; set; }

        public string? CAE { get; set; }

        public double InternalTax { get; set; }

        public string? FechaVtoCAE { get; set; }

        public string? FechaProceso { get; set; }

        public double Ajust => ImpTotal - (ImpNeto + ImpTotalIVA);

        public string NumberString => string.Format("{0:00000}-{1:00000000}", PtoVenta, CompNro);




        //[JsonIgnore]
        public ICollection<InvoiceDetail>? InvoiceDetails { get; set; }

        //[JsonIgnore]
        public virtual Client? Client { get; set; }

        //[JsonIgnore]
        public virtual IvaCondition? IvaCondition { get; set; }

        //[JsonIgnore]
        public virtual Sale? Sale { get; set; }

    }
}
