using GestionComercial.Domain.DTOs.Security;

namespace GestionComercial.Domain.Response
{
    public class PermissionResponse : GeneralResponse
    {
        public List<PermissionViewModel> PermissionViewModels { get; set; }
    }
}
