using System.Text.Json.Serialization;

namespace GestionComercial.Domain.Notifications
{
    public class InvoiceChangeNotification
    {
        [JsonPolymorphic(TypeDiscriminatorPropertyName = "type")]
        [JsonDerivedType(typeof(InvoiceCreado), "created")]
        [JsonDerivedType(typeof(InvoiceActualizado), "updated")]
        [JsonDerivedType(typeof(InvoiceEliminado), "deleted")]

        public abstract record FacturaChangeNotification(int InvoiceId, DateTimeOffset ServerTime);

        public record InvoiceCreado(int InvoiceId, DateTimeOffset ServerTime, string Nombre)
        : FacturaChangeNotification(InvoiceId, ServerTime);

        public record InvoiceActualizado(int InvoiceId, DateTimeOffset ServerTime, string Nombre)
            : FacturaChangeNotification(InvoiceId, ServerTime);

        public record InvoiceEliminado(int InvoiceId, DateTimeOffset ServerTime)
            : FacturaChangeNotification(InvoiceId, ServerTime);
    }
}
