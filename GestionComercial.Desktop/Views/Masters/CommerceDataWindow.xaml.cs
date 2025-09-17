using GestionComercial.Desktop.Helpers;
using GestionComercial.Desktop.Services;
using GestionComercial.Domain.Cache;
using GestionComercial.Domain.Entities.Masters;
using GestionComercial.Domain.Helpers;
using GestionComercial.Domain.Response;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using static GestionComercial.Domain.Constant.Enumeration;

namespace GestionComercial.Desktop.Views.Masters
{
    /// <summary>
    /// Lógica de interacción para CommerceDataWindow.xaml
    /// </summary>
    public partial class CommerceDataWindow : Window
    {
        private MasterClassApiService _apiService;
        private CommerceData CommerceData;

        public CommerceDataWindow()
        {
            InitializeComponent();
            _apiService = new MasterClassApiService();
            CommerceData = MasterCache.Instance.GetCommerceData();

            if (CommerceData == null)
            {
                CommerceData = new CommerceData
                {
                    CreateUser = App.UserName,
                    CreateDate = DateTime.Now,
                    ServiceEnable = true,
                    ServiceValidTo = DateTime.Now.AddDays(5),
                    SystemVersionType = SystemVersionType.PurchaseSale,
                    IsDeleted = false,
                    IsEnabled = true,
                    States = MasterCache.Instance.GetStates(),
                    IvaConditions = MasterCache.Instance.GetIvaConditions(),
                };
                dpDate.DisplayDateEnd = DateTime.Now;
                txtActivationCode.IsReadOnly = false;
                txtRegisterEmail.IsReadOnly = false;
            }
            DataContext = CommerceData;
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {

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
                Aceptar_Click(null, null);
            }
        }

        private async void Aceptar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                btnAdd.IsEnabled = false;
                lblError.Text = string.Empty;
                if (ValidateCommerceData())
                {
                    var x = CommerceData;
                    GeneralResponse resultAdd = await _apiService.AddOrUpdateCommerceDataAsync(CommerceData);
                    if (resultAdd.Success)
                    {
                        MsgBoxAlertHelper.MsgAlertInformation("Datos comerciales guardados correctamente");
                        LogOut();
                    }
                    else
                        msgError(resultAdd.Message);
                }
                btnAdd.IsEnabled = true;
            }
            catch (Exception ex)
            {
                msgError(ex.Message);
                btnAdd.IsEnabled = true;
            }
        }

        private bool ValidateCommerceData()
        {
            bool result = true;
            if (Convert.ToInt32(cbStates.SelectedValue) == 0)
            {
                result = false;
                lblError.Text = "Seleccione la Provincia";
            }

            if (Convert.ToInt32(cbIvaConditions.SelectedValue) == 0)
            {
                result = false;
                lblError.Text = "Seleccione la Condicion Fiscal";
            }

            if (string.IsNullOrEmpty(txtBusinessName.Text))
            {
                result = false;
                lblError.Text = "Ingrese la Razón Social";
            }

            if (string.IsNullOrEmpty(txtCUIT.Text))
            {
                result = false;
                lblError.Text = "Ingrese el CUIT";
            }

            if (string.IsNullOrEmpty(txtIIBB.Text))
            {
                result = false;
                lblError.Text = "Ingrese los datos de Ingresos Brutos";
            }
            if (string.IsNullOrEmpty(txtActivationCode.Text))
            {
                result = false;
                lblError.Text = "Ingrese el Código de Activación";
            }
            if (string.IsNullOrEmpty(txtRegisterEmail.Text))
            {
                result = false;
                lblError.Text = "Ingrese el Email de Registro";
            }
            return result;
        }

        private void Cancelar_Click(object sender, RoutedEventArgs e)
        {
            LogOut();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            GridGeneral.MaxWidth = this.ActualWidth;
            lblError.MaxWidth = this.ActualWidth;
        }

        private void TextBox_KeyDown_MoveNext(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                e.Handled = true; // evita el sonido del Enter
                if (sender is UIElement element)
                {
                    element.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
                }
            }
        }

        private void TextBox_SelectAll(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox tb)
            {
                tb.SelectAll();
            }
        }

        private void TextBox_PreviewMouseLeftButtonDown_SelectAll(object sender, MouseButtonEventArgs e)
        {
            if (sender is TextBox tb && !tb.IsKeyboardFocusWithin)
            {
                e.Handled = true; // evita que WPF cambie el foco primero
                tb.Focus();
                tb.SelectAll();
            }
        }

        private void txtCUIT_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (txtCUIT.IsMaskCompleted)
            {
                if (!ValidatorHelper.ValidateCuit(txtCUIT.Text))
                    msgError("El cuit ingresado es inválido");
            }
        }








        private void msgError(string msg)
        {
            lblError.Text = msg;
        }
        private void LogOut()
        {
            DialogResult = false;
        }

    }
}
