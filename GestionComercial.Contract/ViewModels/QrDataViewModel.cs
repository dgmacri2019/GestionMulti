using System.Runtime.Serialization;

namespace GestionComercial.Contract.ViewModels
{
    [DataContract]
    public class QrDataViewModel
    {
        [DataMember] 
        public int ver { get; set; }
        [DataMember] 
        public string fecha { get; set; }
        [DataMember]
        public long cuit { get; set; }
        [DataMember]
        public int ptoVta { get; set; }
        [DataMember]
        public int tipoCmp { get; set; }
        [DataMember]
        public long nroCmp { get; set; }
        [DataMember]
        public decimal importe { get; set; }
        [DataMember]
        public string moneda { get; set; }
        [DataMember]
        public decimal ctz { get; set; }
        [DataMember]
        public int tipoDocRec { get; set; }
        [DataMember]
        public long nroDocRec { get; set; }
        [DataMember]
        public string tipoCodAut { get; set; }
        [DataMember]
        public long codAut { get; set; }
    }
}
