namespace GestionComercial.API.Mappers
{
    public static class PermissionNameMapper
    {
        public static string MapController(string controller)
        {
            return controller switch
            {
                "Clients" => "Clientes",
                "Providers" => "Proveedores",
                "Articles" => "Articulos",
                "Purchases" => "Compras",
                "Sales" => "Ventas",
                "Banks" => "Bancos",
                "Reports" => "Reportes",
                "Parameters" => "Parametros",
                "Settings" => "Configuraciones",
                "Accounts" => "Cuentas Contables",
                "PriceLists" => "Lista de Precios",
                "Users" => "Usuarios",
                "PurchaseOrders" => "Ordenes de Compra",
                "BackUps" => "BackUps",
                "Budgets" => "Presupuestos",
                "Payments" => "Pagos y Cobranzas",
                "DataBases" => "Base de Datos",
                "Permissions" => "Permisos",
                "MasterClass" => "Parametros",
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

            if (action.Contains("get") || action.Contains("find") || action.Contains("search") || action.Contains("notify"))
                return "Lectura";

            return action;
        }
    }
}