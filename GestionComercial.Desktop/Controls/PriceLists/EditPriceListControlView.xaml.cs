using GestionComercial.Desktop.Services;
using GestionComercial.Domain.DTOs.PriceLists;
using GestionComercial.Domain.Entities.Stock;
using GestionComercial.Domain.Helpers;
using GestionComercial.Domain.Response;
using System.Windows;
using System.Windows.Controls;

namespace GestionComercial.Desktop.Controls.PriceLists
{
    /// <summary>
    /// Lógica de interacción para EditPriceListControlView.xaml
    /// </summary>
    public partial class EditPriceListControlView : UserControl
    {
        private readonly PriceListsApiService _priceListsApiService;
        private readonly int PriceListId;
        private PriceListViewModel PriceListViewModel { get; set; }

        public event Action ListaPrecioActualizada;

        public EditPriceListControlView(int priceListId)
        {
            InitializeComponent();
            _priceListsApiService = new PriceListsApiService();
            PriceListId = priceListId;
            FindPriceList();
            if (PriceListId == 0)
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

        private async Task FindPriceList()
        {
            PriceListResponse result = await _priceListsApiService.GetByIdAsync(PriceListId, true, false);
            if (result.Success)
            {
                PriceListViewModel = result.PriceListViewModel;
                if (PriceListId == 0)
                {
                    PriceListViewModel.CreateUser = App.UserName;
                }
                DataContext = PriceListViewModel;
            }
            else
                lblError.Text = result.Message;
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
                    GeneralResponse resultUpdate = await _priceListsApiService.UpdateAsync(priceList);
                    if (resultUpdate.Success)
                        ListaPrecioActualizada?.Invoke(); // para notificar a la vista principal
                    else
                        lblError.Text = resultUpdate.Message;
                    btnUpdate.IsEnabled = true;
                }
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
            }
        }

        private async void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                lblError.Text = string.Empty;

                if (ValidatePriceList())
                {
                    btnAdd.IsEnabled = false;
                    lblError.Text = string.Empty;
                    PriceList priceList = ConverterHelper.ToPriceList(PriceListViewModel, PriceListViewModel.Id == 0);
                    GeneralResponse resultUpdate = await _priceListsApiService.AddAsync(priceList);
                    if (resultUpdate.Success)
                        ListaPrecioActualizada?.Invoke(); // para notificar a la vista principal
                    else
                        lblError.Text = resultUpdate.Message;
                    btnAdd.IsEnabled = true;
                }
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
            }
        }

        private void txtUtility_LostFocus(object sender, RoutedEventArgs e)
        {
            txtUtility.Text = txtUtility.Text.Replace(".", ",");
        }

        private void txtUtility_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            string textoIngresado = e.Text;
            bool result = false;
            if (ValidatorHelper.IsNumeric(textoIngresado) || textoIngresado == "." || textoIngresado == ",")
                result = false;
            else
                result = true;
            e.Handled = result;
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
