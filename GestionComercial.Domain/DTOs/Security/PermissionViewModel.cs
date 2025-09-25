using static GestionComercial.Domain.Constant.Enumeration;

namespace GestionComercial.Domain.DTOs.Security
{
    public class PermissionViewModel
    {
        public ModuleType Module { get; set; }
        public string ModuleName => Module.ToString();

        public bool CanRead { get; set; }
        public bool CanAdd { get; set; }
        public bool CanEdit { get; set; }
        public bool CanDelete { get; set; }
    }

}
