using System.Text.Json.Serialization;
using static GestionComercial.Domain.Constant.Enumeration;

namespace GestionComercial.Domain.Notifications
{
    public class ClientChangeNotification
    {
        [JsonPolymorphic(TypeDiscriminatorPropertyName = "type")]
        [JsonDerivedType(typeof(ClientCreado), "created")]
        [JsonDerivedType(typeof(ClientActualizado), "updated")]
        [JsonDerivedType(typeof(ClientEliminado), "deleted")]

        public abstract record ClienteChangeNotification(List<int> ClientId, DateTimeOffset ServerTime, ChangeType Action);

        public record ClientCreado(List<int> ClientId, DateTimeOffset ServerTime, ChangeType Action)
        : ClienteChangeNotification(ClientId, ServerTime, Action);

        public record ClientActualizado(List<int> ClientId, DateTimeOffset ServerTime, ChangeType Action)
            : ClienteChangeNotification(ClientId, ServerTime, Action);

        public record ClientEliminado(List<int> ClientId, DateTimeOffset ServerTime, ChangeType Action)
            : ClienteChangeNotification(ClientId, ServerTime, Action);
    }
}
