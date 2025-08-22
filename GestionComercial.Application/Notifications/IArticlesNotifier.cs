using static GestionComercial.Domain.Constant.Enumeration;

namespace GestionComercial.Applications.Notifications
{
    public interface IArticlesNotifier
    {
        Task NotifyAsync(int articleId, string description, ChangeType action);
    }
}
