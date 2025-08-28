using System.Text.Json.Serialization;

namespace GestionComercial.Domain.Notifications
{
    public class SaleChangeNotification
    {
        [JsonPolymorphic(TypeDiscriminatorPropertyName = "type")]
        [JsonDerivedType(typeof(SaleCreado), "created")]
        [JsonDerivedType(typeof(SaleActualizado), "updated")]
        [JsonDerivedType(typeof(SaleEliminado), "deleted")]

        public abstract record VentaChangeNotification(int saleId, DateTimeOffset ServerTime);

        public record SaleCreado(int SaleId, DateTimeOffset ServerTime, string Nombre)
        : VentaChangeNotification(SaleId, ServerTime);

        public record SaleActualizado(int SaleId, DateTimeOffset ServerTime, string Nombre)
            : VentaChangeNotification(SaleId, ServerTime);

        public record SaleEliminado(int SaleId, DateTimeOffset ServerTime)
            : VentaChangeNotification(SaleId, ServerTime);
    }
}
