using System.Text.Json.Serialization;
using static GestionComercial.Domain.Constant.Enumeration;

namespace GestionComercial.Domain.Notifications
{
    public class ArticleChangeNotification
    {
        [JsonPolymorphic(TypeDiscriminatorPropertyName = "type")]
        [JsonDerivedType(typeof(ArticleCreado), "created")]
        [JsonDerivedType(typeof(ArticleActualizado), "updated")]
        [JsonDerivedType(typeof(ArticleEliminado), "deleted")]

        public abstract record ArticuloChangeNotification(List<int> ClientId, DateTimeOffset ServerTime, ChangeType action);

        public record ArticleCreado(List<int> ClientId, DateTimeOffset ServerTime, string Nombre, ChangeType action)
        : ArticuloChangeNotification(ClientId, ServerTime, action);

        public record ArticleActualizado(List<int> ClientId, DateTimeOffset ServerTime, string Nombre, ChangeType action)
            : ArticuloChangeNotification(ClientId, ServerTime, action);

        public record ArticleEliminado(List<int> ClientId, DateTimeOffset ServerTime, ChangeType action)
            : ArticuloChangeNotification(ClientId, ServerTime, action);
    }
}
