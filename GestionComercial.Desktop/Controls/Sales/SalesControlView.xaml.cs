using GestionComercial.Desktop.Services;
using GestionComercial.Desktop.Views.Searchs;
using GestionComercial.Domain.Cache;
using GestionComercial.Domain.DTOs.Client;
using GestionComercial.Domain.DTOs.Sale;
using GestionComercial.Domain.DTOs.Stock;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

namespace GestionComercial.Desktop.Controls.Sales
{
    public partial class SalesControlView : UserControl, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        private readonly SalesApiService _salesApiService;
        private readonly int SaleId;
        private SaleViewModel saleViewModel;
        private readonly bool UsePostMethod;
        public ObservableCollection<ArticleItem> ArticleItems { get; set; }
        public int TotalItems => ArticleItems.Count(a => !string.IsNullOrEmpty(a.Code));
        public decimal TotalPrice => ArticleItems.Sum(a => a.Total);


        // public ObservableCollection<ArticleItem> ArticleItems = new ObservableCollection<ArticleItem>();

        public SalesControlView(int saleId)
        {
            InitializeComponent();
            _salesApiService = new SalesApiService();
            DataContext = this;
            SaleId = saleId;
            ArticleItems = new ObservableCollection<ArticleItem>();
            ArticleItems.CollectionChanged += (s, e) =>
            {
                OnPropertyChanged(nameof(TotalItems));
                OnPropertyChanged(nameof(TotalPrice));
            };

            UsePostMethod = ParameterCache.Instance.GetAllGeneralParameters().First().UsePostMethod;

            //if (!UsePostMethod)
            //{
            //    // Agregar fila inicial en blanco
            //    ArticleItems.Add(new ArticleItem());
            //    //chBarcode.IsChecked = false;
            //}
            _ = LoadSaleAsync();

            btnAdd.Visibility = SaleId == 0 ? Visibility.Visible : Visibility.Hidden;
            btnUpdate.Visibility = SaleId == 0 ? Visibility.Hidden : Visibility.Visible;
        }

        private async Task LoadSaleAsync()
        {
            var result = await _salesApiService.GetByIdAsync(SaleId);
            if (result.Success)
            {
                saleViewModel = result.SaleViewModel;
                DataContext = saleViewModel;
            }
            else
            {
                lblError.Text = result.Message;
            }
        }

        private void txtClientCode_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                ClearClient();
                ClientViewModel? client = ClientCache.Instance.FindClientByOptionalCode(txtClientCode.Text);
                if (client != null)
                {
                    txtFansatyName.Text = string.IsNullOrEmpty(client.FantasyName) ? client.BusinessName : client.FantasyName;
                    txtAddress.Text = $"{client.Address}\n{client.City}, {client.State}\nC.P.{client.PostalCode}";
                    txtEmail.Text = !string.IsNullOrEmpty(client.Email) ? client.Email : string.Empty;
                    chSendEmail.IsChecked = !string.IsNullOrEmpty(client.Email);

                    cbPriceLists.ItemsSource = client.PriceLists;
                    cbPriceLists.SelectedValue = client.PriceListId;

                    cbSaleConditions.ItemsSource = client.SaleConditions;
                    cbSaleConditions.SelectedValue = client.SaleConditionId;
                    spArticles.Visibility = Visibility.Visible;
                    spPostMethod.Visibility = Visibility.Visible;
                    SetingFocus();
                }
                else
                {
                    MessageBox.Show("El código informado no existe", "Aviso al operador", MessageBoxButton.OK, MessageBoxImage.Error);
                    spArticles.Visibility = Visibility.Hidden;
                    spPostMethod.Visibility = Visibility.Hidden;
                }


            }
        }

        private void ClearClient()
        {
            txtFansatyName.Text = string.Empty;
            txtAddress.Text = string.Empty;
            txtEmail.Text = string.Empty;
            chSendEmail.IsChecked = false;
            cbPriceLists.SelectedValue = 0;
            cbSaleConditions.SelectedValue = 0;
        }

        private void miUserControl_Loaded(object sender, RoutedEventArgs e)
        {
            txtClientCode.Focus();
            lblError.MaxWidth = this.ActualWidth;

            try
            {
                if (!UsePostMethod)
                {
                    // Inicializar la primera fila editable
                    if (ArticleItems.Count == 0)
                        ArticleItems.Add(new ArticleItem());
                    dgArticles.SelectedIndex = 0;
                    dgArticles.CurrentCell = new DataGridCellInfo(dgArticles.Items[0], dgArticles.Columns[0]);
                    dgArticles.BeginEdit();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Aviso al operador", MessageBoxButton.OK, MessageBoxImage.Error);

            }
            dpDate.SelectedDate = DateTime.Now;

        }


        private void dgArticles_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            try
            {
                var currentItem = e.Row.Item as ArticleItem;
                if (currentItem == null) return;

                // Actualizar binding de la celda
                if (e.EditingElement is TextBox tb)
                {
                    var binding = tb.GetBindingExpression(TextBox.TextProperty);
                    binding?.UpdateSource();
                }

                // Si cambió el código, buscar artículo
                if (e.Column.Header.ToString() == "Código" && !string.IsNullOrWhiteSpace(currentItem.Code))
                {
                    bool isProductWeight = currentItem.Code.Substring(0, 2) == "20" && currentItem.Code.Length > 8;
                    decimal quantity = 1m;

                    ArticleViewModel? article = isProductWeight ?
                        ArticleCache.Instance.FindByCodeOrBarCode(currentItem.Code.Substring(2, 4))
                        :
                        ArticleCache.Instance.FindByCodeOrBarCode(currentItem.Code);

                    if (article != null)
                    {
                        if (article.IsDeleted)
                        {
                            MessageBox.Show("Artículo Eliminado", "Aviso al operador", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }
                        if (!article.IsEnabled)
                        {
                            MessageBox.Show("Artículo no habilitado para la venta", "Aviso al operador", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }

                        var priceLists = article.PriceLists;
                        int defaultPriceListId = Convert.ToInt32(cbPriceLists.SelectedValue);

                        // Calcular cantidad en caso de producto por peso
                        if (isProductWeight)
                        {
                            if (ParameterCache.Instance.GetAllGeneralParameters().First().ProductBarCodePrice)
                            {
                                string quantityString = currentItem.Code.Substring(7, 5);
                                decimal price = priceLists.Where(pl => pl.Id == defaultPriceListId).First().FinalPrice;
                                quantity = Math.Round(Convert.ToDecimal(quantityString) / price, 3);
                            }
                            else if (ParameterCache.Instance.GetAllGeneralParameters().First().ProductBarCodeWeight)
                            {
                                string quantityString = currentItem.Code.Substring(7, 5);
                                quantity = Math.Round(Convert.ToDecimal(quantityString) / 1000, 3);
                            }
                        }

                        // Asignar datos al item
                        currentItem.SmallMeasureDescription = article.Measures
                            .First(m => m.Id == article.MeasureId).SmallDescription;
                        currentItem.Quantity = quantity;

                        // llenar PriceLists de la fila
                        currentItem.PriceLists.Clear();
                        foreach (var pl in article.PriceLists)
                            currentItem.PriceLists.Add(pl);

                        currentItem.Description = article.Description;
                        currentItem.Code = article.Code;

                        // ✅ Forzar que quede la misma lista de precios seleccionada que en el combo principal
                        if (currentItem.PriceLists.Any(pl => pl.Id == defaultPriceListId))
                            currentItem.PriceListId = defaultPriceListId;
                        else
                            currentItem.PriceListId = currentItem.PriceLists.FirstOrDefault()?.Id ?? 0;

                        currentItem.Bonification = 0;
                        currentItem.Recalculate();

                        OnPropertyChanged(nameof(TotalItems));
                        OnPropertyChanged(nameof(TotalPrice));

                        // ✅ Poner foco en la celda Cantidad
                        Dispatcher.BeginInvoke(new Action(() =>
                        {
                            var dataGrid = (DataGrid)sender;
                            dataGrid.CurrentCell = new DataGridCellInfo(
                                currentItem,
                                dataGrid.Columns.First(c => c.Header?.ToString() == "Cantidad")
                            );
                            dataGrid.BeginEdit();
                        }), DispatcherPriority.Background);


                    }
                }



                // Recalcular precio si cambia la lista de precios
                if (e.Column.Header is TextBlock headerTextBlock && headerTextBlock.Text == "Lista de Precios")
                {

                    int priceListId = currentItem.PriceListId;

                    ArticleViewModel? article = ArticleCache.Instance.FindByCodeOrBarCode(currentItem.Code);

                    if (priceListId == 0)
                    {
                        MessageBox.Show("La lista de precios no puede ser 0", "Aviso al operador", MessageBoxButton.OK, MessageBoxImage.Error);
                        Dispatcher.BeginInvoke(new Action(() =>
                        {
                            var dataGrid = (DataGrid)sender;
                            var nextRowIndex = ArticleItems.IndexOf(currentItem);
                            var priceListColumn = dataGrid.Columns
                            .FirstOrDefault(c => c.Header is TextBlock tb && tb.Text == "Lista de Precios");
                            if (nextRowIndex < ArticleItems.Count)
                            {
                                ArticleItem nextRowItem = ArticleItems[nextRowIndex];
                                dataGrid.CurrentCell = new DataGridCellInfo(nextRowItem, priceListColumn);

                                dataGrid.BeginEdit();
                            }
                        }), DispatcherPriority.Background);

                        return;
                    }

                    if(article != null && !article.PriceLists.Any(pl=>pl.Id == priceListId))
                    {
                        MessageBox.Show($"La lista de precios {priceListId}, no existe para este producto", "Aviso al operador", MessageBoxButton.OK, MessageBoxImage.Error);
                        Dispatcher.BeginInvoke(new Action(() =>
                        {
                            var dataGrid = (DataGrid)sender;
                            var nextRowIndex = ArticleItems.IndexOf(currentItem);
                            var priceListColumn = dataGrid.Columns
                            .FirstOrDefault(c => c.Header is TextBlock tb && tb.Text == "Lista de Precios");
                            if (nextRowIndex < ArticleItems.Count)
                            {
                                ArticleItem nextRowItem = ArticleItems[nextRowIndex];
                                dataGrid.CurrentCell = new DataGridCellInfo(nextRowItem, priceListColumn);

                                dataGrid.BeginEdit();
                            }
                        }), DispatcherPriority.Background);

                        return;
                    }

                    //currentItem.Recalculate();
                    //OnPropertyChanged(nameof(TotalItems));
                    //OnPropertyChanged(nameof(TotalPrice));
                    //if (ArticleItems.Last() == currentItem)
                    //    ArticleItems.Add(new ArticleItem());

                    // ✅ Poner foco en la celda Cantidad
                    Dispatcher.BeginInvoke(new Action(() =>
                    {
                        var dataGrid = (DataGrid)sender;
                        dataGrid.CurrentCell = new DataGridCellInfo(
                            currentItem,
                            dataGrid.Columns.First(c => c.Header?.ToString() == "Cantidad")
                        );
                        dataGrid.BeginEdit();
                    }), DispatcherPriority.Background);
                }

                // Recalcular subtotal y total si cambió cantidad o bonificación
                if (e.Column.Header.ToString() == "Cantidad" || e.Column.Header.ToString() == "Bonif (%)")
                {
                    currentItem.Recalculate();
                    OnPropertyChanged(nameof(TotalItems));
                    OnPropertyChanged(nameof(TotalPrice));
                    if (ArticleItems.Last() == currentItem)
                        ArticleItems.Add(new ArticleItem());
                    // ✅ Forzar foco en la celda "Código" de la fila siguiente
                    Dispatcher.BeginInvoke(new Action(() =>
                    {
                        var dataGrid = (DataGrid)sender;
                        var nextRowIndex = ArticleItems.IndexOf(currentItem) + 1;
                        if (nextRowIndex < ArticleItems.Count)
                        {
                            ArticleItem nextRowItem = ArticleItems[nextRowIndex];
                            dataGrid.CurrentCell = new DataGridCellInfo(
                                nextRowItem,
                                dataGrid.Columns.First(c => c.Header?.ToString() == "Código")
                            );
                            dataGrid.BeginEdit();
                        }
                    }), DispatcherPriority.Background);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Aviso al operador", MessageBoxButton.OK, MessageBoxImage.Error);

            }
        }

        private void dgArticles_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F5)
            {
                var currentCell = dgArticles.CurrentCell;
                if (currentCell.Column != null && currentCell.Column.Header.ToString() == "Código")
                {
                    var currentItem = currentCell.Item as ArticleItem;
                    if (currentItem == null) return;

                    string searchText = currentItem.Code;

                    var searchWindow = new ArticleSearchWindow(searchText) { Owner = Window.GetWindow(this) };
                    if (searchWindow.ShowDialog() == true)
                    {
                        var selectedArticle = searchWindow.SelectedArticle;
                        if (selectedArticle != null)
                        {
                            int priceListId = Convert.ToInt32(cbPriceLists.SelectedValue);
                            currentItem.Code = selectedArticle.Code;
                            currentItem.Description = selectedArticle.Description;
                            currentItem.Quantity = 1;

                            currentItem.PriceLists.Clear();
                            foreach (var pl in selectedArticle.PriceLists)
                                currentItem.PriceLists.Add(pl);

                            //// Forzar que quede seleccionado el mismo ID que está en el combo principal
                            var defaultPriceListId = Convert.ToInt32(cbPriceLists.SelectedValue);

                            if (currentItem.PriceLists.Any(pl => pl.Id == defaultPriceListId))
                                currentItem.PriceListId = defaultPriceListId;
                            else
                                currentItem.PriceListId = currentItem.PriceLists.FirstOrDefault()?.Id ?? 0;

                            currentItem.PriceListId = priceListId;
                            currentItem.Recalculate();
                            OnPropertyChanged(nameof(TotalItems));
                            OnPropertyChanged(nameof(TotalPrice));

                            // ✅ Forzar foco en la celda "Cantidad"
                            Dispatcher.BeginInvoke(new Action(() =>
                            {
                                var dataGrid = (DataGrid)sender;
                                dataGrid.CurrentCell = new DataGridCellInfo(currentItem, dataGrid.Columns.First(c => c.Header?.ToString() == "Cantidad"));
                                dataGrid.BeginEdit();
                            }), DispatcherPriority.Background);

                        }
                    }

                    e.Handled = true;
                }
            }
        }

        // Llamar esto desde el DataGrid PreparingCellForEdit para enganchar el handler de pegado
        private void dgArticles_PreparingCellForEdit(object sender, DataGridPreparingCellForEditEventArgs e)
        {
            // Solo para la columna Cantidad (ajustá la comparación según tu header)
            if (e.Column?.Header?.ToString() == "Cantidad" && e.EditingElement is TextBox tb)
            {
                // Evitar registros duplicados
                DataObject.RemovePastingHandler(tb, new DataObjectPastingEventHandler(Quantity_Pasting));
                DataObject.AddPastingHandler(tb, new DataObjectPastingEventHandler(Quantity_Pasting));
            }
        }
        private void txtBarcode_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                string code = txtBarcode.Text.Trim();
                if (!string.IsNullOrEmpty(code))
                {
                    bool isProductWeight = code.Substring(0, 2) == "20" && code.Length > 8;
                    decimal quantity = 1m;

                    ArticleViewModel? article = isProductWeight ?
                        ArticleCache.Instance.FindByCodeOrBarCode(code.Substring(2, 4))
                        :
                        ArticleCache.Instance.FindByCodeOrBarCode(code);

                    if (article != null)
                    {
                        //isProductWeight = article.IsWeight && code.Length > 8;
                        if (article.IsDeleted)
                        {
                            MessageBox.Show("Artículo Eliminado", "Aviso al operador", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }
                        if (!article.IsEnabled)
                        {
                            MessageBox.Show("Artículo no habilitado para la venta", "Aviso al operador", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }
                        var priceLists = article.PriceLists;
                        int priceListId = Convert.ToInt32(cbPriceLists.SelectedValue);

                        if (isProductWeight)
                        {
                            if (ParameterCache.Instance.GetAllGeneralParameters().First().ProductBarCodePrice)
                            {
                                string quantityString = code.Substring(7, 5);
                                decimal price = priceLists.Where(pl => pl.Id == priceListId).First().FinalPrice;
                                quantity = Math.Round(Convert.ToDecimal(quantityString) / price, 3);
                            }
                            else if (ParameterCache.Instance.GetAllGeneralParameters().First().ProductBarCodeWeight)
                            {
                                string quantityString = code.Substring(7, 5);
                                quantity = Math.Round(Convert.ToDecimal(quantityString) / 1000, 3);
                            }
                        }



                        // Verificar si el artículo ya está en la grilla
                        ArticleItem? existingItem = ArticleItems.FirstOrDefault(x => x.Code == article.Code && x.PriceListId == priceListId);

                        if (existingItem != null && ParameterCache.Instance.GetAllGeneralParameters().First().SumQuantityItems && !isProductWeight)
                        {
                            // Ya existe → solo aumentar la cantidad
                            existingItem.Quantity += 1;
                            existingItem.Recalculate();
                            OnPropertyChanged(nameof(TotalItems));
                            OnPropertyChanged(nameof(TotalPrice));
                        }
                        else
                        {
                            ArticleItem newItem = new()
                            {
                                Code = article.Code,
                                Description = article.Description,
                                SmallMeasureDescription = article.Measures.First(m => m.Id == article.MeasureId).SmallDescription,
                                Quantity = quantity,
                                Bonification = 0,
                            };

                            // llenar PriceLists con las del artículo
                            newItem.PriceLists.Clear();
                            foreach (var pl in article.PriceLists)
                                newItem.PriceLists.Add(pl);

                            // determinar qué lista de precios usar
                            if (cbPriceLists.SelectedValue != null)
                                priceListId = Convert.ToInt32(cbPriceLists.SelectedValue);
                            else if (article.PriceLists.Any())
                                priceListId = Convert.ToInt32(
                                    article.PriceLists.First().GetType().GetProperty("Id")
                                          .GetValue(article.PriceLists.First()));

                            // asignar lista de precios -> setter actualiza el Price
                            newItem.PriceListId = priceListId;

                            newItem.Recalculate();
                            OnPropertyChanged(nameof(TotalItems));
                            OnPropertyChanged(nameof(TotalPrice));
                            ArticleItems.Add(newItem);

                            // 🚫 Solo agregamos fila en blanco si NO está tildado el checkbox de código de barras
                            if (chBarcode.IsChecked == false)
                            {
                                ArticleItems.Add(new ArticleItem());
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("Artículo no encontrado.", "Aviso al operador", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    // limpiar el textbox y volver a enfocarlo
                    txtBarcode.Clear();
                    txtBarcode.Focus();
                }
            }
        }

        private void Quantity_GotFocus(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox textBox)
            {
                // selecciona todo el texto, para que al tipear se borre
                textBox.SelectAll();
                // limpiar todo el texbox:
                // textBox.Clear();
            }
        }

        // Permitir solo números y una coma, y normalizar '.' a ','
        private void Quantity_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (sender is not TextBox tb || string.IsNullOrEmpty(e.Text)) return;

            // Normalizar '.' a ','
            string input = e.Text == "." ? "," : e.Text;

            // Validar que el input sea dígito(s) o coma(s)
            foreach (char ch in input)
            {
                if (!char.IsDigit(ch) && ch != ',')
                {
                    e.Handled = true;
                    return;
                }
            }

            int selStart = tb.SelectionStart;
            int selLen = tb.SelectionLength;
            string current = tb.Text ?? string.Empty;

            // Construir el texto resultante si insertamos "input" en la selección actual
            string preview = current.Substring(0, selStart) + input + current.Substring(Math.Min(current.Length, selStart + selLen));

            // No permitir más de una coma
            if (preview.Count(c => c == ',') > 1)
            {
                e.Handled = true;
                return;
            }

            // Insertar respetando selección y posicionar caret después de la inserción
            e.Handled = true;
            // Usamos SelectedText + forzamos la posición del caret para evitar sobrescrituras
            tb.SelectedText = input;
            tb.SelectionStart = selStart + input.Length;
            tb.SelectionLength = 0;
        }

        // Normalizar si se pega texto con '.'
        // TextChanged: normalizar si se pega texto con '.' (y ajustar caret)
        private void Quantity_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (sender is not TextBox tb) return;

            // Si hay puntos, reemplazamos por comas y preservamos caret lo mejor posible
            if (tb.Text.Contains("."))
            {
                int caret = tb.SelectionStart;
                tb.Text = tb.Text.Replace(".", ",");
                tb.SelectionStart = Math.Min(caret, tb.Text.Length);
            }
        }

        // Handler de pegado: limpiar el texto pegado y normalizar
        private void Quantity_Pasting(object sender, DataObjectPastingEventArgs e)
        {
            if (sender is not TextBox tb) return;

            if (!e.DataObject.GetDataPresent(DataFormats.Text))
            {
                e.CancelCommand();
                return;
            }

            string pasted = e.DataObject.GetData(DataFormats.Text) as string ?? string.Empty;
            pasted = pasted.Replace(".", ","); // normalizar

            // Construir texto resultante si insertamos el pegado
            int selStart = tb.SelectionStart;
            int selLen = tb.SelectionLength;
            string current = tb.Text ?? string.Empty;
            string preview = current.Substring(0, selStart) + pasted + current.Substring(Math.Min(current.Length, selStart + selLen));

            // Filtrar caracteres: solo dígitos y una coma
            var sb = new System.Text.StringBuilder();
            int commaCount = preview.Count(c => c == ',');
            // but the preview count counts existing + pasted; we must ensure final <=1
            // we'll allow first comma only
            int existingComma = current.Count(c => c == ',') - current.Substring(selStart, Math.Min(selLen, Math.Max(0, current.Length - selStart))).Count(c => c == ','); // comas fuera de la selección
                                                                                                                                                                           // Build safe pasted string
            int allowedCommas = existingComma == 0 ? 1 : 0;

            foreach (char ch in pasted)
            {
                if (char.IsDigit(ch))
                    sb.Append(ch);
                else if (ch == ',' && allowedCommas > 0)
                {
                    sb.Append(ch);
                    allowedCommas--;
                }
                // ignorar otros caracteres
            }

            string final = sb.ToString();
            if (string.IsNullOrEmpty(final))
            {
                e.CancelCommand();
                return;
            }

            // Reemplazamos la selección por el texto limpio
            e.CancelCommand();
            tb.SelectedText = final;
            tb.SelectionStart = selStart + final.Length;
            tb.SelectionLength = 0;
        }


        private void chBarcode_Checked(object sender, RoutedEventArgs e)
        {
            txtBarcode.Focus();
        }

        private void chBarcode_Unchecked(object sender, RoutedEventArgs e)
        {
            if (dgArticles.Items.Count > 0)
            {
                dgArticles.SelectedIndex = 0;
                dgArticles.CurrentCell = new DataGridCellInfo(dgArticles.Items[0], dgArticles.Columns[0]);
                dgArticles.BeginEdit();
            }
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            if (this.Parent is ContentControl parent)
                parent.Content = null;
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            // Guardar venta (implementar según tu API)
        }

        private void BtnUpdate_Click(object sender, RoutedEventArgs e)
        {
            // Actualizar venta (implementar según tu API)
        }

        private void SetingFocus()
        {
            if (UsePostMethod)
            {
                chBarcode.IsChecked = true;
                txtBarcode.Focus();
            }
            else
            {
                if (ArticleItems == null)
                    ArticleItems = new ObservableCollection<ArticleItem>();

                if (ArticleItems.Count == 0)
                    ArticleItems.Add(new ArticleItem());
                dgArticles.Dispatcher.BeginInvoke(new System.Action(() =>
                {
                    if (dgArticles.Items.Count > 0 && dgArticles.Columns.Count > 0)
                    {
                        dgArticles.SelectedIndex = 0;
                        dgArticles.CurrentCell = new DataGridCellInfo(dgArticles.Items[0], dgArticles.Columns[0]);
                        dgArticles.ScrollIntoView(dgArticles.Items[0]);
                        dgArticles.BeginEdit();
                    }
                }), System.Windows.Threading.DispatcherPriority.Input);
            }
        }

        private void miUserControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            // Nuevo ancho y alto
            double newWidth = e.NewSize.Width;
            double newHeight = e.NewSize.Height;

            dgArticles.Height = newHeight * 0.5;
            //dgArticles.Width = newWidth * 0.95;
        }
    }

}
