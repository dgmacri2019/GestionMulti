using GestionComercial.Desktop.Helpers;
using GestionComercial.Domain.DTOs.Sale;
using GestionComercial.Domain.Entities.Masters;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

namespace GestionComercial.Desktop.Views.Sales
{
    public partial class PayMethodWindow : Window, INotifyPropertyChanged
    {
        private static readonly char DecSep = CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator[0];
        private static readonly char OtherSep = (DecSep == ',') ? '.' : ',';

        public ObservableCollection<PayMethodItem> MetodosPago { get; set; }
        public List<SaleCondition> AvailableMetodos { get; set; }

        private decimal _totalVenta;

        public decimal TotalVenta
        {
            get => _totalVenta;
            set { _totalVenta = value; OnPropertyChanged(); OnPropertyChanged(nameof(TotalVenta)); }
        }

        private decimal _totalIngresado;
        public decimal TotalIngresado
        {
            get => _totalIngresado;
            set { _totalIngresado = value; OnPropertyChanged(); OnPropertyChanged(nameof(Saldo)); }
        }
        public decimal Saldo => _totalVenta - TotalIngresado;

        public PayMethodWindow(decimal totalVenta)
        {
            InitializeComponent();
            _totalVenta = totalVenta;

            MetodosPago = new ObservableCollection<PayMethodItem>();
            MetodosPago.CollectionChanged += MetodosPago_CollectionChanged;

            AvailableMetodos = new List<SaleCondition>
            {
                new SaleCondition { Id = 4, SmallDescription = "EF", Description = "Efectivo" },
                new SaleCondition { Id = 17, SmallDescription = "MP", Description = "Mercado Pago" },
                new SaleCondition { Id = 7, SmallDescription = "TJ", Description = "Tarjeta" }
            };

            DataContext = this;

            // Prellenar con efectivo por el total
            MetodosPago.Add(new PayMethodItem
            {
                MetodoId = 4,
                MetodoCodigo = "EF",
                MetodoDescripcion = "Efectivo",
                Monto = totalVenta
            });

            RecalcularTotales();
        }

        #region Collection / Totales
        private void MetodosPago_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.OldItems != null)
            {
                foreach (PayMethodItem oldItem in e.OldItems)
                    oldItem.PropertyChanged -= PayMethodItem_PropertyChanged;
            }

            if (e.NewItems != null)
            {
                foreach (PayMethodItem newItem in e.NewItems)
                    newItem.PropertyChanged += PayMethodItem_PropertyChanged;
            }

            RecalcularTotales();
        }

        private void PayMethodItem_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(PayMethodItem.Monto))
            {
                // cuando cambia monto recalc
                RecalcularTotales();
            }
        }

        private void RecalcularTotales()
        {
            TotalIngresado = MetodosPago.Sum(x => x.Monto);
        }
        #endregion

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

        #region Monto - Validaciones y eventos
        private void Monto_GotFocus(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox textBox)
                textBox.SelectAll();
        }

        private void Monto_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (sender is not TextBox tb) return;

            // Si presionó ENTER -> commit y mover foco a Método siguiente
            if (e.Key == Key.Enter)
            {
                e.Handled = true;

                // Commit de la celda/row para forzar que la binding actualice la propiedad Monto
                dgMetodosPago.CommitEdit(DataGridEditingUnit.Cell, true);
                dgMetodosPago.CommitEdit();

                var dataGrid = dgMetodosPago;
                int rowIndex = dataGrid.Items.IndexOf(tb.DataContext);

                // si hay siguiente fila real (no el placeholder final)
                if (rowIndex >= 0 && rowIndex < dataGrid.Items.Count - 1)
                {
                    var nextItem = dataGrid.Items[rowIndex + 1];
                    Dispatcher.BeginInvoke(new Action(() =>
                    {
                        dataGrid.CurrentCell = new DataGridCellInfo(
                            nextItem,
                            dataGrid.Columns.First(c => c.Header?.ToString() == "Método")
                        );
                        dataGrid.BeginEdit();
                    }), DispatcherPriority.Input);
                }
                return;
            }

            if (e.Key == Key.Decimal || e.Key == Key.OemPeriod || e.Key == Key.OemComma)
            {
                e.Handled = true;

                int selStart = tb.SelectionStart;
                int selLen = tb.SelectionLength;

                string current = tb.Text ?? string.Empty;
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

            string input = e.Text;

            // Normalizar separador: cualquier "." o "," -> DecSep
            if (input == "." || input == ",")
                input = DecSep.ToString();

            // Rechazar si no es dígito ni el separador decimal
            if (!char.IsDigit(input[0]) && input[0] != DecSep)
            {
                e.Handled = true;
                return;
            }

            // Previsualizar resultado
            int selStart = tb.SelectionStart;
            int selLen = tb.SelectionLength;
            string current = tb.Text ?? string.Empty;
            string preview = current.Substring(0, selStart) + input + current.Substring(Math.Min(current.Length, selStart + selLen));

            // Rechazar si ya habría más de un separador decimal
            if (preview.Count(ch => ch == DecSep) > 1)
            {
                e.Handled = true;
            }
        }


        private void Monto_TextChanged(object sender, TextChangedEventArgs e)
        {
            // mantenemos el reemplazo de separador
            if (sender is not TextBox tb) return;

            int idx = tb.Text.IndexOf(OtherSep);
            if (idx >= 0)
            {
                int caret = tb.SelectionStart;
                tb.Text = tb.Text.Replace(OtherSep, DecSep);
                tb.SelectionStart = Math.Min(caret, tb.Text.Length);
            }

            // NO forzamos recálculo aquí: el recálculo real ocurre desde la propiedad Monto (PropertyChanged)
            // Sin embargo, si querés recalcular mientras se escribe, descomenta:
            // RecalcularTotales();
        }

        private void Monto_Pasting(object sender, DataObjectPastingEventArgs e)
        {
            if (sender is not TextBox tb) return;
            if (!e.DataObject.GetDataPresent(DataFormats.Text)) { e.CancelCommand(); return; }
            string pasted = (e.DataObject.GetData(DataFormats.Text) as string) ?? string.Empty;

            // Normalizar separadores y filtrar: dígitos + 1 separador
            pasted = pasted.Replace(OtherSep, DecSep);
            var clean = new StringBuilder();
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

        #region DataGrid cell edit / validation
        private void dgMetodosPago_PreparingCellForEdit(object sender, DataGridPreparingCellForEditEventArgs e)
        {
            // Asociar handler Pasting solo al TextBox de la columna Monto
            if (e.Column.Header?.ToString() == "Monto" && e.EditingElement is TextBox tbMonto)
            {
                // Asegurar que no se agregue dos veces
                DataObject.RemovePastingHandler(tbMonto, new DataObjectPastingEventHandler(Monto_Pasting));
                DataObject.AddPastingHandler(tbMonto, new DataObjectPastingEventHandler(Monto_Pasting));
            }

            // Si querés forzar mayúsculas mientras se escribe en "Método", podés enganchar TextChanged aquí.
            // No lo hice por ahora porque el setter de PayMethodItem ya hace ToUpper() al asignar.
        }

        private void dgMetodosPago_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            var dataGrid = (DataGrid)sender;

            if (e.Column.Header?.ToString() == "Monto" &&
                Keyboard.IsKeyDown(Key.Enter))
            {
                int rowIndex = dataGrid.Items.IndexOf(e.Row.Item);
                if (rowIndex < dataGrid.Items.Count - 1)
                {
                    var nextItem = dataGrid.Items[rowIndex + 1];
                    Dispatcher.BeginInvoke(new Action(() =>
                    {
                        dataGrid.CurrentCell = new DataGridCellInfo(
                            nextItem,
                            dataGrid.Columns.First(c => c.Header?.ToString() == "Método")
                        );
                        dataGrid.BeginEdit();
                    }), DispatcherPriority.Background);
                }
            }
            else if (e.Column.Header?.ToString() == "Método")
            {
                var currentItem = e.Row.Item as PayMethodItem;
                if (currentItem == null) return;
                // Si está vacío (fila nueva o dejo vacío), no mostramos error; limpiamos descripción
                if (string.IsNullOrWhiteSpace(currentItem.MetodoCodigo))
                {
                    currentItem.MetodoDescripcion = null;
                    currentItem.MetodoId = 0;
                    return;
                }
                // forzar mayúsculas
                if (!string.IsNullOrEmpty(currentItem.MetodoCodigo))
                    currentItem.MetodoCodigo = currentItem.MetodoCodigo.ToUpper();

                var metodo = AvailableMetodos
                    .FirstOrDefault(m => string.Equals(m.SmallDescription, currentItem.MetodoCodigo, StringComparison.OrdinalIgnoreCase));

                if (metodo != null)
                {
                    currentItem.MetodoDescripcion = metodo.Description;
                    currentItem.MetodoId = metodo.Id;

                    Dispatcher.BeginInvoke(new Action(() =>
                    {
                        dataGrid.CurrentCell = new DataGridCellInfo(
                            currentItem,
                            dataGrid.Columns.First(c => c.Header?.ToString() == "Monto")
                        );
                        dataGrid.BeginEdit();
                    }), DispatcherPriority.Background);
                }
                else
                {
                    MsgBoxAlertHelper.MsgAlertError("Método de pago inválido");
                    Dispatcher.BeginInvoke(new Action(() =>
                    {
                        dataGrid.CurrentCell = new DataGridCellInfo(
                            currentItem,
                            dataGrid.Columns.First(c => c.Header?.ToString() == "Método")
                        );
                        dataGrid.BeginEdit();
                    }), DispatcherPriority.Background);
                }
            }
        }
        #endregion

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

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        #endregion
    }
}
