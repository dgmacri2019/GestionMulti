using GestionComercial.Domain.Entities.BoxAndBank;
using GestionComercial.Domain.Entities.Masters;
using GestionComercial.Domain.Entities.Sales;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;

namespace GestionComercial.Domain.DTOs.Sale
{
    public class SaleViewModel : INotifyPropertyChanged
    {
        public int Id { get; set; }

        private DateTime date = DateTime.Today;
        public DateTime Date
        {
            get => date;
            set { date = value; OnPropertyChanged(); }
        }


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

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [Display(Name = "Cliente")]
        public int ClientId { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [Display(Name = "Fecha de venta")]
        public DateTime SaleDate { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [Display(Name = "Forma de pago")]
        public int SaleConditionId { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [Display(Name = "Lista de precios")]
        public int PriceListId { get; set; }

        [Display(Name = "Importe total de la venta")]
        public decimal Total { get; set; }

        public decimal SubTotal { get; set; }

        public decimal GeneralDiscount { get; set; }

        public decimal InternalTax { get; set; }

        public decimal TotalIVA21 { get; set; }

        public decimal TotalIVA105 { get; set; }

        public decimal TotalIVA27 { get; set; }

        public decimal BaseImp21 { get; set; }

        public decimal BaseImp105 { get; set; }

        public decimal BaseImp27 { get; set; }

        [Display(Name = "Saldo Pendiente")]
        public decimal Sold { get; set; }

        public bool PaidOut { get; set; }

        public bool PartialPay { get; set; }

        public bool IsFinished { get; set; }

        public int AutorizationCode { get; set; }

        public int SaleNumber { get; set; }

        public int SalePoint { get; set; }

        public string SaleNumberString => string.Format("{0:00000}-{1:00000000}", SalePoint, SaleNumber);





        [JsonIgnore]
        public virtual Entities.Masters.Client? Client { get; set; }

        [JsonIgnore]
        public virtual SaleCondition? SaleCondition { get; set; }

        [JsonIgnore]
        public virtual ICollection<SaleDetail>? SaleDetails { get; set; }

        [JsonIgnore]
        public virtual ICollection<SalePayMetodDetail>? SalePayMetodDetails { get; set; }

        [JsonIgnore]
        public virtual ICollection<Acreditation>? Acreditations { get; set; }

        public ObservableCollection<Entities.Masters.Client> Clients { get; set; } = [];

        public ObservableCollection<PriceList> PriceLists { get; set; } = [];

       // public ObservableCollection<ArticleItem> ArticleItems { get; set; } = [];
        public ObservableCollection<SaleCondition> SaleConditions { get; set; } = [];


        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
