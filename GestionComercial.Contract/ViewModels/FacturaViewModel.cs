using System.Runtime.Serialization;

namespace GestionComercial.Contract.ViewModels
{
    [DataContract]
    public class FacturaViewModel
    {
        [DataMember] 
        public byte[] LogoByte { get; set; }
        [DataMember] 
        public string InvoiceDate { get; set; }
        [DataMember] 
        public long Cuit { get; set; }
        [DataMember] 
        public int PtoVenta { get; set; }
        [DataMember] 
        public int CompTypeId { get; set; }
        [DataMember] 
        public long CompNro { get; set; }
        [DataMember] 
        public double ImpTotal { get; set; }
        [DataMember] 
        public int DocType { get; set; }
        [DataMember] 
        public long DocNro { get; set; }
        [DataMember] 
        public string CAE { get; set; }
        [DataMember] 
        public string Leyenda { get; set; }
    }
}
