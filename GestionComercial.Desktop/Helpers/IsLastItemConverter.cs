using System.Collections;
using System.Globalization;
using System.Windows.Data;

namespace GestionComercial.Desktop.Helpers
{
    public class IsLastItemConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var currentItem = values[0];
            var collection = values[1] as IEnumerable;

            if (currentItem == null || collection == null)
                return false;

            object last = null;
            foreach (var item in collection)
                last = item; // obtengo el último

            return ReferenceEquals(currentItem, last);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }
}
