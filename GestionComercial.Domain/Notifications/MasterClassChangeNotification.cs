using System.Text.Json.Serialization;

namespace GestionComercial.Domain.Notifications
{
    public class MasterClassChangeNotification
    {
        [JsonPolymorphic(TypeDiscriminatorPropertyName = "type")]
        [JsonDerivedType(typeof(MasterClassCreado), "created")]
        [JsonDerivedType(typeof(MasterClassActualizado), "updated")]
        [JsonDerivedType(typeof(MasterClassEliminado), "deleted")]

        public abstract record ClaseMaestraChangeNotification(int id, DateTimeOffset ServerTime);

        public record MasterClassCreado(int id, DateTimeOffset ServerTime, string Nombre)
        : ClaseMaestraChangeNotification(id, ServerTime);

        public record MasterClassActualizado(int id, DateTimeOffset ServerTime, string Nombre)
            : ClaseMaestraChangeNotification(id, ServerTime);

        public record MasterClassEliminado(int id, DateTimeOffset ServerTime)
            : ClaseMaestraChangeNotification(id, ServerTime);
    }
}
