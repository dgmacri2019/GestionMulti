using static GestionComercial.Domain.Constant.Enumeration;
using static GestionComercial.Domain.Notifications.ClientChangeNotification;

namespace GestionComercial.Applications.Notifications
{
    public interface IClientsNotifier
    {
        Task NotifyAsync(int clientId, string businessName, ChangeType action);
    }
}
