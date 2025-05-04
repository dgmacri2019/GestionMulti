using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace GestionComercial.Desktop.Helpers
{
    public class NegativoATextoConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var texto = value?.ToString();
            return !string.IsNullOrEmpty(texto) && texto.Trim().StartsWith("-")
                ? Brushes.White
                : Brushes.Black;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }
}
