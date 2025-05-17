namespace GestionComercial.Domain.DTOs.Menu
{
    public class MenuItemModel
    {
        public string Title { get; set; }
        public string Icon { get; set; } // Ruta al ícono
        public string Tag { get; set; }  // Para identificar qué hacer al hacer clic
        public List<MenuItemModel> Children { get; set; } = new ();
    }
}
