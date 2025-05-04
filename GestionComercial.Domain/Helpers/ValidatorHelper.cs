using System.Text.RegularExpressions;

namespace GestionComercial.Domain.Helpers
{
    public static class ValidatorHelper
    {
        #region Method


        public static bool ValidateCuit(string cuit)
        {
            if (string.IsNullOrWhiteSpace(cuit)) return false;
            string cuit_nro = cuit.Replace("-", string.Empty);
            if (cuit_nro.Length != 11) return false;
            bool rv = false;
            int verificador;
            int resultado = 0;
            string codes = "6789456789";
            long cuit_long = 0;
            if (long.TryParse(cuit_nro, out cuit_long))
            {
                verificador = int.Parse(cuit_nro[cuit_nro.Length - 1].ToString());
                int x = 0;
                while (x < 10)
                {
                    int digitoValidador = int.Parse(codes.Substring((x), 1));
                    int digito = int.Parse(cuit_nro.Substring((x), 1));
                    int digitoValidacion = digitoValidador * digito;
                    resultado += digitoValidacion;
                    x++;
                }
                resultado %= 11;
                rv = (resultado == verificador);
            }
            return rv;
        }

        //
        //Resumen:
        //     Indica si la cadena especificada corresponde a un formato numérico.
        //     <br></br>
        //     <br></br>       
        // Devuelve true si el parámetro val corresponde a una formato numérico.
        public static bool IsNumeric(string val) => float.TryParse(val, out float result);

        ///<remarks>
        ///Resumen:
        ///     Indica si la cadena especificada corresponde a un formato de correo electrónico.
        ///     <br></br>
        ///     <br></br>       
        /// Devuelve true si el parámetro email corresponde a una formato de correo electrónico.
        ///</remarks>
        public static bool ValidateEmail(string email)
        {
            string expresion = "\\w+([-+.']\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*";
            if (Regex.IsMatch(email, expresion))
                return Regex.Replace(email, expresion, String.Empty).Length == 0;
            else
                return false;
        }


        public static bool EsPar(int number)
        {
            return number % 2 == 0;
        }

        #endregion
    }
}
