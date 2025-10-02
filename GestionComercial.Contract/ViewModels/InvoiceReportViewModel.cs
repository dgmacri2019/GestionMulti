using System.Runtime.Serialization;

namespace GestionComercial.Contract.ViewModels
{
    [DataContract]
    public class InvoiceReportViewModel
    {
        [DataMember]
        public string CuitE { get; set; }
        [DataMember]
        public string RazonSocialE { get; set; }
        [DataMember]
        public string IIBB { get; set; }
        [DataMember] 
        public string DireccionE { get; set; }
        [DataMember] 
        public string FechaInicio { get; set; }
        [DataMember] 
        public string TelefonoE { get; set; }
        [DataMember] 
        public string EmailE { get; set; }
        [DataMember] 
        public string RazonSocialR { get; set; }
        [DataMember] 
        public string DireccionR { get; set; }
        [DataMember] 
        public string TelefonoR { get; set; }
        [DataMember] 
        public string CondicionIvaR { get; set; }
        [DataMember] 
        public string EmailR { get; set; }
        [DataMember] 
        public string CondicionVenta { get; set; }
        [DataMember] 
        public string Cantidad { get; set; }
        [DataMember] 
        public string Descripcion { get; set; }
        [DataMember]
        public string PrecioUni { get; set; }
        [DataMember] 
        public string PtoVenta { get; set; }
        [DataMember] 
        public string NroCbe { get; set; }
        [DataMember] 
        public string FechaDesde { get; set; }
        [DataMember] 
        public string FechaHasta { get; set; }
        [DataMember] 
        public string FechaVtoPago { get; set; }
        [DataMember] 
        public string SubTotal { get; set; }
        [DataMember] 
        public string Iva21 { get; set; }
        [DataMember] 
        public string Iva105 { get; set; }
        [DataMember] 
        public string Iva5 { get; set; }
        [DataMember] 
        public string Iva25 { get; set; }
        [DataMember] 
        public string Iva0 { get; set; }
        [DataMember] 
        public string Iva27 { get; set; }
        [DataMember] 
        public string IvaTotal { get; set; }
        [DataMember] 
        public string Total { get; set; }
        [DataMember] 
        public string CdoCbe { get; set; }
        [DataMember] 
        public string NombreCbe { get; set; }
        [DataMember] 
        public string FechaEmision { get; set; }
        [DataMember] 
        public string CuitR { get; set; }
        [DataMember] 
        public string SubTotalItem { get; set; }
        [DataMember] 
        public string CAE { get; set; }
        [DataMember] 
        public string FechaVtoCAE { get; set; }
        [DataMember] 
        public string CBU { get; set; }
        [DataMember] 
        public string Alias { get; set; }
        [DataMember] 
        public string LetraCbe { get; set; }
        [DataMember] 
        public string Ajuste { get; set; }
        [DataMember] 
        public string CondicionIvaE { get; set; }
        [DataMember] 
        public string DiscountText { get; set; }
        [DataMember] 
        public string DiscountValue { get; set; }
        [DataMember] 
        public string Leyenda { get; set; }
        [DataMember] 
        public string Lejend { get; set; }
        [DataMember] 
        public bool InformarCBU { get; set; }
        [DataMember] 
        public bool InformarLeyendaFactura { get; set; }
    }
}
