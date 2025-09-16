using GestionComercial.Desktop.Services;
using GestionComercial.Domain.Cache;
using GestionComercial.Domain.DTOs.Client;
using GestionComercial.Domain.Entities.Masters;
using GestionComercial.Domain.Helpers;
using GestionComercial.Domain.Response;
using System.Windows;
using System.Windows.Controls;

namespace GestionComercial.Desktop.Controls.Clients
{
    /// <summary>
    /// Lógica de interacción para EditClientControlView.xaml
    /// </summary>
    public partial class EditClientControlView : UserControl
    {
        private readonly ClientsApiService _clientsApiService;
        private readonly int ClientId;

        private EditClientViewModel editVM = new();

        public event Action ClienteActualizado;


        public EditClientControlView(int clientId)
        {
            InitializeComponent();
            _clientsApiService = new ClientsApiService();
            ClientId = clientId;

            editVM.PriceLists = PriceListCache.Instance.GetAll().Where(pl => pl.IsEnabled).ToList();
            editVM.States = MasterCache.Instance.GetStates();
            editVM.SaleConditions = MasterCache.Instance.GetSaleConditions();
            editVM.IvaConditions = MasterCache.Instance.GetIvaConditions();
            editVM.DocumentTypes = MasterCache.Instance.GetDocumentTypes();

            if (ClientId == 0)
            {
                editVM.Client = new ClientViewModel { CreateUser = App.UserName };
                btnAdd.Visibility = Visibility.Visible;
                btnUpdate.Visibility = Visibility.Hidden;
            }
            else
            {
                editVM.Client = ClientCache.Instance.GetAll()
                                      .FirstOrDefault(c => c.Id == ClientId);
                btnAdd.Visibility = Visibility.Hidden;
                btnUpdate.Visibility = Visibility.Visible;
                if (editVM.Client == null)
                    lblError.Text = "No se reconoce al cliente";
            }

            DataContext = editVM;
        }
              
        private void miUserControl_Loaded(object sender, RoutedEventArgs e)
        {
            GridGeneral.MaxWidth = this.ActualWidth;
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
                    editVM.Client.UpdateUser = App.UserName;
                    editVM.Client.UpdateDate = DateTime.Now;

                    Client client = ConverterHelper.ToClient(editVM.Client, editVM.Client.Id == 0);
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
                    Client client = ConverterHelper.ToClient(editVM.Client, editVM.Client.Id == 0);
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
            //if (Convert.ToInt32(cbSaleConditions.SelectedValue) == 0)
            //{
            //    msgError("Seleccione la condición de venta");
            //    result = false;
            //}

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

        private void txtSold_GotKeyboardFocus(object sender, System.Windows.Input.KeyboardFocusChangedEventArgs e)
        {
            if (sender is TextBox tb)
                tb.SelectAll();
        }

        private void txtSold_PreviewMouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            // Si el TextBox aún no tiene el foco, prevení que el clic mueva el caret
            if (sender is TextBox tb && !tb.IsKeyboardFocusWithin)
            {
                e.Handled = true;   // Consumí este clic
                tb.Focus();         // Forzá el foco -> dispara GotKeyboardFocus -> SelectAll()
            }
        }

        private void txtSold_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            string textoIngresado = e.Text;
            bool result = false;
            if (ValidatorHelper.IsNumeric(textoIngresado) || textoIngresado == "." || textoIngresado == ",")
                result = false;
            else
                result = true;
            e.Handled = result;
        }

        private void txtSold_LostFocus(object sender, RoutedEventArgs e)
        {
            txtSold.Text = txtSold.Text.Replace(".", ",");
        }
    }
}
