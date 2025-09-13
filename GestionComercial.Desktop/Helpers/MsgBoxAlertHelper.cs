using System.Windows;

namespace GestionComercial.Desktop.Helpers
{
    internal static class MsgBoxAlertHelper
    {
        internal static void MsgAlertError(string message)
        {
            MessageBox.Show(message, "Aviso al operador", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
