using System.Globalization;
using System.Windows.Controls;

namespace GestionComercial.Desktop.Helpers
{
    public class StringEmptyValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string input = (value ?? "").ToString();


            if (string.IsNullOrWhiteSpace(input))
                return new ValidationResult(false, "El campo es requido");
            else
                return new ValidationResult(true, "");

        }
    }
}
