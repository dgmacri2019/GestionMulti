using System.Globalization;
using System.Windows.Data;

namespace GestionComercial.Desktop.Converters
{
    public class TwoDecimalMoneyCommaConverter : IValueConverter
    { // Convierte de decimal a string con coma como separador y 2 decimales
        // Solo para mostrar en el TextBox (cuando binding actualiza el control)
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is decimal dec)
                return dec.ToString("C2", new CultureInfo("es-AR"));
            return value;
        }

        // Convierte de string a decimal, pero devuelve Binding.DoNothing si no se puede parsear
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string s)
            {
                s = s.Replace('.', ','); // acepta punto como decimal
                if (decimal.TryParse(s, NumberStyles.Any, new CultureInfo("es-AR"), out decimal dec))
                    return dec;

                // Si no se puede parsear, no actualiza el ViewModel todavía
                return Binding.DoNothing;
            }
            return Binding.DoNothing;
        }
    }
}
