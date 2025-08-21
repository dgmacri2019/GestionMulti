using GestionComercial.Domain.Entities.Masters;

namespace GestionComercial.Domain.Entities.Afip
{
    public class DocumentType : CommonEntity
    {
        public int AfipId { get; set; }

        public string Description { get; set; }
    }
}
