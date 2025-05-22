namespace GestionComercial.API.Mappers
{
    public static class PermissionNameMapper
    {
        public static string MapController(string controller)
        {
            return controller switch
            {
                "Articles" => "Articulos",
                "Clients" => "Clientes",
                "Users" => "Usuarios",
                "Providers" => "Proveedores",
                "Permissions" => "Permisos",
                "PriceLists" => "Lista de Precios",
                "Banks" => "Bancos",
                "Accounts" => "Cuentas Contables",
                _ => controller
            };
        }

        public static string MapAction(string action)
        {
            if (string.IsNullOrWhiteSpace(action))
                return action;

            action = action.ToLowerInvariant();

            if (action.Contains("add"))
                return "Agregar";

            if (action.Contains("edit") || action.Contains("update"))
                return "Editar";

            if (action.Contains("delete"))
                return "Eliminar";

            if (action.Contains("get") || action.Contains("find") || action.Contains("search"))
                return "Lectura";

            return action;
        }
    }
}