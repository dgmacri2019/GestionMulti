using GestionComercial.Domain.Entities.Masters;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static GestionComercial.Domain.Constant.Enumeration;

namespace GestionComercial.Domain.DTOs.Provider
{
    public class ProviderViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [MaxLength(300, ErrorMessage = "El campo {0} debe contener un máximo de {1} caracteres")]
        [Display(Name = "Razón Social")]
        public string BusinessName { get; set; }

        [MaxLength(300, ErrorMessage = "El campo {0} debe contener un máximo de {1} caracteres")]
        [Display(Name = "Nombre de Fantasía")]
        public string? FantasyName { get; set; }

        [Display(Name = "Tipo Documento")]
        public DocumentType DocumentType { get; set; }

        [MaxLength(13, ErrorMessage = "El {0} no puede contener mas de {1} caracteres")]
        [MinLength(7, ErrorMessage = "El {0} no puede contener menos de {1} caracteres")]
        [Display(Name = "Número Documento")]
        public string DocumentNumber { get; set; }

        [Display(Name = "Teléfono / Fax")]
        [MaxLength(50, ErrorMessage = "El campo {0} no puede contener mas de {1} caracteres")]
        public string? Phone { get; set; }

        [Display(Name = "Teléfono / Fax")]
        [MaxLength(50, ErrorMessage = "El campo {0} no puede contener mas de {1} caracteres")]
        public string? Phone1 { get; set; }

        [Display(Name = "Teléfono / Fax")]
        [MaxLength(50, ErrorMessage = "El campo {0} no puede contener mas de {1} caracteres")]
        public string? Phone2 { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [Display(Name = "Dirección")]
        [MaxLength(200, ErrorMessage = "El campo {0} no puede contener mas de {1} caracteres")]
        public string? Address { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [Display(Name = "Código Postal")]
        [MaxLength(8, ErrorMessage = "El campo {0} no puede contener mas de {1} caracteres")]
        public string PostalCode { get; set; }

        [Display(Name = "E-Mail")]
        [MaxLength(100, ErrorMessage = "El campo {0} no puede contener mas de {1} caracteres")]
        [DataType(DataType.EmailAddress)]
        public string? Email { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [Display(Name = "Provincia")]
        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar la {0}")]
        public int StateId { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [Display(Name = "Localidad")]
        [MaxLength(500, ErrorMessage = "El campo {0} no puede contener mas de {1} caracteres")]
        public string? City { get; set; }

        [Display(Name = "Sitio Web")]
        [MaxLength(100, ErrorMessage = "El campo {0} no puede contener mas de {1} caracteres")]
        public string? WebSite { get; set; }

        [Display(Name = "Sitio Web")]
        [DataType(DataType.MultilineText)]
        public string? Remark { get; set; }

        [Display(Name = "Fecha de última compra")]
        public DateTime? LastPuchase { get; set; }

        [Display(Name = "Saldo Pendiente")]
        public decimal Sold { get; set; }

        [Display(Name = "Día de pago")]
        [MaxLength(200, ErrorMessage = "El campo {0} no puede contener mas de {1} caracteres")]
        public string? PayDay { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [Display(Name = "Metodo de pago")]
        public SaleCondition SaleCondition { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [Display(Name = "Condición de IVA")]
        [Range(1, double.MaxValue, ErrorMessage = "Debe seleccionar la {0}")]
        public TaxCondition TaxCondition { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy HH:mm}", ApplyFormatInEditMode = true)]
        [Required(ErrorMessage = "El campo {0} es requerido")]
        [Display(Name = "Creado el")]
        public DateTime CreateDate { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [MaxLength(100, ErrorMessage = "El campo {0} no puede contener más de {1} caracteres")]
        [Display(Name = "Creado por")]
        public string CreateUser { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy HH:mm}", ApplyFormatInEditMode = true)]
        [Display(Name = "Modificado el")]
        public DateTime? UpdateDate { get; set; }

        [MaxLength(100, ErrorMessage = "El campo {0} no puede contener más de {1} caracteres")]
        [Display(Name = "Modificado por")]
        public string? UpdateUser { get; set; }

        [Display(Name = "Borrado?")]
        public bool IsDeleted { get; set; }

        [Display(Name = "Habilitado?")]
        public bool IsEnabled { get; set; }

        [Display(Name = "Provincia")]
        public string State { get; set; }

        public ICollection<State> States { get; set; }
        public ObservableCollection<SaleCondition> SaleConditions { get; set; }
        public ObservableCollection<TaxCondition> TaxConditions { get; set; }
        public ObservableCollection<DocumentType> DocumentTypes { get; set; }
    }
}
