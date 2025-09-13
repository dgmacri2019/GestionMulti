using System.Text.Json.Serialization;
using static GestionComercial.Domain.Constant.Enumeration;

namespace GestionComercial.Domain.Notifications
{
    public class MasterClassChangeNotification
    {
        [JsonPolymorphic(TypeDiscriminatorPropertyName = "type")]
        [JsonDerivedType(typeof(MasterClassCreado), "created")]
        [JsonDerivedType(typeof(MasterClassActualizado), "updated")]
        [JsonDerivedType(typeof(MasterClassEliminado), "deleted")]

        public abstract record ClaseMaestraChangeNotification(int Id, DateTimeOffset ServerTime, ChangeType Action, ChangeClass ChangeClass);

        public record MasterClassCreado(int Id, DateTimeOffset ServerTime, string Nombre, ChangeType Action, ChangeClass ChangeClass)
        : ClaseMaestraChangeNotification(Id, ServerTime, Action, ChangeClass);

        public record MasterClassActualizado(int Id, DateTimeOffset ServerTime, string Nombre, ChangeType Action, ChangeClass ChangeClass)
            : ClaseMaestraChangeNotification(Id, ServerTime, Action, ChangeClass);

        public record MasterClassEliminado(int Id, DateTimeOffset ServerTime, ChangeType Action, ChangeClass ChangeClass)
            : ClaseMaestraChangeNotification(Id, ServerTime, Action, ChangeClass);
    }
}
