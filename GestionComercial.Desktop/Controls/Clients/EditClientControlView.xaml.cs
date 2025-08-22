using GestionComercial.Desktop.Cache;
using GestionComercial.Desktop.Services;
using GestionComercial.Domain.DTOs.Client;
using GestionComercial.Domain.Entities.Masters;
using GestionComercial.Domain.Helpers;
using GestionComercial.Domain.Response;
using System.Windows;
using System.Windows.Controls;
using static GestionComercial.Domain.Constant.Enumeration;

namespace GestionComercial.Desktop.Controls.Clients
{
    /// <summary>
    /// Lógica de interacción para EditClientControlView.xaml
    /// </summary>
    public partial class EditClientControlView : UserControl
    {
        private readonly ClientsApiService _clientsApiService;
        private readonly int ClientId;

        private ClientViewModel clientViewModel { get; set; }

        public event Action ClienteActualizado;


        public EditClientControlView(int clientId)
        {
            InitializeComponent();
            _clientsApiService = new ClientsApiService();
            ClientId = clientId;
            _ = FindClientAsync();
            if (ClientId > 0)
            {
                btnAdd.Visibility = Visibility.Hidden;
                btnUpdate.Visibility = Visibility.Visible;
            }
            else
            {
                btnAdd.Visibility = Visibility.Visible;
                btnUpdate.Visibility = Visibility.Hidden;
            }
        }

        private async Task FindClientAsync()
        {
            ClientResponse result = await _clientsApiService.GetByIdAsync(ClientId);

            clientViewModel = ClientCache.Instance.GetAllClients().FirstOrDefault(c => c.Id == ClientId);

            if (result.Success)
            {
                if (ClientId == 0)
                {
                    clientViewModel = result.ClientViewModel;
                    clientViewModel.CreateUser = App.UserName;
                }
                DataContext = clientViewModel;
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
            ClienteActualizado?.Invoke(); // para notificar a la vista principal
        }

        private async void BtnUpdate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                lblError.Text = string.Empty;

                if (ValidateClient())
                {
                    btnUpdate.IsEnabled = false;
                    lblError.Text = string.Empty;
                    clientViewModel.UpdateUser = App.UserName;
                    clientViewModel.UpdateDate = DateTime.Now;

                    Client client = ConverterHelper.ToClient(clientViewModel, clientViewModel.Id == 0);
                    GeneralResponse resultUpdate = await _clientsApiService.UpdateAsync(client);
                    if (resultUpdate.Success)
                        ClienteActualizado?.Invoke(); // para notificar a la vista principal
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

                if (ValidateClient())
                {
                    //clientViewModel.DocumentType = (DocumentType)Convert.ToInt32(cbDocumentTypes.SelectedValue);
                    //clientViewModel.TaxCondition = (TaxCondition)Convert.ToInt32(cbTaxConditions.SelectedValue);
                    //clientViewModel.SaleCondition = (SaleCondition)Convert.ToInt32(cbSaleConditions.SelectedValue);
                    btnAdd.IsEnabled = false;
                    lblError.Text = string.Empty;
                    Client client = ConverterHelper.ToClient(clientViewModel, clientViewModel.Id == 0);
                    GeneralResponse resultUpdate = await _clientsApiService.AddAsync(client);
                    if (resultUpdate.Success)
                        ClienteActualizado?.Invoke(); // para notificar a la vista principal
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

        private bool ValidateClient()
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

            if (Convert.ToInt32(cbPriceLists.SelectedValue) == 0)
            {
                msgError("Seleccione la lista de precios");
                result = false;
            }
            if (string.IsNullOrWhiteSpace(txtDocumentNumber.Text))
            {
                msgError("Ingrese el documento, cuit o cuil");
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
