using static GestionComercial.Domain.Constant.Enumeration;

namespace GestionComercial.Applications.Notifications
{
    public interface IBoxAndBanksNotifier
    {
        Task NotifyAsync(int Id, string businessName, ChangeType action);
    }
}
