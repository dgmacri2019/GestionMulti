using GestionComercial.Desktop.Helpers;
using GestionComercial.Desktop.Services;
using GestionComercial.Desktop.Views.Searchs;
using GestionComercial.Domain.Cache;
using GestionComercial.Domain.DTOs.Client;
using GestionComercial.Domain.DTOs.Sale;
using GestionComercial.Domain.DTOs.Stock;
using GestionComercial.Domain.Entities.Masters;
using GestionComercial.Domain.Entities.Sales;
using GestionComercial.Domain.Entities.Stock;
using GestionComercial.Domain.Helpers;
using GestionComercial.Domain.Response;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

namespace GestionComercial.Desktop.Views.Sales
{
    public partial class SaleAddWindow : Window, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        private readonly SalesApiService _salesApiService;
        private readonly int SaleId;
        private SaleViewModel SaleViewModel;
        private bool UsePostMethod;
        public ObservableCollection<ArticleItem> ArticleItems { get; set; }
        public int TotalItems => ArticleItems.Count(a => !string.IsNullOrEmpty(a.Code));
        public decimal SubTotalPrice => ArticleItems.Sum(a => a.Total);
        //public decimal GeneralDiscount { get; set; }
        public decimal TotalPrice => SaleViewModel != null? SubTotalPrice - (SubTotalPrice * SaleViewModel.GeneralDiscount / 100) : 0;

        private int SalePoint;
        private int SaleNumber = 0;

        // Separador decimal según cultura actual (si querés forzar coma: const char DecSep = ',';)
        private static readonly char DecSep = CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator[0];
        private static readonly char OtherSep = (DecSep == ',') ? '.' : ',';

        private int Width;
        private int Height;

        public SaleAddWindow(int saleId)
        {
            InitializeComponent();

            _salesApiService = new SalesApiService();
            DataContext = this;
            SaleId = saleId;
            ArticleItems = [];
            ArticleItems.CollectionChanged += (s, e) =>
            {
                OnPropertyChanged(nameof(TotalItems));
                OnPropertyChanged(nameof(TotalPrice));
                OnPropertyChanged(nameof(SubTotalPrice));
                //OnPropertyChanged(nameof(Bonification));
            };

            // Llamamos a un método async sin bloquear
            Loaded += async (s, e) => await InitializeAsync();
        }

        private async Task InitializeAsync()
        {
            // Esperamos hasta que los parámetros estén listos o 5 segundos máximo
            bool ready = await WaitForDataAsync(TimeSpan.FromSeconds(5));

            if (!ready)
            {
                MessageBox.Show("Error cargando datos iniciales", "Aviso al operador",
                    MessageBoxButton.OK, MessageBoxImage.Error);

                this.Close(); // 👈 Cerramos la ventana
                return;
            }

            UsePostMethod = ParameterCache.Instance.GetAllGeneralParameters().First().UsePostMethod;
            SalePoint = ParameterCache.Instance.GetPcParameter().SalePoint;
            SaleNumber = SaleCache.Instance.GetLastSaleNumber() + 1;
            await LoadSaleAsync();



            var (width, height) = ScreenHelper.ObtenerResolucion(this);
            Width = width;
            Height = height;
        }

        private async Task<bool> WaitForDataAsync(TimeSpan timeout)
        {
            var start = DateTime.Now;

            while (!ParameterCache.Instance.HasDataPCParameters ||
                   !ParameterCache.Instance.HasDataGeneralParameters ||
                   !ClientCache.Instance.HasData ||
                   !ArticleCache.Instance.HasData)
            {
                if (DateTime.Now - start > timeout)
                    return false;

                await Task.Delay(50); // 👈 Libera la UI mientras espera
            }

            return true;
        }


        private async Task LoadSaleAsync()
        {

            if (SaleId == 0)
            {
                SaleViewModel = new SaleViewModel
                {
                    SalePoint = SalePoint,
                    SaleNumber = SaleNumber,
                    //saleViewModel.Clients = ClientCache.Instance.GetAllClients().ToList();
                    PriceLists = new ObservableCollection<PriceList>(MasterCache.Instance.GetPriceLists()),
                    SaleConditions = new ObservableCollection<SaleCondition>(MasterCache.Instance.GetSaleConditions())
                };
            }
            else
            {
                var result = await _salesApiService.GetByIdAsync(SaleId, Environment.MachineName);

                if (result.Success)
                    SaleViewModel = result.SaleViewModel;
                else
                    lblError.Text = result.Message;
            }

            DataContext = SaleViewModel;
        }


        private void dgArticles_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            try

            {
                var currentItem = e.Row.Item as ArticleItem;
                if (currentItem == null) return;
                if (string.IsNullOrWhiteSpace(currentItem.Code) && !string.IsNullOrEmpty(currentItem.Description))
                {
                    MessageBox.Show("Código de artículo vacio", "Aviso al operador", MessageBoxButton.OK, MessageBoxImage.Error);
                    // ✅ Poner foco en la celda Código
                    Dispatcher.BeginInvoke(new Action(() =>
                    {
                        var dataGrid = (DataGrid)sender;
                        dataGrid.CurrentCell = new DataGridCellInfo(
                            currentItem,
                            dataGrid.Columns.First(c => c.Header?.ToString() == "Código")
                        );
                        dataGrid.BeginEdit();
                    }), DispatcherPriority.Background);
                    return;
                }
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
                            Dispatcher.BeginInvoke(new Action(() =>
                            {
                                var dataGrid = (DataGrid)sender;
                                var nextRowIndex = ArticleItems.IndexOf(currentItem);
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
                            return;
                        }
                        if (!article.IsEnabled)
                        {
                            MessageBox.Show("Artículo no habilitado para la venta", "Aviso al operador", MessageBoxButton.OK, MessageBoxImage.Error);
                            Dispatcher.BeginInvoke(new Action(() =>
                            {
                                var dataGrid = (DataGrid)sender;
                                var nextRowIndex = ArticleItems.IndexOf(currentItem);
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
                        currentItem.SmallMeasureDescription = MasterCache.Instance.GetMeasures()
                            .First(m => m.Id == article.MeasureId).SmallDescription;
                        currentItem.Quantity = quantity;

                        // llenar PriceLists de la fila
                        currentItem.PriceLists.Clear();
                        foreach (var pl in article.PriceLists)
                            currentItem.PriceLists.Add(pl);

                        currentItem.Description = article.Description;
                        currentItem.Code = article.Code;
                        currentItem.IsLowStock = article.StockCheck && article.Stock <= article.MinimalStock;
                        // ✅ Forzar que quede la misma lista de precios seleccionada que en el combo principal
                        if (currentItem.PriceLists.Any(pl => pl.Id == defaultPriceListId))
                            currentItem.PriceListId = defaultPriceListId;
                        else
                            currentItem.PriceListId = currentItem.PriceLists.FirstOrDefault()?.Id ?? 0;

                        currentItem.Bonification = 0;
                        currentItem.IsLowStock = article.StockCheck && article.Stock <= article.MinimalStock;
                        currentItem.Recalculate();

                        OnPropertyChanged(nameof(TotalItems));
                        OnPropertyChanged(nameof(TotalPrice));
                        OnPropertyChanged(nameof(SubTotalPrice));
                        //OnPropertyChanged(nameof(Bonification));

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
                    else
                    {
                        MessageBox.Show("Artículo no encontrado.", "Aviso al operador", MessageBoxButton.OK, MessageBoxImage.Error);
                        Dispatcher.BeginInvoke(new Action(() =>
                        {
                            var dataGrid = (DataGrid)sender;
                            var nextRowIndex = ArticleItems.IndexOf(currentItem);
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
                        return;
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

                    if (article != null && !article.PriceLists.Any(pl => pl.Id == priceListId))
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
                    OnPropertyChanged(nameof(SubTotalPrice));
                    //OnPropertyChanged(nameof(Bonification));

                    if (chBarcode.IsChecked == false)
                    {
                        if (ArticleItems.Last() == currentItem)
                            ArticleItems.Add(new ArticleItem());
                        // ✅ Forzar foco en la celda "Código" de la fila siguiente
                        // Evitar registros duplicados

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
                    else
                    {
                        // limpiar el textbox y volver a enfocarlo
                        txtBarcode.Clear();
                        txtBarcode.Focus();
                    }
                    dgArticles.ScrollIntoView(ArticleItems.Last());
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
                            currentItem.IsLowStock = selectedArticle.StockCheck && selectedArticle.Stock <= selectedArticle.MinimalStock;

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
                            OnPropertyChanged(nameof(SubTotalPrice));
                            //OnPropertyChanged(nameof(Bonification));

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

                    //cbSaleConditions.ItemsSource = client.SaleConditions;
                    //cbSaleConditions.SelectedValue = client.SaleConditionId;
                    ArticleItems.Clear();
                    dgArticles.Visibility = Visibility.Visible;
                    dgPostMethod.Visibility = Visibility.Visible;
                    SetingFocus();
                }
                else
                {
                    MessageBox.Show("El código informado no existe", "Aviso al operador", MessageBoxButton.OK, MessageBoxImage.Error);
                    dgArticles.Visibility = Visibility.Hidden;
                    dgPostMethod.Visibility = Visibility.Hidden;
                }


            }
            if (e.Key == Key.F5)
            {
                var searchWindow = new ClientSearchWindows(txtClientCode.Text) { Owner = Window.GetWindow(this) };
                if (searchWindow.ShowDialog() == true)
                {
                    ClientViewModel? selectedClient = searchWindow.SelectedClient;
                    if (selectedClient != null)
                    {
                        txtClientCode.Text = selectedClient.OptionalCode;
                        txtFansatyName.Text = string.IsNullOrEmpty(selectedClient.FantasyName) ? selectedClient.BusinessName : selectedClient.FantasyName;
                        txtAddress.Text = $"{selectedClient.Address}\n{selectedClient.City}, {selectedClient.State}\nC.P.{selectedClient.PostalCode}";
                        txtEmail.Text = !string.IsNullOrEmpty(selectedClient.Email) ? selectedClient.Email : string.Empty;
                        chSendEmail.IsChecked = !string.IsNullOrEmpty(selectedClient.Email);

                        cbPriceLists.ItemsSource = selectedClient.PriceLists;
                        cbPriceLists.SelectedValue = selectedClient.PriceListId;


                        //cbSaleConditions.ItemsSource = selectedClient.SaleConditions;
                        //cbSaleConditions.SelectedValue = selectedClient.SaleConditionId;
                        dgArticles.Visibility = Visibility.Visible;
                        dgPostMethod.Visibility = Visibility.Visible;
                        SetingFocus();

                    }
                }

                e.Handled = true;
            }
        }


        private void txtBarcode_KeyDown(object sender, KeyEventArgs e)
        {
            try
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
                                existingItem.IsLowStock = article.StockCheck && article.Stock <= article.MinimalStock;
                                existingItem.Quantity += 1;
                                existingItem.Recalculate();
                                OnPropertyChanged(nameof(TotalItems));
                                OnPropertyChanged(nameof(TotalPrice)); 
                                OnPropertyChanged(nameof(SubTotalPrice));
                                //OnPropertyChanged(nameof(Bonification));
                            }
                            else
                            {
                                ArticleItem newItem = new()
                                {
                                    Code = article.Code,
                                    Description = article.Description,
                                    SmallMeasureDescription = MasterCache.Instance.GetMeasures().First(m => m.Id == article.MeasureId).SmallDescription,
                                    Quantity = quantity,
                                    Bonification = 0,
                                     IsLowStock = article.StockCheck && article.Stock <= article.MinimalStock,
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
                                OnPropertyChanged(nameof(SubTotalPrice));
                                //OnPropertyChanged(nameof(Bonification));
                                ArticleItems.Add(newItem);
                                dgArticles.ScrollIntoView(ArticleItems.Last());
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
                if (e.Key == Key.F5)
                {
                    var searchWindow = new ArticleSearchWindow(txtBarcode.Text) { Owner = Window.GetWindow(this) };
                    if (searchWindow.ShowDialog() == true)
                    {
                        var article = searchWindow.SelectedArticle;
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

                            // Verificar si el artículo ya está en la grilla

                            //ArticleItem? existingItem = ArticleItems.FirstOrDefault(x => x.Code == article.Code && x.PriceListId == priceListId);


                            ArticleItem newItem = new()
                            {
                                Code = article.Code,
                                Description = article.Description,
                                SmallMeasureDescription = MasterCache.Instance.GetMeasures().First(m => m.Id == article.MeasureId).SmallDescription,
                                Quantity = 1,
                                Bonification = 0,
                                IsLowStock = article.StockCheck && article.Stock <= article.MinimalStock,

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
                            OnPropertyChanged(nameof(SubTotalPrice));
                            //OnPropertyChanged(nameof(Bonification));
                            ArticleItems.Add(newItem);
                            dgArticles.ScrollIntoView(ArticleItems.Last());
                            // 🚫 Solo agregamos fila en blanco si NO está tildado el checkbox de código de barras
                            if (chBarcode.IsChecked == false)
                            {
                                ArticleItems.Add(new ArticleItem());
                            }

                        }

                    }

                    e.Handled = true;
                    // limpiar el textbox y volver a enfocarlo
                    txtBarcode.Clear();
                    txtBarcode.Focus();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Aviso al operador", MessageBoxButton.OK, MessageBoxImage.Error);

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
            LogOut();
        }


        private async void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            lblError.Text = string.Empty;
            try
            {
                if (ValidateSale())
                {
                    if (ClientCache.Instance.FindClientByOptionalCode(txtClientCode.Text) != null)
                    {
                        Sale sale = ToSale(SaleViewModel, ArticleItems);

                        var payMethodWindows = new PayMethodWindow(Math.Round(sale.Total, 2));
                        if (payMethodWindows.ShowDialog() == true)
                        {
                            var selectedMethods = payMethodWindows.MetodosPago;
                            ICollection<SalePayMetodDetail> salePayMetodDetails = [];
                            foreach (var pm in selectedMethods)
                                salePayMetodDetails.Add(new SalePayMetodDetail
                                {
                                    CreateDate = DateTime.Now,
                                    CreateUser = App.UserName,
                                    IsDeleted = false,
                                    IsEnabled = true,
                                    Value = pm.Monto,
                                    SaleConditionId = pm.MetodoId,
                                });
                            decimal totalpay = salePayMetodDetails.Sum(sp => sp.Value);
                            sale.SalePayMetodDetails = salePayMetodDetails;
                            sale.Sold = sale.Total - totalpay;
                            sale.PaidOut = sale.Total == totalpay;
                            sale.PartialPay = totalpay > 0 && totalpay < sale.Total;


                            GeneralResponse resultAdd = await _salesApiService.AddAsync(sale);
                            if (resultAdd.Success)
                            {
                                ClearClient();
                                ArticleItems.Clear();
                                txtClientCode.Focus();
                                txtClientCode.Text = string.Empty;
                                SaleNumber++;
                                await LoadSaleAsync();
                            }
                            else
                                lblError.Text = resultAdd.Message;
                        }
                    }
                    else
                    {
                        lblError.Text = "Cliente inválido";
                    }
                }



            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
            }
        }


        private void BtnUpdate_Click(object sender, RoutedEventArgs e)
        {
            // Actualizar venta (implementar según tu API)
        }


        private void BtnBuscar_Click(object sender, RoutedEventArgs e)
        {
            // lógica buscar
            MessageBox.Show("Buscar...");
        }


        private void BtnImprimir_Click(object sender, RoutedEventArgs e)
        {
            // lógica imprimir
            MessageBox.Show("Imprimiendo...");
        }


        private void Quantity_GotFocus(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox textBox)
            {
                // selecciona todo el texto, para que al tipear se borre
                textBox.SelectAll();
            }
        }
        private void Quantity_PreviewKeyDown(object sender, KeyEventArgs e)
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
        private void Quantity_PreviewTextInput(object sender, TextCompositionEventArgs e)
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
        private void Quantity_TextChanged(object sender, TextChangedEventArgs e)
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
        private void Quantity_Pasting(object sender, DataObjectPastingEventArgs e)
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




        private void SetingFocus()
        {
            try
            {
                if (UsePostMethod)
                {
                    if (ArticleItems != null && ArticleItems.Count > 0 && string.IsNullOrEmpty(ArticleItems[0].Code))
                        ArticleItems.Clear();
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
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
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
            //dgArticles.Height = Height * 0.3;
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                LogOut();
            }
            else if (e.Key == Key.F2)
            {
                // F2 → guardar
                BtnAdd_Click(null, null);
            }
        }

        private void ClearClient()
        {
            txtFansatyName.Text = string.Empty;
            txtAddress.Text = string.Empty;
            txtEmail.Text = string.Empty;
            chSendEmail.IsChecked = false;
            cbPriceLists.SelectedValue = 0;
            //cbSaleConditions.SelectedValue = 0;
        }


        private bool ValidateSale()
        {
            bool result = true;
            if (Convert.ToInt32(cbPriceLists.SelectedValue) == 0)
            {
                result = false;
                lblError.Text = "Seleccione la lista de precios";
            }
            //if (Convert.ToInt32(cbSaleConditions.SelectedValue) == 0)
            //{
            //    result = false;
            //    lblError.Text = "Seleccione la condición de venta";
            //}
            if (chSendEmail.IsChecked == true)
                if (string.IsNullOrEmpty(txtEmail.Text))
                {
                    result = false;
                    lblError.Text = "Ingrese el correo electrónico";
                }
                else if (!string.IsNullOrEmpty(txtEmail.Text) && !ValidatorHelper.ValidateEmail(txtEmail.Text))
                {
                    result = false;
                    lblError.Text = "Ingrese un correo electrónico válido";
                }
            if (string.IsNullOrEmpty(txtClientCode.Text))
            {
                result = false;
                lblError.Text = "Ingrese el código de cliente";
            }
            if (ArticleItems.Count(a => !string.IsNullOrEmpty(a.Code)) <= 0)
            {
                result = false;
                lblError.Text = "Debe ingresar al menos 1 artículo";
            }
            return result;
        }


        private void LogOut()
        {
            DialogResult = false;
        }


        private Sale ToSale(SaleViewModel saleViewModel, ObservableCollection<ArticleItem> articleItems)
        {
            List<SaleDetail> saleDetails = [];
            foreach (ArticleItem item in articleItems.Where(ai => !string.IsNullOrEmpty(ai.Code)))
            {
                ArticleViewModel article = ArticleCache.Instance.FindByCodeOrBarCode(item.Code);
                if (article != null)
                {
                    saleDetails.Add(new SaleDetail
                    {
                        ArticleId = article.Id,
                        Code = article.Code,
                        CreateDate = DateTime.Now,
                        CreateUser = App.UserName,
                        Description = item.Description,
                        Discount = item.Bonification,
                        IsDeleted = false,
                        IsEnabled = true,
                        Price = item.Price,
                        Quantity = item.Quantity,
                        SubTotal = item.Subtotal,
                        TotalItem = item.Total,
                        TaxId = article.TaxId,
                        List = item.PriceListId,
                    });
                }
                else
                    return null;

            }

            DateTime saleDate = (DateTime)dpDate.SelectedDate;

            decimal baseimp105 = saleDetails.Where(sd => sd.TaxId == 2).Sum(sd => sd.TotalItem);
            decimal baseimp21 = saleDetails.Where(sd => sd.TaxId == 3).Sum(sd => sd.TotalItem);
            decimal baseimp27 = saleDetails.Where(sd => sd.TaxId == 4).Sum(sd => sd.TotalItem);
            decimal iva105 = saleDetails.Any(sd => sd.TaxId == 2) ? saleDetails.Where(sd => sd.TaxId == 2).Sum(sd => sd.TotalItem) * 10.5m / 100 : 0;
            decimal iva21 = saleDetails.Any(sd => sd.TaxId == 3) ? saleDetails.Where(sd => sd.TaxId == 3).Sum(sd => sd.TotalItem) * 21m / 100 : 0;
            decimal iva27 = saleDetails.Any(sd => sd.TaxId == 4) ? saleDetails.Where(sd => sd.TaxId == 4).Sum(sd => sd.TotalItem) * 27m / 100 : 0;

            decimal subTotal = TotalPrice - iva105 - iva21 - iva27;

            return new Sale
            {
                ClientId = ClientCache.Instance.FindClientByOptionalCode(txtClientCode.Text).Id,
                CreateDate = DateTime.Now,
                CreateUser = App.UserName,
                IsDeleted = false,
                IsEnabled = true,
                IsFinished = true,
                //SaleConditionId = saleViewModel.SaleConditionId,
                GeneralDiscount = saleViewModel.GeneralDiscount,
                SaleDate = saleDate.Date,
                SalePoint = saleViewModel.SalePoint,
                SaleNumber = saleViewModel.SaleNumber,
                SubTotal = subTotal,
                Total = subTotal - (subTotal * saleViewModel.GeneralDiscount / 100),
                //SaleCondition = saleViewModel.SaleCondition,
                BaseImp105 = baseimp105 - (baseimp105 * saleViewModel.GeneralDiscount / 100),
                BaseImp21 = baseimp21 - (baseimp21 * saleViewModel.GeneralDiscount / 100),
                BaseImp27 = baseimp27 - (baseimp27 * saleViewModel.GeneralDiscount / 100),
                TotalIVA105 = iva105 - (iva105 * saleViewModel.GeneralDiscount / 100),
                TotalIVA21 = iva21 - (iva21 * saleViewModel.GeneralDiscount / 100),
                TotalIVA27 = iva27 - (iva27 * saleViewModel.GeneralDiscount / 100),
                SaleDetails = saleDetails,

            };
        }

    }

}
