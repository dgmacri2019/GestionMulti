using GestionComercial.Domain.Entities.Masters;
using static GestionComercial.Domain.Constant.Enumeration;

namespace GestionComercial.Domain.Entities.Sales
{
    public class Invoice : CommonEntity
    {
        public int ClientId { get; set; }

        public int SaleId { get; set; }

        public long Cuit { get; set; }

        public TaxCondition TaxCondition { get; set; }

        public int PtoVenta { get; set; }

        public int CompTypeId { get; set; }

        public long CompNro { get; set; }

        public string InvoiceDate { get; set; }

        public string ServDesde { get; set; }

        public string ServHasta { get; set; }

        public string VtoPago { get; set; }

        public int DocType { get; set; }

        public long DocNro { get; set; }

        public double ImpTotal { get; set; }

        public double ImpTotalConc { get; set; }

        public double ImpNeto { get; set; }

        public double ImpTotalIVA { get; set; }

        public string CBU { get; set; }

        public string Alias { get; set; }

        public string CAE { get; set; }

        public string FechaVtoCAE { get; set; }

        public string FechaProceso { get; set; }

        public double Ajust => ImpTotal - (ImpNeto + ImpTotalIVA);

        public string NumberString => string.Format("{0:00000}-{1:00000000}", PtoVenta, CompNro);



        public ICollection<InvoiceDetail> InvoiceDetails { get; set; }

        public virtual Client Client { get; set; }

        public virtual Sale Sale { get; set; }
    }
}
