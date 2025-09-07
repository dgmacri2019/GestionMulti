using System.Globalization;

namespace GestionComercial.Domain.DTOs.Sale
{
    public class PayMethodItem : ObservableObject
    {
        public int MetodoId { get; set; }
        public decimal Monto { get; set; }
    }

    public class MetodoPago
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
    }
}