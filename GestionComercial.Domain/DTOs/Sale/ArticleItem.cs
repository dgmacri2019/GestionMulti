using GestionComercial.Domain.DTOs.Stock;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace GestionComercial.Domain.DTOs.Sale
{
    public class ArticleItem : ObservableObject
    {
        private string _code;
        private string _description;
        private string _smallMeasureDescription;
        private int _priceListId;
        private decimal _price;
        private decimal _quantity = 1;
        private decimal _bonification;
        private decimal _subtotal;
        private decimal _total;

        public string Code { get => _code; set => SetProperty(ref _code, value); }
        public string Description { get => _description; set => SetProperty(ref _description, value); }
        public string SmallMeasureDescription { get => _smallMeasureDescription; set => SetProperty(ref _smallMeasureDescription, value); }

        // Colección por fila con las price lists que trae el artículo
        public ObservableCollection<PriceListItemDto> PriceLists { get; set; } = [];

        public int PriceListId
        {
            get => _priceListId;
            set
            {
                if (SetProperty(ref _priceListId, value))
                {
                    var found = PriceLists.FirstOrDefault(p => p.Id == value);
                    if (found != null)
                        Price = found.FinalPrice;

                    Recalculate();
                }
            }
        }

        public decimal Price { get => _price; set { if (SetProperty(ref _price, value)) Recalculate(); } }
        public decimal Quantity { get => _quantity; set { if (SetProperty(ref _quantity, value)) Recalculate(); } }
        public decimal Bonification 
        { 
            get => _bonification; 
            set 
            { 
                if (SetProperty(ref _bonification, value)) 
                    Recalculate(); 
            } 
        }

        public decimal Subtotal { get => _subtotal; private set => SetProperty(ref _subtotal, value); }
        public decimal Total { get => _total; private set => SetProperty(ref _total, value); }

        public void Recalculate()
        {
            Subtotal = Price * Quantity;
            Total = Subtotal - (Subtotal * Bonification / 100m);
        }
    }

    public class ObservableObject : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
        {
            if (Equals(storage, value)) return false;
            storage = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            return true;
        }
    }
}