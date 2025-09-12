using GestionComercial.Domain.Entities.Afip;
using GestionComercial.Domain.Entities.Stock;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace GestionComercial.Domain.DTOs.Stock
{
    public class ArticleViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private decimal _cost, _bonification, _realCost, _costWithTaxes, _internalTax, _utility, _salePrice, _salePriceWithTaxes;
        private int _taxId;


        [Required(ErrorMessage = "El campo {0} es requerido")]
        public int Id { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [MaxLength(8, ErrorMessage = "El campo {0} debe contener un máximo de {1} caracteres")]
        [Display(Name = "Código")]
        public string Code { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [MaxLength(1000, ErrorMessage = "El campo {0} debe contener un máximo de {1} caracteres")]
        [Display(Name = "Descripción")]
        public string Description { get; set; }

        [MaxLength(50, ErrorMessage = "El campo {0} debe contener un máximo de {1} caracteres")]
        [Display(Name = "Codigo de Barras")]
        public string? BarCode { get; set; }

        [Required(ErrorMessage = "El Campo {0} es requerido")]
        [Display(Name = "Precio de compra")]
        [DisplayFormat(DataFormatString = "{0:C4}", ApplyFormatInEditMode = false)]
        //[Range(0, double.MaxValue, ErrorMessage = "Debe seleccion un {0} entre {1} y {2}")]
        public decimal Cost
        {
            get => _cost;
            set
            {
                if (_cost != value)
                {
                    _cost = value;
                    OnPropertyChanged(nameof(Cost));
                    RecalculateFromCost();
                }
            }
        }

        [Required(ErrorMessage = "El Campo {0} es requerido")]
        [Display(Name = "Bonificación / Recargo")]
        [Range(0, 100, ErrorMessage = "Debe seleccion un {0} entre {1} y {2}")]
        public decimal Bonification
        {
            get => _bonification;
            set
            {
                if (_bonification != value)
                {
                    _bonification = value;
                    OnPropertyChanged(nameof(Bonification));
                    RecalculateFromCost();
                }
            }
        }

        [Display(Name = "Precio de compra")]
        [DisplayFormat(DataFormatString = "{0:C4}", ApplyFormatInEditMode = false)]
        //[Range(0, double.MaxValue, ErrorMessage = "Debe seleccion un {0} entre {1} y {2}")]
        public decimal RealCost
        {
            get => _realCost;
            set
            {
                if (_realCost != value)
                {
                    _realCost = value;
                    OnPropertyChanged(nameof(RealCost));
                    RecalculateFromRealCost();
                }
            }
        }

        [Display(Name = "Utilidad")]
        [DisplayFormat(DataFormatString = "{0:P0}", ApplyFormatInEditMode = false)]
        [Range(0, 99999, ErrorMessage = "Debe seleccion un {0} entre {1} y {2}")]
        public decimal Utility
        {
            get => _utility;
            set
            {
                if (_utility != value)
                {
                    _utility = value;
                    OnPropertyChanged(nameof(Utility));
                    RecalculateSalePricesFromUtility();
                }
            }
        }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [Range(1, double.MaxValue, ErrorMessage = "Debe seleccionar un {0}")]
        [Display(Name = "Tipo de IVA")]
        public int TaxId
        {
            get => _taxId;
            set
            {
                if (_taxId != value)
                {
                    _taxId = value;
                    OnPropertyChanged(nameof(TaxId));
                    RecalculateFromRealCost();
                }
            }
        }

        [Required(ErrorMessage = "El Campo {0} es requerido")]
        [Display(Name = "Impuestos Internos")]
        [DisplayFormat(DataFormatString = "{0:P0}", ApplyFormatInEditMode = false)]
        [Range(0, 100, ErrorMessage = "Debe seleccion un {0} entre {1} y {2}")]
        public decimal InternalTax
        {
            get => _internalTax;
            set
            {
                if (_internalTax != value)
                {
                    _internalTax = value;
                    OnPropertyChanged(nameof(InternalTax));
                    RecalculateFromCost();
                }
            }
        }

        //[Required(ErrorMessage = "El Campo {0} es requerido")]
        //[DisplayFormat(DataFormatString = "{0:P2}", ApplyFormatInEditMode = false)]
        [Display(Name = "Puntos por venta")]
        [Range(0, 99999999, ErrorMessage = "Debe seleccion un {0} entre {1} y {2}")]
        public int SalePoint { get; set; }

        //[Required(ErrorMessage = "El Campo {0} es requerido")]
        //[DisplayFormat(DataFormatString = "{0:P2}", ApplyFormatInEditMode = false)]
        [Display(Name = "Puntos para canje")]
        [Range(0, 99999999, ErrorMessage = "Debe seleccion un {0} entre {1} y {2}")]
        public int ChangePoint { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [Range(1, double.MaxValue, ErrorMessage = "Debe seleccionar un {0}")]
        [Display(Name = "Unidad de Medida")]
        public int MeasureId { get; set; }

        [Display(Name = "Verifica Stock")]
        public bool StockCheck { get; set; }

        [Display(Name = "Es Pesable")]
        public bool IsWeight { get; set; }

        [Display(Name = "Stock")]
        [DisplayFormat(DataFormatString = "{0:N4}", ApplyFormatInEditMode = false)]
        public decimal Stock { get; set; }

        [Display(Name = "Stock Mínimo")]
        [DisplayFormat(DataFormatString = "{0:N4}", ApplyFormatInEditMode = false)]
        public decimal MinimalStock { get; set; }

        [Display(Name = "Reposición")]
        [DisplayFormat(DataFormatString = "{0:N4}", ApplyFormatInEditMode = false)]
        public decimal Replacement { get; set; }

        [Display(Name = "Umbral")]
        [DisplayFormat(DataFormatString = "{0:N4}", ApplyFormatInEditMode = false)]
        public decimal Umbral { get; set; }

        [Display(Name = "Aclaraciones")]
        public string? Clarifications { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [Range(1, double.MaxValue, ErrorMessage = "Debe seleccionar un {0}")]
        [Display(Name = "Rubro")]
        public int CategoryId { get; set; }

        [Display(Name = "Observaciones")]
        [DataType(DataType.MultilineText)]
        public string? Remark { get; set; }

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


        public List<PriceListItemDto> PriceLists { get; set; } = [];
        public List<TaxePriceDto> TaxesPrice { get; set; } = [];

        public string Category { get; set; } = string.Empty;
        public string CategoryColor { get; set; } = string.Empty;

        //[DisplayFormat(DataFormatString = "{0:C4}", ApplyFormatInEditMode = false)]
        public decimal CostWithTaxes
        {
            get => _costWithTaxes;
            set
            {
                if (_costWithTaxes != value)
                {
                    _costWithTaxes = value;
                    OnPropertyChanged(nameof(CostWithTaxes));
                    RecalculateFromCostWithTaxes();
                }
            }
        }

        public decimal SalePrice
        {
            get => _salePrice;
            set
            {
                if (_salePrice != value)
                {
                    _salePrice = value;
                    OnPropertyChanged(nameof(SalePrice));
                    RecalculateUtilityFromSalePrice();
                }
            }
        }

        public decimal SalePriceWithTaxes
        {
            get => _salePriceWithTaxes;
            set
            {
                if (_salePriceWithTaxes != value)
                {
                    _salePriceWithTaxes = value;
                    OnPropertyChanged(nameof(SalePriceWithTaxes));
                    RecalculateUtilityFromSalePriceWithTaxes();
                }
            }
        }

        private bool _isUpdating = false;

        private void RecalculateFromCost()
        {
            if (_isUpdating || TaxId == 0 || Taxes == null) return;
            _isUpdating = true;

            Tax? tax = Taxes.FirstOrDefault(t => t.Id == TaxId);
            if (tax != null)
            {
                decimal bonifFactor = 1m - (_bonification / 100m);
                decimal internalFactor = 1m + (_internalTax / 100m);
                decimal ivaFactor = 1m + (tax.Rate / 100m);
                decimal utilityFactor = 1m + (_utility / 100m);

                _realCost = Round2(_cost * bonifFactor * internalFactor);
                _costWithTaxes = Round2(_realCost * ivaFactor);

                _salePrice = Round2(_realCost * utilityFactor);
                _salePriceWithTaxes = Round2(_salePrice * ivaFactor);

                NotifyAll(nameof(RealCost), nameof(CostWithTaxes), nameof(SalePrice), nameof(SalePriceWithTaxes));

            }
            _isUpdating = false;
        }

        private void RecalculateFromRealCost()
        {
            if (_isUpdating || TaxId == 0 || Taxes == null) return;
            _isUpdating = true;
            Tax? tax = Taxes.FirstOrDefault(t => t.Id == TaxId);
            if (tax != null)
            {
                decimal bonifFactor = 1m - (_bonification / 100m);
                decimal internalFactor = 1m + (_internalTax / 100m);
                decimal ivaFactor = 1m + (tax.Rate / 100m);
                decimal utilityFactor = 1m + (_utility / 100m);

                decimal denom = bonifFactor * internalFactor;
                _cost = denom != 0m ? Round2(_realCost / denom) : 0m;

                _costWithTaxes = Round2(_realCost * ivaFactor);

                _salePrice = Round2(_realCost * utilityFactor);
                _salePriceWithTaxes = Round2(_salePrice * ivaFactor);

                NotifyAll(nameof(Cost), nameof(CostWithTaxes), nameof(SalePrice), nameof(SalePriceWithTaxes));

            }
            _isUpdating = false;
        }

        private void RecalculateFromCostWithTaxes()
        {
            if (_isUpdating || TaxId == 0 || Taxes == null) return;
            _isUpdating = true;
            Tax? tax = Taxes.FirstOrDefault(t => t.Id == TaxId);
            if (tax != null)
            {
                decimal ivaFactor = 1m + (tax.Rate / 100m);
                decimal bonifFactor = 1m - (_bonification / 100m);
                decimal internalFactor = 1m + (_internalTax / 100m);
                decimal utilityFactor = 1m + (_utility / 100m);

                _realCost = Round2(_costWithTaxes / ivaFactor);

                decimal denom = bonifFactor * internalFactor;
                _cost = denom != 0m ? Round2(_realCost / denom) : 0m;

                _salePrice = Round2(_realCost * utilityFactor);
                _salePriceWithTaxes = Round2(_salePrice * ivaFactor);

                NotifyAll(nameof(RealCost), nameof(Cost), nameof(SalePrice), nameof(SalePriceWithTaxes));

            }
            _isUpdating = false;
        }

        private void RecalculateSalePricesFromUtility()
        {
            if (_isUpdating || TaxId == 0 || Taxes == null) return;
            _isUpdating = true;
            Tax? tax = Taxes.FirstOrDefault(t => t.Id == TaxId);
            if (tax != null)
            {
                decimal ivaFactor = 1m + (tax.Rate / 100m);
                decimal utilityFactor = 1m + (_utility / 100m);

                _salePrice = Round2(_realCost * utilityFactor);
                _salePriceWithTaxes = Round2(_salePrice * ivaFactor);

                NotifyAll(nameof(SalePrice), nameof(SalePriceWithTaxes));
            }
            _isUpdating = false;
        }

        private void RecalculateUtilityFromSalePrice()
        {
            if (_isUpdating || TaxId == 0 || Taxes == null) return;
            _isUpdating = true;
            Tax? tax = Taxes.FirstOrDefault(t => t.Id == TaxId);
            if (tax != null)
            {
                decimal ivaFactor = 1m + (tax.Rate / 100m);
                decimal utilityFactor = 1m + (_utility / 100m);
                // Si el costo está en 0, lo calculamos a partir del SalePrice
                if (_cost == 0m)
                {
                    decimal bonifFactor = 1m - (_bonification / 100m);
                    decimal internalFactor = 1m + (_internalTax / 100m);

                    _realCost = Round2(_salePrice / utilityFactor);
                    _cost = bonifFactor * internalFactor != 0m ? Round2(_realCost / (bonifFactor * internalFactor)) : 0m;
                    _costWithTaxes = Round2(_realCost * ivaFactor);
                }
                _utility = _realCost != 0m ? Round2((_salePrice / _realCost - 1m) * 100m) : 0m;
                _salePriceWithTaxes = Round2(_salePrice * ivaFactor);

                NotifyAll(nameof(Utility), nameof(SalePriceWithTaxes), nameof(RealCost), nameof(Cost), nameof(CostWithTaxes));

            }
            _isUpdating = false;
        }

        private void RecalculateUtilityFromSalePriceWithTaxes()
        {
            if (_isUpdating || TaxId == 0 || Taxes == null) return;
            _isUpdating = true;
            Tax? tax = Taxes.FirstOrDefault(t => t.Id == TaxId);
            if (tax != null)
            {
                decimal ivaFactor = 1m + (tax.Rate / 100m);
                decimal utilityFactor = 1m + (_utility / 100m);
                _salePrice = Round2(_salePriceWithTaxes / ivaFactor);
                // Si el costo está en 0, lo calculamos a partir del SalePrice
                if (_cost == 0m)
                {
                    decimal bonifFactor = 1m - (_bonification / 100m);
                    decimal internalFactor = 1m + (_internalTax / 100m);

                    _realCost = Round2(_salePrice / utilityFactor);
                    _cost = bonifFactor * internalFactor != 0m ? Round2(_realCost / (bonifFactor * internalFactor)) : 0m;
                    _costWithTaxes = Round2(_realCost * ivaFactor);
                }
                
                
                _utility = _realCost != 0m ? Round2((_salePrice / _realCost - 1m) * 100m) : 0m;

                NotifyAll(nameof(SalePrice), nameof(Utility), nameof(RealCost), nameof(Cost), nameof(CostWithTaxes));
            }
            _isUpdating = false;
        }

        // Auxiliar
        private decimal Round2(decimal value) => decimal.Round(value, 4, MidpointRounding.AwayFromZero);

        private void NotifyAll(params string[] propertyNames)
        {
            foreach (var name in propertyNames)
                OnPropertyChanged(name);
        }

        private void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        public virtual ICollection<Tax> Taxes { get; set; }
        public virtual ICollection<Measure> Measures { get; set; }
        public virtual ICollection<Category> Categories { get; set; }
    }


    public class PriceListItemDto
    {
        public int Id { get; set; }
        public string Description { get; set; } = string.Empty;
        public decimal Utility { get; set; }
        public decimal FinalPrice { get; set; }
    }

    public class TaxePriceDto
    {
        public string Description { get; set; } = string.Empty;
        public decimal Utility { get; set; }
        public decimal Price { get; set; }
    }

}
