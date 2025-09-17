using GestionComercial.Desktop.Helpers;
using GestionComercial.Desktop.Services;
using GestionComercial.Domain.Entities.Masters;
using GestionComercial.Domain.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace GestionComercial.Desktop.Views.Masters
{
    /// <summary>
    /// Lógica de interacción para BillingWindow.xaml
    /// </summary>
    public partial class BillingWindow : Window
    {
        private MasterClassApiService _apiService;
        private int opcionSeleccionada;

        private Billing Billing;
        public BillingWindow()
        {
            InitializeComponent();
            _apiService = new MasterClassApiService();
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
                if (ValidateBilling())
                {
                    GeneralResponse resultAdd = await _apiService.AddOrUpdateBillingAsync(Billing);
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

        private bool ValidateBilling()
        {
            throw new NotImplementedException();
        }

       
        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            if (sender is RadioButton rb && rb.Tag != null)
            {
                opcionSeleccionada = int.Parse(rb.Tag.ToString()!);
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
    }
}
