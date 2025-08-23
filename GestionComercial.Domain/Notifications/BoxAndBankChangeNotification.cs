using System.Text.Json.Serialization;

namespace GestionComercial.Domain.Notifications
{
    public class BoxAndBankChangeNotification
    {
        [JsonPolymorphic(TypeDiscriminatorPropertyName = "type")]
        [JsonDerivedType(typeof(BoxAndBankCreado), "created")]
        [JsonDerivedType(typeof(BoxAndBankActualizado), "updated")]
        [JsonDerivedType(typeof(BoxAndBankEliminado), "deleted")]

        public abstract record CajaYBancoChangeNotification(int Id, DateTimeOffset ServerTime);

        public record BoxAndBankCreado(int Id, DateTimeOffset ServerTime, string Nombre)
        : CajaYBancoChangeNotification(Id, ServerTime);

        public record BoxAndBankActualizado(int Id, DateTimeOffset ServerTime, string Nombre)
            : CajaYBancoChangeNotification(Id, ServerTime);

        public record BoxAndBankEliminado(int Id, DateTimeOffset ServerTime)
            : CajaYBancoChangeNotification(Id, ServerTime);
    }
}
