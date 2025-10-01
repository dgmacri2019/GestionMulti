namespace Reports.ViewModels
{
    public class FacturaViewModel
    {
        public byte[] LogoByte { get; set; }
        public string InvoiceDate { get; set; }
        public long Cuit { get; set; }
        public int PtoVenta { get; set; }
        public int CompTypeId { get; set; }
        public long CompNro { get; set; }
        public double ImpTotal { get; set; }
        public int DocType { get; set; }
        public long DocNro { get; set; }
        public string CAE { get; set; }
    }
}
