using static GestionComercial.Domain.Constant.Enumeration;

namespace GestionComercial.Desktop.Dictionary
{
    public class MenuDefinition
    {
        public string Title { get; set; }
        public string Icon { get; set; }
        public string Tag { get; set; }
        public string PermissionKey { get; set; }  // permiso específico
        public ModuleType? Module { get; set; }    // módulo requerido
        public List<MenuDefinition> Children { get; set; } = new();
    }
}
