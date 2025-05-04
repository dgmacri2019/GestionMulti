using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace GestionComercial.Desktop.Helpers
{
    public class NegativoAFondoConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var texto = value?.ToString();
            return !string.IsNullOrEmpty(texto) && texto.Trim().StartsWith("-")
                ? new SolidColorBrush(Colors.Red)
                : Brushes.Transparent;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }
}
