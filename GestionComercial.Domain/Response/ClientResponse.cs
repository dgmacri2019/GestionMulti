using GestionComercial.Domain.DTOs.Client;

namespace GestionComercial.Domain.Response
{
    public class ClientResponse : GeneralResponse
    {
        public string BussinessName { get; set; } = string.Empty;
        public string FantasyName { get; set; } = string.Empty;

        public ClientViewModel ClientViewModel { get; set; }
    }
}
