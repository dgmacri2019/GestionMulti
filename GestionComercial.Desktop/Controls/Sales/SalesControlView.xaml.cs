using GestionComercial.Desktop.Services;
using GestionComercial.Domain.Cache;
using GestionComercial.Domain.DTOs.Client;
using GestionComercial.Domain.DTOs.Sale;
using GestionComercial.Domain.DTOs.Stock;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
        public int TotalItems => ArticleItems.Sum(a => (int)a.Quantity);
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
                                string quantityString = currentItem.Code.Substring(7, 5);
                                decimal price = priceLists.Where(pl => pl.Id == priceListId).First().FinalPrice;
                                quantity = Math.Round(Convert.ToDecimal(quantityString) / price, 3);
                            }
                            else if (ParameterCache.Instance.GetAllGeneralParameters().First().ProductBarCodeWeight)
                            {
                                string quantityString = currentItem.Code.Substring(7, 5);
                                quantity = Math.Round(Convert.ToDecimal(quantityString) / 1000, 3);
                            }
                        }


                        currentItem.SmallMeasureDescription = article.Measures
                            .First(m => m.Id == article.MeasureId).SmallDescription;
                        currentItem.Quantity = quantity;
                        // llenar la colección PriceLists de la fila (por fila)
                        currentItem.PriceLists.Clear();
                        foreach (var pl in article.PriceLists) // article.PriceLists es la colección que ya tenés
                            currentItem.PriceLists.Add(pl);
                        // asignar descripción
                        currentItem.Description = article.Description;
                        currentItem.Code = article.Code;
                        currentItem.PriceListId = priceListId; // esto actualiza Price via el setter
                        currentItem.Bonification = 0;
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

                        if (ArticleItems.Last() == currentItem)
                            ArticleItems.Add(new ArticleItem());
                    }
                }

                // Recalcular subtotal y total si cambió cantidad o bonificación
                if (e.Column.Header.ToString() == "Cantidad" || e.Column.Header.ToString() == "Bonificación (%)")
                {
                    currentItem.Recalculate();
                    OnPropertyChanged(nameof(TotalItems));
                    OnPropertyChanged(nameof(TotalPrice));
                    // ✅ Forzar foco en la celda "Código" de la fila siguiente
                    Dispatcher.BeginInvoke(new Action(() =>
                    {
                        var dataGrid = (DataGrid)sender;
                        var nextRowIndex = ArticleItems.IndexOf(currentItem) + 1;
                        if (nextRowIndex < ArticleItems.Count)
                        {
                            var nextRowItem = ArticleItems[nextRowIndex];
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
            if (e.Key == Key.Enter)
            {
                /*
                var currentItem = dgArticles.CurrentItem as ArticleItem;
                if (currentItem != null && !string.IsNullOrWhiteSpace(currentItem.Code))
                {
                    var article = ArticleCache.Instance.FindByCodeOrBarCode(currentItem.Code);
                    if (article != null)
                    {
                        // asignar descripción
                        currentItem.Description = article.Description;

                        // llenar la colección PriceLists de la fila (por fila)
                        currentItem.PriceLists.Clear();
                        foreach (var pl in article.PriceLists) // article.PriceLists es la colección que ya tenés
                            currentItem.PriceLists.Add(pl);

                        // elegir priceListId por defecto: el cbPriceLists del cliente o la primera de article
                        int priceListId = 0;
                        if (cbPriceLists.SelectedValue != null)
                            priceListId = Convert.ToInt32(cbPriceLists.SelectedValue);
                        else if (article.PriceLists.Any())
                            priceListId = Convert.ToInt32(article.PriceLists.First().GetType().GetProperty("Id").GetValue(article.PriceLists.First()));

                        currentItem.PriceListId = priceListId; // esto actualiza Price via el setter
                        currentItem.Quantity = 1;
                        currentItem.Bonification = 0;
                        currentItem.Recalculate();

                        

                        if (ArticleItems.Last() == currentItem)
                            ArticleItems.Add(new ArticleItem());
                    }
                }
                */
            }
        }

        private void txtBarcode_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                var code = txtBarcode.Text.Trim();
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
                                //TODO: ver tema etiqueta con precios
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
                        var existingItem = ArticleItems.FirstOrDefault(x => x.Code == article.Code && x.PriceListId == priceListId);

                        if (existingItem != null && !isProductWeight)
                        {
                            // Ya existe → solo aumentar la cantidad
                            existingItem.Quantity += 1;
                            existingItem.Recalculate();
                            OnPropertyChanged(nameof(TotalItems));
                            OnPropertyChanged(nameof(TotalPrice));
                        }
                        else
                        {
                            var newItem = new ArticleItem
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
            var textBox = sender as TextBox;
            //textBox?.SelectAll(); // selecciona todo el texto, para que al tipear se borre
            // o si preferís limpiar directamente:
            textBox.Clear();
        }
        private void Quantity_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            var textBox = sender as TextBox;

            // Si escribe punto, reemplazo por coma manualmente
            if (e.Text == ".")
            {
                int caret = textBox.CaretIndex;
                textBox.Text = textBox.Text.Insert(caret, ",");
                textBox.CaretIndex = caret + 1;
                e.Handled = true; // anulamos el input original
                return;
            }

            // Bloquear más de un punto o coma decimal
            if ((e.Text == "." || e.Text == ",") && textBox.Text.Contains(","))
            {
                e.Handled = true;
                return;
            }

            // Permitir solo números o punto
            if (!char.IsDigit(e.Text, 0) && (e.Text != "." || e.Text != ","))
            {
                e.Handled = true;
            }
        }
        private void Quantity_TextChanged(object sender, TextChangedEventArgs e)
        {
            var textBox = sender as TextBox;

            // Si alguien pega texto con punto, lo normalizamos
            if (textBox.Text.Contains("."))
            {
                int caret = textBox.CaretIndex;
                textBox.Text = textBox.Text.Replace(".", ",");
                textBox.CaretIndex = caret;
            }
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
