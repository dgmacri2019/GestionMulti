using System.Security.Cryptography.X509Certificates;

namespace GestionComercial.Domain.Response
{
    public class AfipLoginResquestResponse : GeneralResponse
    {
        public UInt32 UniqueId { get; set; }

        public DateTime GenerationTime { get; set; }

        public DateTime ExpirationTime { get; set; }

        public string Sign { get; set; }

        public string Token { get; set; }

        public X509Certificate2 X509Certificate2 { get; set; }
    }
}
