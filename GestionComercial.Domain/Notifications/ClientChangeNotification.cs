using System.Text.Json.Serialization;

namespace GestionComercial.Domain.Notifications
{
    public class ClientChangeNotification
    {
        [JsonPolymorphic(TypeDiscriminatorPropertyName = "type")]
        [JsonDerivedType(typeof(ClientCreado), "created")]
        [JsonDerivedType(typeof(ClientActualizado), "updated")]
        [JsonDerivedType(typeof(ClientEliminado), "deleted")]
       
        public abstract record ClienteChangeNotification(int ClientId, DateTimeOffset ServerTime);

        public record ClientCreado(int ClientId, DateTimeOffset ServerTime, string Nombre)
        : ClienteChangeNotification(ClientId, ServerTime);

        public record ClientActualizado(int ClientId, DateTimeOffset ServerTime, string Nombre)
            : ClienteChangeNotification(ClientId, ServerTime);

        public record ClientEliminado(int ClientId, DateTimeOffset ServerTime)
            : ClienteChangeNotification(ClientId, ServerTime);
    }
}
