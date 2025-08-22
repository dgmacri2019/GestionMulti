using static GestionComercial.Domain.Constant.Enumeration;

namespace GestionComercial.Applications.Notifications
{
    public interface IProvidersNotifier
    {
        Task NotifyAsync(int providerId, string businessName, ChangeType action);
    }
}
