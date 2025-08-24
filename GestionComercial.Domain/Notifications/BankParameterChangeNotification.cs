using System.Text.Json.Serialization;

namespace GestionComercial.Domain.Notifications
{
    public class BankParameterChangeNotification
    {
        [JsonPolymorphic(TypeDiscriminatorPropertyName = "type")]
        [JsonDerivedType(typeof(BankParameterCreado), "created")]
        [JsonDerivedType(typeof(BankParameterActualizado), "updated")]
        [JsonDerivedType(typeof(BankParameterEliminado), "deleted")]

        public abstract record ParametroBancarioChangeNotification(int Id, DateTimeOffset ServerTime);

        public record BankParameterCreado(int Id, DateTimeOffset ServerTime, string Nombre)
        : ParametroBancarioChangeNotification(Id, ServerTime);

        public record BankParameterActualizado(int Id, DateTimeOffset ServerTime, string Nombre)
            : ParametroBancarioChangeNotification(Id, ServerTime);

        public record BankParameterEliminado(int Id, DateTimeOffset ServerTime)
            : ParametroBancarioChangeNotification(Id, ServerTime);
    }
}
