using System.ComponentModel.DataAnnotations;

namespace GestionComercial.Domain.Entities.Masters.Configuration
{
    public class PcParameter : CommonEntity
    {
        [Display(Name = "Nombre PC")]
        public string PCName { get; set; }

        [Display(Name = "Punto de venta")]
        public int SalePoint { get; set; }

        [Display(Name = "Ultimo Inicio")]
        public DateTime? LastLogin { get; set; }
    }
}
