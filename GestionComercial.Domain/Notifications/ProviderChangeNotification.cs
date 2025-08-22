using System.Text.Json.Serialization;

namespace GestionComercial.Domain.Notifications
{
    public class ProviderChangeNotification
    {
        [JsonPolymorphic(TypeDiscriminatorPropertyName = "type")]
        [JsonDerivedType(typeof(ProviderCreado), "created")]
        [JsonDerivedType(typeof(ProviderActualizado), "updated")]
        [JsonDerivedType(typeof(ProviderEliminado), "deleted")]

        public abstract record ProveedorChangeNotification(int ProviderId, DateTimeOffset ServerTime);

        public record ProviderCreado(int ProviderId, DateTimeOffset ServerTime, string Nombre)
        : ProveedorChangeNotification(ProviderId, ServerTime);

        public record ProviderActualizado(int ProviderId, DateTimeOffset ServerTime, string Nombre)
            : ProveedorChangeNotification(ProviderId, ServerTime);

        public record ProviderEliminado(int ProviderId, DateTimeOffset ServerTime)
            : ProveedorChangeNotification(ProviderId, ServerTime);
    }
}
