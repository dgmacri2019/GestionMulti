using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace GestionComercial.Desktop.Converters
{
    public class HexToBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string hex && !string.IsNullOrWhiteSpace(hex))
            {
                try
                {
                    var color = (Color)ColorConverter.ConvertFromString(hex);
                    return new SolidColorBrush(color);
                }
                catch { return Brushes.Transparent; }
            }
            return Brushes.Transparent;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is SolidColorBrush brush)
            {
                return $"#{brush.Color.R:X2}{brush.Color.G:X2}{brush.Color.B:X2}";
            }
            return "#000000";
        }
    }
}
