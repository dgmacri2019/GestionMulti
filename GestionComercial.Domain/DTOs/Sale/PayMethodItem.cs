namespace GestionComercial.Domain.DTOs.Sale
{
    public class PayMethodItem : ObservableObject
    {
        private string _metodo;
        private decimal _monto;
        private string _caja; // se asigna automáticamente según el método

        public string Metodo
        {
            get => _metodo;
            set
            {
                if (SetProperty(ref _metodo, value))
                {
                    // Asignar caja automáticamente
                    _caja = value switch
                    {
                        "Efectivo" => "Caja Efectivo",
                        "MercadoPago" => "Caja MercadoPago",
                        "Tarjeta" => "Caja Tarjeta",
                        _ => "Caja General"
                    };
                    OnPropertyChanged(nameof(Caja));
                }
            }
        }

        public string Caja => _caja; // Solo lectura

        public decimal Monto
        {
            get => _monto;
            set => SetProperty(ref _monto, value);
        }
    }
}