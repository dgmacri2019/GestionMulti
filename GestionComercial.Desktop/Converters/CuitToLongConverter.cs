using System.Globalization;
using System.Windows.Data;

namespace GestionComercial.Desktop.Converters
{
    public class CuitToLongConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Mostrar con guiones
            if (value is long cuit)
            {
                string s = cuit.ToString("00000000000"); // 11 dígitos
                return $"{s.Substring(0, 2)}-{s.Substring(2, 8)}-{s.Substring(10, 1)}";
            }
            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string s)
            {
                string digits = s.Replace("-", "").Trim();
                if (long.TryParse(digits, out long result))
                    return result;
            }
            return 0L; // o DependencyProperty.UnsetValue para invalidar
        }
    }
}
