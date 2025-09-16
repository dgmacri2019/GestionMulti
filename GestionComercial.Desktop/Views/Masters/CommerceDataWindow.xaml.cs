using GestionComercial.Domain.Helpers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace GestionComercial.Desktop.Views.Masters
{
    /// <summary>
    /// Lógica de interacción para CommerceDataWindow.xaml
    /// </summary>
    public partial class CommerceDataWindow : Window
    {
        public CommerceDataWindow()
        {
            InitializeComponent();
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {

        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void TextBox_KeyDown_MoveNext(object sender, KeyEventArgs e)
        {

        }

        private void TextBox_SelectAll(object sender, RoutedEventArgs e)
        {

        }

        private void TextBox_PreviewMouseLeftButtonDown_SelectAll(object sender, MouseButtonEventArgs e)
        {

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

        private void Aceptar_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Cancelar_Click(object sender, RoutedEventArgs e)
        {
            LogOut();
        }


        private void LogOut()
        {
            DialogResult = false;
        }

    }
}
