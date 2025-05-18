using GestionComercial.Domain.Entities.BoxAndBank;
using GestionComercial.Domain.Entities.Masters;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace GestionComercial.Domain.Entities.AccountingBook
{
    public class Account : CommonEntity
    {
        [Required(ErrorMessage = "El campo {0} es requerido")]
        [Display(Name = "Grupo")]
        [Range(1, 9, ErrorMessage = "debe seleccionar el {0}")]
        public int AccountGroupNumber { get; set; }


        [Required(ErrorMessage = "El campo {0} es requerido")]
        [Display(Name = "SubGrupo 1")]
        [Range(0, 99, ErrorMessage = "debe seleccionar el {0}")]
        public int AccountSubGroupNumber1 { get; set; }


        [Required(ErrorMessage = "El campo {0} es requerido")]
        [Display(Name = "SubGrupo 2")]
        [Range(0, 999, ErrorMessage = "debe seleccionar el {0}")]
        public int AccountSubGroupNumber2 { get; set; }


        [Required(ErrorMessage = "El campo {0} es requerido")]
        [Display(Name = "SubGrupo 3")]
        [Range(0, 9999, ErrorMessage = "debe seleccionar el {0}")]
        public int AccountSubGroupNumber3 { get; set; }


        [Required(ErrorMessage = "El campo {0} es requerido")]
        [Display(Name = "SubGrupo 4")]
        [Range(0, 99999, ErrorMessage = "debe seleccionar el {0}")]
        public int AccountSubGroupNumber4 { get; set; }


        [Required(ErrorMessage = "El campo {0} es requerido")]
        [Display(Name = "SubGrupo 5")]
        [Range(0, 999999, ErrorMessage = "debe seleccionar el {0}")]
        public int AccountSubGroupNumber5 { get; set; }


        [Required(ErrorMessage = "El campo {0} es requerido")]
        [MaxLength(100, ErrorMessage = "El campo {0} no puede contener más de {1} caracteres")]
        [Display(Name = "Nombre")]
        public string Name { get; set; }

        [Display(Name = "Descripción")]
        public string Description { get; set; }


        public int AccountTypeId { get; set; }


        [Display(Name = "Referenciado?")]
        public bool IsReference { get; set; }


        [Display(Name = "Cuenta")]
        public string FullDescription
        {
            get
            {
                return string.Format("{0:00}.{1:000}.{2:0000}.{3:00000}.{4:000000} - {5}",
                     AccountSubGroupNumber1,
                    AccountSubGroupNumber2,
                    AccountSubGroupNumber3,
                    AccountSubGroupNumber4,
                    AccountSubGroupNumber5,
                    Name);
                //return string.Format("{0:0}.{1:00}.{2:000}.{3:0000}.{4:00000}.{5:000000} - {6}",
                //    AccountGroupNumber,
                //    AccountSubGroupNumber1,
                //    AccountSubGroupNumber2,
                //    AccountSubGroupNumber3,
                //    AccountSubGroupNumber4,
                //    AccountSubGroupNumber5,
                //    Name);
            }
        }


        [JsonIgnore] 
        public virtual ICollection<Box>? Boxes { get; set; }


        [JsonIgnore] 
        public virtual ICollection<Bank>? Banks { get; set; }

        [JsonIgnore]
        public virtual ICollection<AccountVinculation>? AccountVinculations { get; set; }

        [JsonIgnore]
        public virtual AccountType? AccountType { get; set; }
    }
}
