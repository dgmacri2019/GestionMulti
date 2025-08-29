using GestionComercial.Desktop.Cache;
using GestionComercial.Desktop.Services;
using GestionComercial.Domain.DTOs.Client;
using GestionComercial.Domain.DTOs.Sale;
using GestionComercial.Domain.DTOs.Stock;
using GestionComercial.Domain.Entities.Masters;
using GestionComercial.Domain.Response;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace GestionComercial.Desktop.Controls.Sales
{
    /// <summary>
    /// Lógica de interacción para SalesControlView.xaml
    /// </summary>
    public partial class SalesControlView : UserControl
    {

        private readonly SalesApiService _salesApiService;
        private readonly int SaleId;

        private SaleViewModel saleViewModel { get; set; }

        public event Action VentaActualizado;

        public SalesControlView(int saleId)
        {
            InitializeComponent();
            _salesApiService = new SalesApiService();
            SaleId = saleId;
            _ = FindSaleAsync();
            if (SaleId == 0)
            {
                btnAdd.Visibility = Visibility.Visible;
                btnUpdate.Visibility = Visibility.Hidden;
            }
            else
            {
                btnAdd.Visibility = Visibility.Hidden;
                btnUpdate.Visibility = Visibility.Visible;
            }
        }

        private void txtClientCode_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.F5:
                    {
                        break;
                    }
                case Key.Enter:
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
                            MessageBox.Show("El código informado no existe", "Aviso al operador", MessageBoxButton.OK, MessageBoxImage.Error);
                        break;
                    }
                default:
                    break;
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
            dpDate.SelectedDate = DateTime.Now;
        }

        private async Task FindSaleAsync()
        {
            SaleResponse result = await _salesApiService.GetByIdAsync(SaleId);

            //saleViewModel = SaleCache.Instance.GetAllSales().FirstOrDefault(p => p.Id == SaleId);

            if (result.Success)
            {
                saleViewModel = result.SaleViewModel;
                if (SaleId == 0)
                {
                    saleViewModel.CreateUser = App.UserName;
                }
                DataContext = saleViewModel;
            }
            else
                lblError.Text = result.Message;
        }


        private void dgArticles_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                var vm = this.DataContext as SaleViewModel;
                if (vm != null)
                {
                    vm.Articles.Add(new ArticleItem());
                    dgArticles.SelectedIndex = vm.Articles.Count - 1;
                    dgArticles.CurrentCell = new DataGridCellInfo(dgArticles.Items[vm.Articles.Count - 1], dgArticles.Columns[0]);
                    dgArticles.BeginEdit();
                    e.Handled = true;
                }
            }
        }

        private void dgArticles_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            if (e.EditAction == DataGridEditAction.Commit)
            {
                if (e.Row.Item is ArticleItem item)
                    item.UpdateTotals();
            }
        }

        private void txtBarcode_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                var code = txtBarcode.Text;
                var vm = this.DataContext as SaleViewModel;
                if (vm != null)
                {
                    // Buscar artículo en tu cache
                    ArticleViewModel? article = ArticleCache.Instance.FindByCodeOrBarCode(code); // Reemplazar con tu implementación
                    if (article != null)
                    {
                        var taxPrice = article.PriceWithTax;
                        var x = article.RealCost;
                        var priceLists = article.PriceLists;
                        
                        var newItem = new ArticleItem
                        {
                            Code = article.Code,
                            Description = article.Description,
                            Price = 10,
                            PriceListId = Convert.ToInt32(cbPriceLists.SelectedValue),
                            
                        };
                        vm.Articles.Add(newItem);
                    }

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


        private void msgError(string msg)
        {
            lblError.Text = msg;
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            // Buscar el ContentControl padre y vaciarlo
            var parent = this.Parent as ContentControl;
            if (parent != null)
            {
                parent.Content = null;
            }
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnUpdate_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
