using GestionComercial.Domain.DTOs.PriceLists;
using GestionComercial.Domain.Entities.Afip;
using GestionComercial.Domain.Entities.Masters;

namespace GestionComercial.Domain.DTOs.Client
{
    public class EditClientViewModel
    {
        public ClientViewModel Client { get; set; }

        // Listas maestras compartidas
        public List<PriceListViewModel> PriceLists { get; set; }
        public List<State> States { get; set; }
        public List<SaleCondition> SaleConditions { get; set; }
        public List<IvaCondition> IvaConditions { get; set; }
        public List<DocumentType> DocumentTypes { get; set; }
    }
}
