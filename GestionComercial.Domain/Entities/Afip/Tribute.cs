using GestionComercial.Domain.Entities.Masters;

namespace GestionComercial.Domain.Entities.Afip
{
    public class Tribute : CommonEntity
    {
        public short AfipId { get; set; }

        public string Description { get; set; }
    }
}
