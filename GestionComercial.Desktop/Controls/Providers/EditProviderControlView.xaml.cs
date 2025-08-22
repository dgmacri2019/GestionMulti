using GestionComercial.Desktop.Services;
using GestionComercial.Domain.DTOs.Client;
using GestionComercial.Domain.DTOs.Provider;
using GestionComercial.Domain.Entities.Masters;
using GestionComercial.Domain.Helpers;
using GestionComercial.Domain.Response;
using System.Windows;
using System.Windows.Controls;
using static GestionComercial.Domain.Constant.Enumeration;

namespace GestionComercial.Desktop.Controls.Providers
{
    /// <summary>
    /// Lógica de interacción para EditProviderControlView.xaml
    /// </summary>
    public partial class EditProviderControlView : UserControl
    {
        private readonly ProvidersApiService _providersApiService;
        private readonly int ProviderId;

        private ProviderViewModel providerViewModel { get; set; }

        public event Action ProveedorActualizado;

        public EditProviderControlView(int providerId)
        {
            InitializeComponent();
            _providersApiService = new ProvidersApiService();
            ProviderId = providerId;
            FindProvider();
            if (ProviderId == 0)
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



        private async Task FindProvider()
        {
            ProviderResponse result = await _providersApiService.GetByIdAsync(ProviderId, true, false);
            if (result.Success)
            {
                providerViewModel = result.ProviderViewModel;
                if (ProviderId == 0)
                {
                    providerViewModel.CreateUser = App.UserName;
                }
                DataContext = providerViewModel;
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
            ProveedorActualizado?.Invoke(); // para notificar a la vista principal
        }

        private async void BtnUpdate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                lblError.Text = string.Empty;

                if (ValidateProvider())
                {
                    btnUpdate.IsEnabled = false;
                    lblError.Text = string.Empty;
                    providerViewModel.UpdateUser = App.UserName;
                    providerViewModel.UpdateDate = DateTime.Now;

                    Provider provider = ConverterHelper.ToProvider(providerViewModel, providerViewModel.Id == 0);
                    GeneralResponse resultUpdate = await _providersApiService.UpdateAsync(provider);
                    if (resultUpdate.Success)
                        ProveedorActualizado?.Invoke(); // para notificar a la vista principal
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

                if (ValidateProvider())
                {
                    //providerViewModel.DocumentType = (DocumentType)Convert.ToInt32(cbDocumentTypes.SelectedValue);
                    //providerViewModel.TaxCondition = (TaxCondition)Convert.ToInt32(cbTaxConditions.SelectedValue);
                    //providerViewModel.SaleCondition = (SaleCondition)Convert.ToInt32(cbSaleConditions.SelectedValue);
                    btnAdd.IsEnabled = false;
                    lblError.Text = string.Empty;
                    Provider provider = ConverterHelper.ToProvider(providerViewModel, providerViewModel.Id == 0);
                    GeneralResponse resultUpdate = await _providersApiService.AddAsync(provider);
                    if (resultUpdate.Success)
                        ProveedorActualizado?.Invoke(); // para notificar a la vista principal
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

        private bool ValidateProvider()
        {
            bool result = true;

            if (!string.IsNullOrWhiteSpace(txtEmail.Text) && !ValidatorHelper.ValidateEmail(txtEmail.Text))
            {
                msgError("El correo electrónico es invalido");
                result = false;
            }
            if (Convert.ToInt32(cbDocumentTypes.SelectedValue) == 0)
            {
                msgError("Seleccione el tipo de documento");
                result = false;
            }
                       
            if (string.IsNullOrWhiteSpace(txtDocumentNumber.Text))
            {
                msgError("Ingrese el documento o cuit");
                result = false;
            }
            else if (Convert.ToInt32(cbDocumentTypes.SelectedValue) >= 3)
            {
                if (!ValidatorHelper.IsNumeric(txtDocumentNumber.Text))
                {
                    msgError("Ingrese solo números");
                    result = false;
                }
            }
            else if (Convert.ToInt32(cbDocumentTypes.SelectedValue) <= 2)
            {
                if (!ValidatorHelper.ValidateCuit(txtDocumentNumber.Text.Replace("-", "")))
                {
                    msgError("El cuit/cuil ingresado es inválido");
                    result = false;
                }
            }
            if (string.IsNullOrWhiteSpace(txtAddress.Text))
            {
                msgError("Ingrese el domicilio");
                result = false;
            }
            if (string.IsNullOrWhiteSpace(txtPostalCode.Text))
            {
                msgError("Ingrese el el código postal");
                result = false;
            }
            if (string.IsNullOrWhiteSpace(txtBussinessName.Text))
            {
                msgError("Ingrese la razón social");
                result = false;
            }

            if (string.IsNullOrWhiteSpace(txtCity.Text))
            {
                msgError("Ingrese la localidad");
                result = false;
            }

            if (Convert.ToInt32(cbTaxConditions.SelectedValue) == 0)
            {
                msgError("Seleccione la condición de I.V.A.");
                result = false;
            }
            if (Convert.ToInt32(cbSaleConditions.SelectedValue) == 0)
            {
                msgError("Seleccione la condición de venta");
                result = false;
            }

            if (Convert.ToInt32(cbStates.SelectedValue) == 0)
            {
                msgError("Ingrese la provincia");
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
