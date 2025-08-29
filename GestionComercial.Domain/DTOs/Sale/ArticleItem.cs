using GestionComercial.Domain.DTOs.Stock;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace GestionComercial.Domain.DTOs.Sale
{
    public class ArticleItem : INotifyPropertyChanged
    {
        private string code;
        public string Code
        {
            get => code;
            set { code = value; OnPropertyChanged(); LoadFromCache(); }
        }

        public string Description { get; set; }
        public int PriceListId { get; set; }
        public decimal Price { get; set; }

        private int quantity = 1;
        public int Quantity
        {
            get => quantity;
            set { quantity = value; OnPropertyChanged(); UpdateTotals(); }
        }

        private decimal discount = 0;
        public decimal Discount
        {
            get => discount;
            set { discount = value; OnPropertyChanged(); UpdateTotals(); }
        }

        private decimal subtotal;
        public decimal Subtotal
        {
            get => subtotal;
            set { subtotal = value; OnPropertyChanged(); }
        }

        private decimal total;
        public decimal Total
        {
            get => total;
            set { total = value; OnPropertyChanged(); }
        }

        public void UpdateTotals()
        {
            Subtotal = Price * Quantity;
            Total = Subtotal * (1 - Discount / 100);
        }

        // Simula la búsqueda en cache de artículos por código
        private void LoadFromCache()
        {
            if (string.IsNullOrWhiteSpace(Code)) return;

            //ArticleViewModel? article = ArticleCache.Instance.FindByCodeOrBarCode(Code); // Implementa tu cache real
            //if (article != null)
            //{
            //    // var salePrice = article.

            //    Description = article.Description;
            //    Price = 10;
            //    OnPropertyChanged(nameof(Description));
            //    OnPropertyChanged(nameof(Price));
            //    UpdateTotals();
            //}
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
