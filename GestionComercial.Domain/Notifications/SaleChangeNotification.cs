using System.Text.Json.Serialization;
using static GestionComercial.Domain.Constant.Enumeration;

namespace GestionComercial.Domain.Notifications
{
    public class SaleChangeNotification
    {
        [JsonPolymorphic(TypeDiscriminatorPropertyName = "type")]
        [JsonDerivedType(typeof(SaleCreado), "created")]
        [JsonDerivedType(typeof(SaleActualizado), "updated")]
        [JsonDerivedType(typeof(SaleEliminado), "deleted")]

        public abstract record VentaChangeNotification(int SaleId, DateTimeOffset ServerTime, ChangeType action);

        public record SaleCreado(int SaleId, DateTimeOffset ServerTime, string Nombre, ChangeType action)
        : VentaChangeNotification(SaleId, ServerTime, action);

        public record SaleActualizado(int SaleId, DateTimeOffset ServerTime, string Nombre, ChangeType action)
            : VentaChangeNotification(SaleId, ServerTime, action);

        public record SaleEliminado(int SaleId, DateTimeOffset ServerTime, ChangeType action)
            : VentaChangeNotification(SaleId, ServerTime, action);
    }
}
