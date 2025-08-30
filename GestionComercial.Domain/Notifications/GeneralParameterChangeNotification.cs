using System.Text.Json.Serialization;

namespace GestionComercial.Domain.Notifications
{
    public class GeneralParameterChangeNotification
    {
        [JsonPolymorphic(TypeDiscriminatorPropertyName = "type")]
        [JsonDerivedType(typeof(GeneralParameterCreado), "created")]
        [JsonDerivedType(typeof(GeneralParameterActualizado), "updated")]
        [JsonDerivedType(typeof(GeneralParameterEliminado), "deleted")]

        public abstract record ParametroGeneralChangeNotification(int ClientId, DateTimeOffset ServerTime);

        public record GeneralParameterCreado(int ClientId, DateTimeOffset ServerTime, string Nombre)
        : ParametroGeneralChangeNotification(ClientId, ServerTime);

        public record GeneralParameterActualizado(int ClientId, DateTimeOffset ServerTime, string Nombre)
            : ParametroGeneralChangeNotification(ClientId, ServerTime);

        public record GeneralParameterEliminado(int ClientId, DateTimeOffset ServerTime)
            : ParametroGeneralChangeNotification(ClientId, ServerTime);
    }
}
