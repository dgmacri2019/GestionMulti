using GestionComercial.Domain.DTOs.Sale;
using System.Collections.ObjectModel;
using System.Windows;

namespace GestionComercial.Desktop.Views.Sales
{
    /// <summary>
    /// Lógica de interacción para PayMethodWindow.xaml
    /// </summary>
    public partial class PayMethodWindow : Window
    {
        public ObservableCollection<PayMethodItem> MetodosPago { get; set; }
        public List<string> AvailableMetodos { get; set; }

        private readonly decimal _totalVenta;


        public PayMethodWindow(decimal totalVenta)
        {
            InitializeComponent();
            _totalVenta = totalVenta;

            MetodosPago = [];
            AvailableMetodos = new List<string> { "Efectivo", "MercadoPago", "Tarjeta" };

            DataContext = this;

            // Prellenar con una fila inicial en efectivo por el total
            MetodosPago.Add(new PayMethodItem { Metodo = "Efectivo", Monto = totalVenta });
        }

        private void Aceptar_Click(object sender, RoutedEventArgs e)
        {
            var suma = MetodosPago.Sum(m => m.Monto);
            if (suma != _totalVenta)
            {
                MessageBox.Show($"La suma de los métodos de pago ({suma:C2}) no coincide con el total de la venta ({_totalVenta:C2}).",
                    "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            this.DialogResult = true;
            this.Close();
        }

        private void Cancelar_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }
    }
}

