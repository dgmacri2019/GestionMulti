using System.ComponentModel.DataAnnotations;

namespace GestionComercial.Domain.DTOs.Master.Configurations.Commerce
{
    public class BillingViewModel
    {
        public bool HasCertificate { get; set; }

        public string? CertPass { get; set; }

        public UInt32 UniqueId { get; set; }

        public DateTime? WSDLGenerationTime { get; set; }

        public DateTime? WSDLExpirationTime { get; set; }

        public string? WSDLSign { get; set; }

        public string? WSDLToken { get; set; }

        public bool UseWSDL { get; set; }

        public DateTime? PadronGenerationTime { get; set; }

        public DateTime? PadronExpirationTime { get; set; }

        public string? PadronSign { get; set; }

        public bool UsePadron { get; set; }

        public string? PadronToken { get; set; }

        public int Id { get; set; }

        public DateTime CreateDate { get; set; }

        public string? CreateUser { get; set; }

        public DateTime? UpdateDate { get; set; }

        public string? UpdateUser { get; set; }

        public bool IsDeleted { get; set; }

        public bool IsEnabled { get; set; }

        public bool EmitInvoiceM { get; set; }

        public int Concept { get; set; }

        public bool ExpireCertificate { get; set; }

        public string? ExpireCertificateText { get; set; }

        public string? CertPath { get; set; }

        public int CommerceDataId { get; set; }
        public byte[]? CertificateByteArray { get; set; }
    }
}
