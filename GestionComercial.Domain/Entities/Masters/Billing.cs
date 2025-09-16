using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace GestionComercial.Domain.Entities.Masters
{
    public class Billing : CommonEntity
    {
        [Display(Name = "Importo Certifacado")]
        public bool HasCertificate { get; set; }

        [Display(Name = "Password Certificado")]
        public string? CertPass { get; set; }

        [Required]
        public UInt32 UniqueId { get; set; }

        [Display(Name = "Fecha de generación Token Factura")]
        public DateTime? WSDLGenerationTime { get; set; }

        [Display(Name = "Fecha de vencimiento Token Factura")]
        public DateTime? WSDLExpirationTime { get; set; }

        [Required]
        [Display(Name = "Password Factura")]
        public string? WSDLSign { get; set; }

        [Required]
        [Display(Name = "Token Factura")]
        public string? WSDLToken { get; set; }

        [Display(Name = "Importo Certifacado")]
        public bool UseWSDL { get; set; }

        [Display(Name = "Fecha de generación Token Padron")]
        public DateTime? PadronGenerationTime { get; set; }

        [Display(Name = "Fecha de vencimiento Token Padron")]
        public DateTime? PadronExpirationTime { get; set; }

        [Required]
        [Display(Name = "Password Padron")]
        public string? PadronSign { get; set; }

        [Display(Name = "Importo Certifacado")]
        public bool UsePadron { get; set; }

        [Required]
        [Display(Name = "Token Padron")]
        public string? PadronToken { get; set; }

        //[Display(Name = "Fecha de generación Token Padron A5")]
        //public DateTime? PadronA5GenerationTime { get; set; }

        //[Display(Name = "Fecha de vencimiento Token Padron A5")]
        //public DateTime? PadronA5ExpirationTime { get; set; }

        //[Required]
        //[Display(Name = "Password Padron A5")]
        //public string? PadronA5Sign { get; set; }

        //[Required]
        //[Display(Name = "Token Padron A5")]
        //public string? PadronA5Token { get; set; }

        [Required]
        [Display(Name = "Emitir Comprobantes M")]
        public bool EmitInvoiceM { get; set; }

        [Display(Name = "Punto de Venta")]
        public int SalePoint { get; set; }

        [Display(Name = "Concepto de Comprobantes")]
        public int Concept { get; set; }

        public bool ExpireCertificate { get; set; }

        public string? ExpireCertificateText { get; set; }

        public int CommerceDataId { get; set; }




        //[JsonIgnore] 
        public virtual CommerceData? CommerceData { get; set; }


    }
}
