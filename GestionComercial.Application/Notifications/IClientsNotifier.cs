using static GestionComercial.Domain.Constant.Enumeration;

namespace GestionComercial.Applications.Notifications
{
    public interface IClientsNotifier
    {
        Task NotifyAsync(List<int> clientId, string businessName, ChangeType action);
    }
}
