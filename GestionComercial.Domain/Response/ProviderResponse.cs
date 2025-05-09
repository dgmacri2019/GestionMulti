using GestionComercial.Domain.DTOs.Provider;

namespace GestionComercial.Domain.Response
{
    public class ProviderResponse : GeneralResponse
    {
        public string BussinessName { get; set; } = string.Empty;
        public string FantasyName { get; set; } = string.Empty;

        public ProviderViewModel ProviderViewModel { get; set; }
    }
}
