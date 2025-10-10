using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace GestionComercial.Desktop.Converters
{
    public class NegativoAVisibleConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            Visibility visibility = Visibility.Collapsed;
            if (values.Length < 3)
                return visibility;
            else if (values.Length == 3) //ANULAR
            {
                bool hasCAE = values[0] is bool b && b;

                decimal total = 0;
                if (values[1] != null)
                {
                    if (values[1] is decimal dec)
                        total = dec;
                    else if (decimal.TryParse(values[1].ToString(), out var parsed))
                        total = parsed;
                }
                bool isDeleted = values[2] is bool d && d;

                // Ocultar si el total es negativo o si está eliminado
                if (total < 0 || isDeleted)
                    visibility = Visibility.Collapsed;
                else
                    visibility = Visibility.Visible;
            }
            else if(values.Length == 4) //FACTURAR
            {
                bool hasCAE = values[0] is bool b && b;

                decimal total = 0;
                if (values[1] != null)
                {
                    if (values[1] is decimal dec)
                        total = dec;
                    else if (decimal.TryParse(values[1].ToString(), out var parsed))
                        total = parsed;
                }
                bool isDeleted = values[2] is bool d && d;

                // Ocultar si el total es negativo o si está eliminado
                if (total < 0 || isDeleted || hasCAE)
                    visibility = Visibility.Collapsed;
                else
                    visibility = Visibility.Visible;
            }
                

           

           

            
            

            return visibility;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }
}