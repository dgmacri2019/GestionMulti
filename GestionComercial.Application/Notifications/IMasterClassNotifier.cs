using static GestionComercial.Domain.Constant.Enumeration;

namespace GestionComercial.Applications.Notifications
{
    public interface IMasterClassNotifier
    {
        Task NotifyAsync(int Id, string description, ChangeType action);
    }
}
