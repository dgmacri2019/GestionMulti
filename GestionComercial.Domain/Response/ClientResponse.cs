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
        public List<PriceList> PriceLists { get; set; }
        public List<State> States { get; set; }
        public List<IvaCondition> IvaConditions { get; set; }
        public List<DocumentType> DocumentTypes { get; set; }
        public List<SaleCondition> SaleConditions { get; set; }
    }
}
