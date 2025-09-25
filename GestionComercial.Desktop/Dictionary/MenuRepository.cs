using static GestionComercial.Domain.Constant.Enumeration;

namespace GestionComercial.Desktop.Dictionary
{
    public static class MenuRepository
    {
        public static List<MenuDefinition> Definitions = new()
    {
        new MenuDefinition
        {
            Title = "Clientes",
            Icon = "/Images/Clients 32.png",
            Module = ModuleType.Clients,
            Children =
            {
                new() { Title = "Clientes", Icon = "/Images/Clients 32.png", Tag = "Clients", PermissionKey = "Clientes-Lectura" }
            }
        },
        new MenuDefinition
        {
            Title = "Proveedores",
            Icon = "/Images/Provider 32.png",
            Module = ModuleType.Providers,
            Children =
            {
                new() { Title = "Proveedores", Icon = "/Images/Provider 32.png", Tag = "Providers", PermissionKey = "Proveedores-Lectura" }
            }
        },
        new MenuDefinition
        {
            Title = "Stock",
            Icon = "/Images/Products 32.png",
            Module = ModuleType.Articles,
            Children =
            {
                new() { Title = "Articulos", Icon = "/Images/Products 32.png", Tag = "Stock", PermissionKey = "Articulos-Lectura" },
                new MenuDefinition
                {
                    Title = "Reportes",
                    Icon = "/Images/Report 20.png",
                    PermissionKey = "Stock.Report",
                    Children =
                    {
                        new() { Title = "Reporte Stock", Icon = "/Images/Report 32.png", Tag = "Stock_Report", PermissionKey = "Reportes-Lectura" }
                    }
                }
            }
        },
        new MenuDefinition
        {
            Title = "Ventas",
            Icon = "/Images/Sales 32.png",
            Module = ModuleType.Sales,
            Children =
            {
                new() { Title = "Nueva Venta", Icon = "/Images/Sales 32.png", Tag = "NewSale", PermissionKey = "Ventas-Agregar" },
                new() { Title = "Lista de ventas", Icon = "/Images/Sale List 32.png", Tag = "ListSales", PermissionKey = "Ventas-Lectura" },
                new MenuDefinition
                {
                    Title = "Reportes",
                    Icon = "/Images/Report 20.png",
                    PermissionKey = "Sales.Report",
                    Children =
                    {
                        new() { Title = "Reporte Ventas", Icon = "/Images/Report 32.png", Tag = "Sale_Report", PermissionKey = "Reportes-Lectura" }
                    }
                }
            }
        },
        new MenuDefinition
        {
            Title = "Cajas y Bancos",
            Icon = "/Images/Bank 32.png",
            Module = ModuleType.Banks,
            Children =
            {
                new() { Title = "Listado de Cajas Bancos", Icon = "/Images/Bank 32.png", Tag = "Banks", PermissionKey = "Bancos-Lectura" },
                new() { Title = "Parametros de acreditación", Icon = "/Images/Bank 32.png", Tag = "Banks_Parameter", PermissionKey = "Bancos-Lectura" }
            }
        },
        new MenuDefinition
        {
            Title = "Cuentas Contables",
            Icon = "/Images/Acounting Book 32.png",
            Module = ModuleType.Accountings,
            Children =
            {
                new() { Title = "Tipos de cuenta", Icon = "/Images/Acounting Book 32.png", Tag = "AccountTypes", PermissionKey = "Cuentas Contables-Lectura" },
                new() { Title = "Cuentas", Icon = "/Images/Acounting Book 32.png", Tag = "Accounts", PermissionKey = "Cuentas Contables-Lectura" }
            }
        },
        new MenuDefinition
        {
            Title = "Configuraciones",
            Icon = "/Images/Config 32.png",
            Module = ModuleType.Settings,
            Children =
            {
                new MenuDefinition
                {
                    Title = "Maestros",
                    Icon = "/Images/Control Panel 32.png",
                    Children =
                    {
                        new MenuDefinition
                        {
                            Title="Stock",
                            Icon = "/Images/Products 32.png",
                            Children =
                            {
                                new() { Title = "Listas de Precios", Icon = "/Images/Price List 32.png", Tag = "PriceLists", PermissionKey = "Listas de Precios-Lectura" },
                                new() { Title = "Rubros", Icon = "/Images/Product Category 32.png", Tag = "Categories", PermissionKey = "Listas de Precios-Lectura" }
                            }
                        },
                        new MenuDefinition
                        {
                            Title = "Seguridad",
                            Icon = "/Images/Security 32.png",
                            Children =
                            {
                                new() { Title = "Usuarios", Icon = "/Images/Users 32.png", Tag = "Users", PermissionKey = "Usuarios-Lectura" },
                                new() { Title = "Permisos", Icon = "/Images/Security 32.png", Tag = "Permissions", PermissionKey = "Permisos-Lectura" }
                            }
                        },
                        new MenuDefinition
                        {
                            Title = "Empresa",
                            Icon = "/Images/Bienes.png",
                            Children =
                            {
                                new() { Title = "Datos de la empresa", Icon = "/Images/Data Commerce 32.png", Tag = "CommerceData", PermissionKey = "Configuraciones-Lectura" },
                                new() { Title = "Datos Fiscales", Icon = "/Images/Billing Data Commerce 32.png", Tag = "Billing", PermissionKey = "Configuraciones-Lectura" }
                            }
                        }
                    }
                },
                new MenuDefinition
                {
                    Title = "Parámetros",
                    Icon = "/Images/Setting Config 32.png",
                    Children =
                    {
                        new() { Title = "Parámetros Generales", Icon ="/Images/Setting 32.png", Tag = "GeneralParameter_Setup", PermissionKey = "Parametros-Lectura" },
                        new() { Title = "Parámetros de PC", Icon ="/Images/Setting 32.png", Tag = "PcParameter_Setup", PermissionKey = "Parametros-Lectura" }
                    }
                }
            }
        },
        // Fijos
        new MenuDefinition { Title = "Cerrar Sesión", Icon = "/Images/Block 32.png", Tag = "LogOut" },
        new MenuDefinition { Title = "Salir", Icon = "/Images/Exit 32.png", Tag = "Close" }
    };
    }

}
