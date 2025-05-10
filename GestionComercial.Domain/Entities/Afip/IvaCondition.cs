using GestionComercial.Domain.Entities.Masters;

namespace GestionComercial.Domain.Entities.Afip
{
    public class IvaCondition : CommonEntity
    {
        public string Description { get; set; }

        public string CbteClase { get; set; }

        public int AfipId { get; set; }
    }
}
