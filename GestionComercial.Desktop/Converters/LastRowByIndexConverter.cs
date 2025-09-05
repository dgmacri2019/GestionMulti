using System.Globalization;
using System.Windows.Data;

namespace GestionComercial.Desktop.Converters
{
    public class LastRowByIndexConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length < 2) return false;

            if (values[0] is int index && values[1] is int count)
            {
                return index == count - 1; // true solo para la última fila
            }

            return false;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }
}
