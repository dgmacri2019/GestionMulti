using GestionComercial.Desktop.Helpers;
using GestionComercial.Domain.DTOs.Sale;
using GestionComercial.Domain.Entities.Masters;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

namespace GestionComercial.Desktop.Views.Sales
{
    public partial class PayMethodWindow : Window
    {
        private static readonly char DecSep = CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator[0];
        private static readonly char OtherSep = (DecSep == ',') ? '.' : ',';

        public ObservableCollection<PayMethodItem> MetodosPago { get; set; }
        public List<SaleCondition> AvailableMetodos { get; set; }

        private readonly decimal _totalVenta;

        public PayMethodWindow(decimal totalVenta)
        {
            InitializeComponent();
            _totalVenta = totalVenta;

            MetodosPago = [];
            AvailableMetodos =
            [
                new SaleCondition { Id = 4, SmallDescription = "EF", Description = "Efectivo" },
                new SaleCondition { Id = 17, SmallDescription = "MP", Description = "Mercado Pago" },
                new SaleCondition { Id = 7, SmallDescription = "TJ", Description = "Tarjeta" }
            ];

            DataContext = this;


            // ✅ Prellenar con efectivo por el total
            MetodosPago.Add(new PayMethodItem
            {
                MetodoId = 4,
                MetodoCodigo = "EF",
                MetodoDescripcion = "Efectivo",
                Monto = totalVenta
            });
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

        #region Monto - Validaciones

        private void Monto_GotFocus(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox textBox)
                textBox.SelectAll();
        }

        private void Monto_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (sender is not TextBox tb) return;

            if (e.Key == Key.Decimal || e.Key == Key.OemPeriod || e.Key == Key.OemComma)
            {
                e.Handled = true;

                int selStart = tb.SelectionStart;
                int selLen = tb.SelectionLength;

                string current = tb.Text ?? string.Empty;
                //string preview = current.Substring(0, selStart) + DecSep + current.Substring(Math.Min(current.Length, selStart + selLen));
                string preview = current.Substring(0, selStart) + DecSep + current.Substring(Math.Min(current.Length, selStart + selLen));

                if (preview.Count(ch => ch == DecSep) > 1) return;

                tb.SelectedText = DecSep.ToString();
                tb.SelectionStart = selStart + 1;
                tb.SelectionLength = 0;
            }
        }
        private void Monto_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (sender is not TextBox tb || string.IsNullOrEmpty(e.Text)) return;

            string input = (e.Text == "." || e.Text == ",") ? DecSep.ToString() : e.Text;

            foreach (char ch in input)
                if (!char.IsDigit(ch) && ch != DecSep) { e.Handled = true; return; }

            int selStart = tb.SelectionStart;
            int selLen = tb.SelectionLength;
            string current = tb.Text ?? string.Empty;
            //string preview = current.Substring(0, selStart) + input + current.Substring(Math.Min(current.Length, selStart + selLen));
            string preview = current.Substring(0, selStart) + input + current.Substring(Math.Min(current.Length, selStart + selLen));

            if (preview.Count(ch => ch == DecSep) > 1) { e.Handled = true; return; }

            e.Handled = true;
            tb.SelectedText = input;
            tb.SelectionStart = selStart + input.Length;
            tb.SelectionLength = 0;
        }
        private void Monto_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (sender is not TextBox tb) return;

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

        #endregion

        private void dgMetodosPago_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            if (e.Column.Header?.ToString() == "Método")
            {
                var currentItem = e.Row.Item as PayMethodItem;
                if (currentItem == null) return;

                var metodo = AvailableMetodos
                    .FirstOrDefault(m => string.Equals(m.SmallDescription, currentItem.MetodoCodigo, StringComparison.OrdinalIgnoreCase));

                if (metodo != null)
                {
                    currentItem.MetodoDescripcion = metodo.Description;
                    currentItem.MetodoId = metodo.Id;
                    // ✅ Poner foco en la celda Monto
                    Dispatcher.BeginInvoke(new Action(() =>
                    {
                        var dataGrid = (DataGrid)sender;
                        dataGrid.CurrentCell = new DataGridCellInfo(
                            currentItem,
                            dataGrid.Columns.First(c => c.Header?.ToString() == "Monto")
                        );
                        dataGrid.BeginEdit();
                    }), DispatcherPriority.Background);
                }
                else
                {
                    MsgBoxAlertHelper.MsgAlertError("Metodo de pago inválido");
                    // ✅ Poner foco en la celda Método
                    Dispatcher.BeginInvoke(new Action(() =>
                    {
                        var dataGrid = (DataGrid)sender;
                        dataGrid.CurrentCell = new DataGridCellInfo(
                            currentItem,
                            dataGrid.Columns.First(c => c.Header?.ToString() == "Método")
                        );
                        dataGrid.BeginEdit();
                    }), DispatcherPriority.Background);
                }
            }            
        }

        private void PayMethodWin_Loaded(object sender, RoutedEventArgs e)
        {
            if (dgMetodosPago.Items.Count > 0)
            {
                dgMetodosPago.SelectedIndex = 0;
                dgMetodosPago.CurrentCell = new DataGridCellInfo(
                    dgMetodosPago.Items[0],
                    dgMetodosPago.Columns[0]); // primera columna = Método
                dgMetodosPago.BeginEdit();
            }
        }

        private void PayMethodWin_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                this.DialogResult = false;
                this.Close();
            }
            else if (e.Key == Key.F2)
            {
                this.DialogResult = true;
                this.Close();
            }
            
        }
    }
}
