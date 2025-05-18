using GestionComercial.Domain.Entities.Masters;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace GestionComercial.Domain.Entities.AccountingBook
{
    public class AccountVinculation : CommonEntity
    {
        [Required(ErrorMessage = "El campo {0} es requerido")]
        [Display(Name = "Cuenta")]
        public int AccountId { get; set; }


        [Required(ErrorMessage = "El campo {0} es requerido")]
        [Display(Name = "Vinculada con")]
        public int VinculatedAccountId { get; set; }



        [JsonIgnore] 
        public virtual Account? Account { get; set; }
    }
}
