using static GestionComercial.Domain.Constant.Enumeration;

namespace GestionComercial.Applications.Notifications
{
    public interface IUsersNotifier
    {
        Task NotifyAsync(string userId, string name, ChangeType action);
    }
}
