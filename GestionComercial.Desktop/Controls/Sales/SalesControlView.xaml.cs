using GestionComercial.Desktop.Cache;
using GestionComercial.Desktop.Services;
using GestionComercial.Domain.DTOs.Client;
using GestionComercial.Domain.DTOs.Sale;
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
                        ClientViewModel? client = ClientCache.Instance.FindClientByOptionalCode(txtClientCode.Text);
                        if (client != null)
                        {
                            txtFansatyName.Text = string.IsNullOrEmpty(client.FantasyName) ? client.BusinessName : client.FantasyName;
                            txtAddress.Text = $"{client.Address}\n{client.City}, {client.State}\nC.P.{client.PostalCode}";
                        }
                        else
                            MessageBox.Show("El código informado no existe", "Aviso al operador", MessageBoxButton.OK, MessageBoxImage.Error);
                        break;
                    }
                default:
                    break;
            }
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



        private void msgError(string msg)
        {
            lblError.Text = msg;
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnUpdate_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
