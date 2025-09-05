using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace GestionComercial.Desktop.Converters
{
    public class StockMultiConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values[0] is decimal stock && values[1] is decimal minimalStock)
            {
                if (stock < 0)
                    return new SolidColorBrush(Colors.Red);     // negativo → rojo
                if (stock < minimalStock)
                    return new SolidColorBrush(Color.FromRgb(255,249,32));  // menor que el threshold → amarillo
                return Brushes.Transparent; // sino transparente
            }

            return Brushes.Transparent;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }
}
