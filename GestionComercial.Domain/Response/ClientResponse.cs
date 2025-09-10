using GestionComercial.Domain.DTOs.Client;
using GestionComercial.Domain.Entities.Afip;
using GestionComercial.Domain.Entities.Masters;
using GestionComercial.Domain.Entities.Stock;

namespace GestionComercial.Domain.Response
{
    public class ClientResponse : GeneralResponse
    {
        public string BussinessName { get; set; } = string.Empty;
        public string FantasyName { get; set; } = string.Empty;

        public ClientViewModel ClientViewModel { get; set; }

        public List<ClientViewModel> ClientViewModels { get; set; }

        public int TotalRegisters { get; set; }      
    }
}
