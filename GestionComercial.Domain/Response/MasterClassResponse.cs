using GestionComercial.Domain.DTOs.Master.Configurations.Commerce;
using GestionComercial.Domain.Entities.Afip;
using GestionComercial.Domain.Entities.Masters;
using GestionComercial.Domain.Entities.Stock;

namespace GestionComercial.Domain.Response
{
    public class MasterClassResponse : GeneralResponse
    {
       public List<State>? States { get; set; }
        public List<DocumentType>? DocumentTypes { get; set; }
        public List<IvaCondition>? IvaConditions { get; set; }
        public List<SaleCondition>? SaleConditions { get; set; }
        public List<Measure>? Measures { get; set; }
        public List<Tax>? Taxes { get; set; }
        public CommerceData? CommerceData { get; set; }
        public BillingViewModel? BillingViewModel { get; set; }
    }
}
