using GestionComercial.Desktop.Helpers;
using GestionComercial.Desktop.Services;
using GestionComercial.Domain.Cache;
using GestionComercial.Domain.DTOs.Master.Configurations.Commerce;
using GestionComercial.Domain.Helpers;
using GestionComercial.Domain.Response;
using Microsoft.Win32;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace GestionComercial.Desktop.Views.Masters
{
    /// <summary>
    /// Lógica de interacción para BillingWindow.xaml
    /// </summary>
    public partial class BillingWindow : Window
    {
        private MasterClassApiService _apiService;
        private OpenFileDialog FileDialogCert = new()
        {
            Filter = "PFX (*.pfx)|*.pfx",
            FilterIndex = 1,
            Multiselect = false,
            Title = "Seleccione el certificado"
        };

        private BillingViewModel BillingViewModel;
        public BillingWindow()
        {
            InitializeComponent();
            _apiService = new MasterClassApiService();
            BillingViewModel = MasterCache.Instance.GetBilling();
            if (BillingViewModel == null)
                BillingViewModel = new BillingViewModel
                {
                    CreateDate = DateTime.Now,
                    CreateUser = LoginUserCache.UserName,
                    IsEnabled = true,
                    PadronExpirationTime = DateTime.Now,
                    PadronGenerationTime = DateTime.Now,
                    PadronSign = "System",
                    PadronToken = "System",
                    WSDLExpirationTime = DateTime.Now,
                    WSDLGenerationTime = DateTime.Now,
                    WSDLSign = "System",
                    WSDLToken = "System",
                    CommerceDataId = MasterCache.Instance.GetCommerceData().Id,
                    Concept = 1,
                };
            else
            {
                BillingViewModel.CertPass = CryptoHelper.Decrypt(BillingViewModel.CertPass);
                BillingViewModel.UpdateDate = DateTime.Now;
                BillingViewModel.UpdateUser = LoginUserCache.UserName;
                rbProduct.IsChecked = BillingViewModel.Concept == 1;
                rbService.IsChecked = BillingViewModel.Concept == 2;
                rbProductAndService.IsChecked = BillingViewModel.Concept == 3;
            }
            DataContext = BillingViewModel;
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            //TODO: Usar Homologacion o Produccion
            //TODO: Ver por que no marca el certificado

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
                if (ValidateBilling())
                {
                    BillingViewModel.CertPass = CryptoHelper.Encrypt(BillingViewModel.CertPass);
                    if (BillingViewModel.CertPath != null && !string.IsNullOrEmpty(BillingViewModel.CertPath))
                    {
                        byte[]? certificateByteArray = null;
                        certificateByteArray = FileHelper.FilePathToByteArray(BillingViewModel.CertPath);
                        if (certificateByteArray != null && certificateByteArray.Length > 0)
                        {
                            BillingViewModel.CertificateByteArray = certificateByteArray;
                        }
                    }

                    GeneralResponse resultAdd = await _apiService.AddOrUpdateBillingAsync(BillingViewModel);
                    if (resultAdd.Success)
                    {
                        MsgBoxAlertHelper.MsgAlertInformation("Datos fiscales guardados correctamente");
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

        private bool ValidateBilling()
        {
            bool result = true;

            if (BillingViewModel.Concept == 0)
            {
                msgError("Debe seleccionar el concepto de las facturas");
                result = false;
            }
            if ((chbUseWSDL.IsChecked == true || chbUsePadron.IsChecked == true) && string.IsNullOrEmpty(txtPassword.Text))
            {
                msgError("Debe ingresar el Password del certificado");
                result = false;
            }
            return result;
        }


        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            if (sender is RadioButton rb && rb.Tag != null && BillingViewModel != null)
            {
                BillingViewModel.Concept = int.Parse(rb.Tag.ToString()!);
                // Ahora opcionSeleccionada tiene 1, 2 o 3 según la opción
            }
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


        private void msgError(string msg)
        {
            lblError.Text = msg;
        }
        private void LogOut()
        {
            DialogResult = false;
        }

        private void btnCertificate_Click(object sender, RoutedEventArgs e)
        {
            if (FileDialogCert.ShowDialog() == true)
            {
                txtCertificate.Text = FileDialogCert.FileName;
                BillingViewModel.CertPath = FileDialogCert.FileName;
                chbUseWSDL.IsChecked = true;
            }
        }

        private void btnGenerateToken_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("¿Confirma el forzado de generación de tokens?", "Alerta", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                //LogInServiceAfip logInServiceAfip = new LogInServiceAfip();
                //if (StaticCommerceData.Billing.UsePadron && StaticCommerceData.Billing.UseWSDL)
                //{
                //    await logInServiceAfip.LogInAsync(AfipLogInType.PadronA13, true);
                //    await logInServiceAfip.LogInAsync(AfipLogInType.PadronA5, true);
                //    await logInServiceAfip.LogInAsync(AfipLogInType.WSDL, true);
                //}
                //else if (!StaticCommerceData.Billing.UsePadron && StaticCommerceData.Billing.UseWSDL)
                //    await logInServiceAfip.LogInAsync(AfipLogInType.WSDL, true);

                //else if (StaticCommerceData.Billing.UsePadron && !StaticCommerceData.Billing.UseWSDL)
                //{
                //    await logInServiceAfip.LogInAsync(AfipLogInType.PadronA13, true);
                //    await logInServiceAfip.LogInAsync(AfipLogInType.PadronA5, true);
                //}
            }
        }
    }
}
