using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows.Controls;

namespace GestionComercial.Desktop.Helpers
{
    public class EmailValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string input = (value ?? "").ToString();


            if (string.IsNullOrWhiteSpace(input))
                return new ValidationResult(true,"");

            string pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";

            if (!Regex.IsMatch(input, pattern))
                return new ValidationResult(false, "Email no válido.");

            return ValidationResult.ValidResult;
        }
    }
}
