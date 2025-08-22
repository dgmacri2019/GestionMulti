using Microsoft.AspNetCore.SignalR;
using static GestionComercial.Domain.Notifications.ArticleChangeNotification;

namespace GestionComercial.API.Hubs
{
    public interface IArticlesClient
    {
        Task ArticulosActualizados(ArticuloChangeNotification notification);
    }

    // Hub tipado
    public class ArticlesHub : Hub<IArticlesClient>
    {
        // Podés sobreescribir OnConnectedAsync / OnDisconnectedAsync para logging, grupos, etc.
    }
}
