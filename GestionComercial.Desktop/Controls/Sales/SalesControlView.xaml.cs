using GestionComercial.Desktop.Services;
using GestionComercial.Domain.Cache;
using GestionComercial.Domain.DTOs.Client;
using GestionComercial.Domain.DTOs.Sale;
using GestionComercial.Domain.Entities.Masters;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace GestionComercial.Desktop.Controls.Sales
{
    public partial class SalesControlView : UserControl
    {
        private readonly SalesApiService _salesApiService;
        private readonly int SaleId;
        private SaleViewModel saleViewModel;

        public ObservableCollection<ArticleItem> ArticleItems { get; set; }

        public SalesControlView(int saleId)
        {
            InitializeComponent();
            _salesApiService = new SalesApiService();
            ArticleItems = new ObservableCollection<ArticleItem>();
            DataContext = this;
            SaleId = saleId;

            // Agregar fila inicial en blanco
            ArticleItems.Add(new ArticleItem());

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
                }
                else
                {
                    MessageBox.Show("El código informado no existe", "Aviso al operador", MessageBoxButton.OK, MessageBoxImage.Error);
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
            lblError.MaxWidth = this.ActualWidth;
            dpDate.SelectedDate = System.DateTime.Now;

            // Inicializar la primera fila editable
            if (ArticleItems.Count == 0)
                ArticleItems.Add(new ArticleItem());

            dgArticles.SelectedIndex = 0;
            dgArticles.CurrentCell = new DataGridCellInfo(dgArticles.Items[0], dgArticles.Columns[0]);
            dgArticles.BeginEdit();
        }

        private void dgArticles_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
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

            // Recalcular subtotal y total si cambió cantidad o bonificación
            if (e.Column.Header.ToString() == "Cantidad" || e.Column.Header.ToString() == "Bonificación (%)")
                currentItem.Recalculate();
        }

        private void dgArticles_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
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
            }
        }

        private void txtBarcode_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                var code = txtBarcode.Text.Trim();
                if (!string.IsNullOrEmpty(code))
                {
                    var article = ArticleCache.Instance.FindByCodeOrBarCode(code);
                    if (article != null)
                    {
                        var priceLists = article.PriceLists;
                        int priceListId = Convert.ToInt32(cbPriceLists.SelectedValue);

                        // Verificar si el artículo ya está en la grilla
                        var existingItem = ArticleItems.FirstOrDefault(x => x.Code == article.Code && x.PriceListId == priceListId);

                        if (existingItem != null)
                        {
                            // Ya existe → solo aumentar la cantidad
                            existingItem.Quantity += 1;
                            existingItem.Recalculate();
                        }
                        else
                        {
                            var newItem = new ArticleItem
                            {
                                Code = article.Code,
                                Description = article.Description
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

                            newItem.Quantity = 1;
                            newItem.Bonification = 0;
                            newItem.Recalculate();
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

        private void dgArticles_Loaded(object sender, RoutedEventArgs e)
        {
            // Asegurarnos de tener al menos una fila
            if (ArticleItems == null)
                ArticleItems = new ObservableCollection<ArticleItem>();

            if (ArticleItems.Count == 0)
                ArticleItems.Add(new ArticleItem());

            // Si no estamos en modo código de barras, poner foco en la primera celda de Código
            if (!chBarcode.IsChecked.GetValueOrDefault())
            {
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
    }

}
