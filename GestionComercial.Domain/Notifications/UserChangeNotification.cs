using System.Text.Json.Serialization;

namespace GestionComercial.Domain.Notifications
{
    public class UserChangeNotification
    {
        [JsonPolymorphic(TypeDiscriminatorPropertyName = "type")]
        [JsonDerivedType(typeof(UserCreado), "created")]
        [JsonDerivedType(typeof(UserActualizado), "updated")]
        [JsonDerivedType(typeof(UserEliminado), "deleted")]

        public abstract record UsuarioChangeNotification(string UserId, DateTimeOffset ServerTime);

        public record UserCreado(string UserId, DateTimeOffset ServerTime, string Nombre)
        : UsuarioChangeNotification(UserId, ServerTime);

        public record UserActualizado(string UserId, DateTimeOffset ServerTime, string Nombre)
            : UsuarioChangeNotification(UserId, ServerTime);

        public record UserEliminado(string UserId, DateTimeOffset ServerTime)
            : UsuarioChangeNotification(UserId, ServerTime);
    }
}
