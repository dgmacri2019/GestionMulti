using GestionComercial.Desktop.Helpers;
using GestionComercial.Desktop.Services;
using GestionComercial.Domain.Cache;
using GestionComercial.Domain.DTOs.PriceLists;
using GestionComercial.Domain.Entities.Stock;
using GestionComercial.Domain.Helpers;
using GestionComercial.Domain.Response;
using System.Windows;
using System.Windows.Controls;

namespace GestionComercial.Desktop.Controls.Maters.Configurations.Maestros.Stock
{
    /// <summary>
    /// Lógica de interacción para EditPriceListControlView.xaml
    /// </summary>
    public partial class EditPriceListControlView : UserControl
    {
        private readonly MasterClassApiService apiService;
        private readonly int PriceListId;
        private PriceListViewModel PriceListViewModel { get; set; }

        public event Action ListaPrecioActualizada;

        public EditPriceListControlView(int priceListId)
        {
            InitializeComponent();
            apiService = new MasterClassApiService();
            PriceListId = priceListId;
            if (PriceListId == 0)
            {
                btnAdd.Visibility = Visibility.Visible;
                btnUpdate.Visibility = Visibility.Hidden;
                PriceListViewModel = new PriceListViewModel { CreateUser = App.UserName, IsEnabled = true };
            }
            else
            {
                PriceListViewModel? priceList = PriceListCache.Instance.FindById(PriceListId);
                if (priceList != null)
                {
                    PriceListViewModel = priceList;
                    btnAdd.Visibility = Visibility.Hidden;
                    btnUpdate.Visibility = Visibility.Visible;
                }
                else
                    MsgBoxAlertHelper.MsgAlertError("No se pudo cargar la lista de precio");
            }
            DataContext = PriceListViewModel;
        }

        private void miUserControl_Loaded(object sender, RoutedEventArgs e)
        {
            lblError.MaxWidth = this.ActualWidth;
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            ListaPrecioActualizada?.Invoke(); // para notificar a la vista principal
        }

        private async void BtnUpdate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                lblError.Text = string.Empty;

                if (ValidatePriceList())
                {
                    btnUpdate.IsEnabled = false;
                    lblError.Text = string.Empty;
                    PriceListViewModel.UpdateUser = App.UserName;
                    PriceListViewModel.UpdateDate = DateTime.Now;

                    PriceList priceList = ConverterHelper.ToPriceList(PriceListViewModel, PriceListViewModel.Id == 0);
                    GeneralResponse resultUpdate = await apiService.UpdatePriceListAsync(priceList);
                    if (resultUpdate.Success)
                        ListaPrecioActualizada?.Invoke(); // para notificar a la vista principal
                    else
                        msgError(resultUpdate.Message);
                    btnUpdate.IsEnabled = true;
                }
            }
            catch (Exception ex)
            {
                msgError(ex.Message);
                btnUpdate.IsEnabled = true;
            }
        }

        private async void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                btnAdd.IsEnabled = false;
                lblError.Text = string.Empty;

                if (ValidatePriceList())
                {
                    btnAdd.IsEnabled = false;
                    lblError.Text = string.Empty;

                    PriceList priceList = ConverterHelper.ToPriceList(PriceListViewModel, PriceListViewModel.Id == 0);
                    GeneralResponse resultUpdate = await apiService.AddPriceListAsync(priceList);
                    if (resultUpdate.Success)
                        ListaPrecioActualizada?.Invoke(); // para notificar a la vista principal
                    else
                        msgError(resultUpdate.Message);
                    btnAdd.IsEnabled = true;
                }
            }
            catch (Exception ex)
            {
                msgError(ex.Message);
                btnAdd.IsEnabled = true;
            }
        }
              
        private void TextBox_PreviewMouseLeftButtonDown_SelectAll(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (sender is TextBox tb && !tb.IsKeyboardFocusWithin)
            {
                e.Handled = true; // evita que WPF cambie el foco primero
                tb.Focus();
                tb.SelectAll();
            }
        }

        private void TextBox_SelectAll(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox tb)
            {
                tb.SelectAll();
            }
        }


        private bool ValidatePriceList()
        {
            bool result = true;

            if (string.IsNullOrWhiteSpace(txtDescription.Text))
            {
                msgError("Ingrese la descripción");
                result = false;
            }

            if (string.IsNullOrWhiteSpace(txtUtility.Text))
            {
                msgError("Ingrese la utilidad");
                result = false;
            }


            return result;
        }
        private void msgError(string msg)
        {
            lblError.Text = msg;
        }

    }
}
