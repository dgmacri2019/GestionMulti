using GestionComercial.Domain.DTOs.Sale;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace GestionComercial.Desktop.Views.Sales
{
    /// <summary>
    /// Lógica de interacción para PayMethodWindow.xaml
    /// </summary>
    public partial class PayMethodWindow : Window
    {
        // Separador decimal según cultura actual (si querés forzar coma: const char DecSep = ',';)
        private static readonly char DecSep = CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator[0];
        private static readonly char OtherSep = (DecSep == ',') ? '.' : ',';


        public ObservableCollection<PayMethodItem> MetodosPago { get; set; }
        public List<MetodoPago> AvailableMetodos { get; set; }

        private readonly decimal _totalVenta;


        public PayMethodWindow(decimal totalVenta)
        {
            InitializeComponent();
            _totalVenta = totalVenta;

            MetodosPago = [];
            AvailableMetodos =
            [
                new MetodoPago { Id = 4, Nombre = "Efectivo" },
                new MetodoPago { Id = 17, Nombre = "MercadoPago" },
                new MetodoPago { Id = 7, Nombre = "Tarjeta" }
            ];

            DataContext = this;

            // Prellenar con una fila inicial en efectivo por el total
            MetodosPago.Add(new PayMethodItem { MetodoId = 4, Monto = totalVenta });
        }

        private void Aceptar_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            this.Close();
        }

        private void Cancelar_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }


        private void Monto_GotFocus(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox textBox)
            {
                // selecciona todo el texto, para que al tipear se borre
                textBox.SelectAll();
            }
        }
        private void Monto_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (sender is not TextBox tb) return;

            // Teclas de separador decimal que a veces no disparan TextInput
            if (e.Key == Key.Decimal || e.Key == Key.OemPeriod || e.Key == Key.OemComma)
            {
                e.Handled = true;

                int selStart = tb.SelectionStart;
                int selLen = tb.SelectionLength;

                string current = tb.Text ?? string.Empty;
                string preview = current.Substring(0, selStart) + DecSep + current.Substring(Math.Min(current.Length, selStart + selLen));

                // impedir más de un separador
                if (preview.Count(ch => ch == DecSep) > 1) return;

                tb.SelectedText = DecSep.ToString();
                tb.SelectionStart = selStart + 1;
                tb.SelectionLength = 0;
            }
        }
        private void Monto_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (sender is not TextBox tb || string.IsNullOrEmpty(e.Text)) return;

            // Normalizar '.'/',' al separador decimal de la cultura
            string input = (e.Text == "." || e.Text == ",") ? DecSep.ToString() : e.Text;

            // Aceptar solo dígitos o el separador
            foreach (char ch in input)
                if (!char.IsDigit(ch) && ch != DecSep) { e.Handled = true; return; }

            int selStart = tb.SelectionStart;
            int selLen = tb.SelectionLength;
            string current = tb.Text ?? string.Empty;
            string preview = current.Substring(0, selStart) + input + current.Substring(Math.Min(current.Length, selStart + selLen));

            // impedir más de un separador
            if (preview.Count(ch => ch == DecSep) > 1) { e.Handled = true; return; }

            // Insertar manualmente y marcar handled
            e.Handled = true;
            tb.SelectedText = input;
            tb.SelectionStart = selStart + input.Length;
            tb.SelectionLength = 0;
        }
        private void Monto_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (sender is not TextBox tb) return;

            // Si se coló el separador "equivocado", normalizá
            int idx = tb.Text.IndexOf(OtherSep);
            if (idx >= 0)
            {
                int caret = tb.SelectionStart;
                tb.Text = tb.Text.Replace(OtherSep, DecSep);
                tb.SelectionStart = Math.Min(caret, tb.Text.Length);
            }
        }
        private void Monto_Pasting(object sender, DataObjectPastingEventArgs e)
        {
            if (sender is not TextBox tb) return;
            if (!e.DataObject.GetDataPresent(DataFormats.Text)) { e.CancelCommand(); return; }

            string pasted = (e.DataObject.GetData(DataFormats.Text) as string) ?? string.Empty;

            // Normalizar separadores y filtrar: dígitos + 1 separador
            pasted = pasted.Replace(OtherSep, DecSep);
            var clean = new System.Text.StringBuilder();
            bool hasSep = (tb.Text?.Count(ch => ch == DecSep) ?? 0) - (tb.SelectedText?.Count(ch => ch == DecSep) ?? 0) > 0;

            foreach (char ch in pasted)
            {
                if (char.IsDigit(ch)) clean.Append(ch);
                else if (ch == DecSep && !hasSep) { clean.Append(ch); hasSep = true; }
            }

            string final = clean.ToString();
            if (string.IsNullOrEmpty(final)) { e.CancelCommand(); return; }

            e.CancelCommand();
            int selStart = tb.SelectionStart;
            tb.SelectedText = final;
            tb.SelectionStart = selStart + final.Length;
            tb.SelectionLength = 0;
        }


    }
}

