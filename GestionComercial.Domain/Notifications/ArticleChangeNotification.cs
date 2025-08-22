using System.Text.Json.Serialization;

namespace GestionComercial.Domain.Notifications
{
    public class ArticleChangeNotification
    {
        [JsonPolymorphic(TypeDiscriminatorPropertyName = "type")]
        [JsonDerivedType(typeof(ArticleCreado), "created")]
        [JsonDerivedType(typeof(ArticleActualizado), "updated")]
        [JsonDerivedType(typeof(ArticleEliminado), "deleted")]

        public abstract record ArticuloChangeNotification(int ClientId, DateTimeOffset ServerTime);

        public record ArticleCreado(int ClientId, DateTimeOffset ServerTime, string Nombre)
        : ArticuloChangeNotification(ClientId, ServerTime);

        public record ArticleActualizado(int ClientId, DateTimeOffset ServerTime, string Nombre)
            : ArticuloChangeNotification(ClientId, ServerTime);

        public record ArticleEliminado(int ClientId, DateTimeOffset ServerTime)
            : ArticuloChangeNotification(ClientId, ServerTime);
    }
}
