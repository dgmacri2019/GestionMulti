using GestionComercial.Desktop.Controls.Accounts;
using GestionComercial.Desktop.Controls.Articles;
using GestionComercial.Desktop.Controls.Banks;
using GestionComercial.Desktop.Controls.Clients;
using GestionComercial.Desktop.Controls.Maters.Configurations.Maestros.Stock;
using GestionComercial.Desktop.Controls.Maters.Configurations.Parameters.PcParameters;
using GestionComercial.Desktop.Controls.Providers;
using GestionComercial.Desktop.Controls.Sales;
using GestionComercial.Desktop.Controls.Users;
using GestionComercial.Desktop.Dictionary;
using GestionComercial.Desktop.Helpers;
using GestionComercial.Desktop.ViewModels;
using GestionComercial.Desktop.ViewModels.Client;
using GestionComercial.Desktop.ViewModels.Master;
using GestionComercial.Desktop.ViewModels.Parameter;
using GestionComercial.Desktop.ViewModels.Sale;
using GestionComercial.Desktop.ViewModels.Stock;
using GestionComercial.Desktop.ViewModels.Users;
using GestionComercial.Desktop.Views.Masters;
using GestionComercial.Desktop.Views.Sales;
using GestionComercial.Domain.Cache;
using GestionComercial.Domain.DTOs.Menu;
using GestionComercial.Domain.Entities.Masters.Security;
using System.Windows;
using System.Windows.Controls;
using static GestionComercial.Domain.Constant.Enumeration;

namespace GestionComercial.Desktop.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Window _currentWindow = null;
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainViewModel();
            GlobalProgressHelper.Initialize(StatusProgressBar, StatusText, Dispatcher);
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            NavigationTree.ItemsSource = CreateMenuItem();
            await Task.Run(async () => await CargarCacheAsync());
        }


        private List<MenuItemModel> CreateMenuItem()
        {
            List<MenuItemModel> menuItems = new();

            foreach (var def in MenuRepository.Definitions)
            {
                var item = BuildMenuItem(def);
                if (item != null)
                    menuItems.Add(item);
            }

            return menuItems;
        }

        private MenuItemModel? BuildMenuItem(MenuDefinition def)
        {
            // Validar módulo
            if (def.Module.HasValue && !AutorizeOperationHelper.ValidateModule(def.Module.Value))
                return null;

            // Validar permiso
            if (!string.IsNullOrEmpty(def.PermissionKey) && def.Module.HasValue
                && !AutorizeOperationHelper.ValidateOperation(def.Module.Value, def.PermissionKey))
                return null;

            // Hijos
            var children = def.Children
                .Select(BuildMenuItem)
                .Where(x => x != null)
                .ToList();

            // Si no tiene hijos válidos y tampoco Tag => lo descarto
            if (children.Count == 0 && string.IsNullOrEmpty(def.Tag))
                return null;

            return new MenuItemModel
            {
                Title = def.Title,
                Icon = def.Icon,
                Tag = def.Tag,
                Children = children
            };
        }
        private void NavigationTree_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (!(e.NewValue is MenuItemModel selected) || string.IsNullOrEmpty(selected.Tag))
                return;

            // 1) Cerrar ventana abierta, si hay
            _currentWindow?.Close();
            _currentWindow = null;

            // 2) Limpiar UserControl actual
            MainContent.Content = null;

            // 3) Abrir nuevo contenido según Tag
            switch (selected.Tag)
            {
                case "Banks":
                    MainContent.Content = new ListBankControlView();
                    break;
                case "Banks_Parameter":
                    MainContent.Content = new ListBankParameterControlView();
                    break;
                case "Stock":
                    MainContent.Content = new ListAticleControlView();
                    break;
                case "Clients":
                    MainContent.Content = new ListClientControlView();
                    break;
                case "Providers":
                    MainContent.Content = new ListProviderControlView();
                    break;
                case "PriceLists":
                    MainContent.Content = new ListPriceListControlView();
                    break;
                case "Accounts":
                    MainContent.Content = new ListAccountControlView();
                    break;
                case "Users":
                    MainContent.Content = new ListUsersControlView();
                    break;
                case "Permissions":
                    _currentWindow = new UserPermissionsWindow() { Owner = this };
                    _currentWindow.Show();
                    break;
                case "NewSale":
                    _currentWindow = new SaleAddWindow(0) { Owner = this };
                    _currentWindow.Show();
                    break;
                case "ListSales":
                    MainContent.Content = new ListSaleControlView();
                    break;
                case "PcParameter_Setup":
                    MainContent.Content = new PcParametersControlView();
                    break;
                case "Categories":
                    MainContent.Content = new ListCategoryControlView();
                    break;
                case "CommerceData":
                    _currentWindow = new CommerceDataWindow() { Owner = this };
                    _currentWindow.ShowDialog();
                    _currentWindow = null; // como es ShowDialog(), se cierra solo al terminar
                    break;
                case "Billing":
                    _currentWindow = new BillingWindow() { Owner = this };
                    _currentWindow.ShowDialog();
                    _currentWindow = null;
                    break;
                case "LogOut":
                    LogOut();
                    break;
                case "Close":
                    if (MessageBox.Show("Confirma que desea cerrar el programa", "Aviso al operador", MessageBoxButton.YesNo, MessageBoxImage.Information) == MessageBoxResult.Yes)
                        Close();
                    break;
            }
        }

        private void LogOut()
        {
            // Limpiar sesión
            LoginUserCache.AuthToken = string.Empty;
            LoginUserCache.UserName = string.Empty;
            LoginUserCache.UserRole = string.Empty;
            LoginUserCache.Password = string.Empty;
            LoginUserCache.UserId = string.Empty;
            LoginUserCache.Permisions.Clear();
            CacheManager.ClearAll();
            LoginWindow loginView = new LoginWindow();
            loginView.Show();
            // Importante: cerrar la ventana actual correctamente
                    
            foreach (Window window in Application.Current.Windows)
            {
                if (window is MainWindow)
                {
                    window.Close();
                    break;
                }
            }

            // Establecer login como nueva MainWindow si querés seguir con el flujo
            Application.Current.MainWindow = loginView;
        }

        private async Task CargarCacheAsync()
        {
            if (AutorizeOperationHelper.ValidateModule(ModuleType.Users))
            {
                GlobalProgressHelper.ReportIndeterminate("Cargando Lista de usuarios");
                UserListViewModel userViewModel = new();
                while (!UserCache.Instance.HasData)
                    await Task.Delay(10);
            }
            if (AutorizeOperationHelper.ValidateModule(ModuleType.PriceLists))
            {
                GlobalProgressHelper.ReportIndeterminate("Cargando Lista de precios");
                PriceListListViewModel priceListViewModel = new();
                while (!PriceListCache.Instance.HasData)
                    await Task.Delay(10);
            }
            if (AutorizeOperationHelper.ValidateModule(ModuleType.Parameters))
            {
                GlobalProgressHelper.ReportIndeterminate("Cargando clase maestra");
                MasterClassListViewModel masterClassListViewModel = new();
                while (!MasterCache.Instance.HasData)
                    await Task.Delay(10);

                GlobalProgressHelper.ReportIndeterminate("Cargando Rubros");
                CategoryListViewModel categoryListViewModel = new();
                while (!CategoryCache.Instance.HasData)
                    await Task.Delay(10);

                GlobalProgressHelper.ReportIndeterminate("Cargando Parametros");
                ParameterListViewModel parameterListViewModel = new();
                while (!ParameterCache.Instance.HasDataPCParameters || !ParameterCache.Instance.HasDataGeneralParameters)
                    await Task.Delay(10);
            }
            if (AutorizeOperationHelper.ValidateModule(ModuleType.Clients))
            {
                GlobalProgressHelper.ReportIndeterminate("Cargando Clientes");
                ClientListViewModel clientListViewModel = new();
                while (!ClientCache.Instance.HasData && !ClientCache.ReadingOk)
                    await Task.Delay(10);
            }
            if (AutorizeOperationHelper.ValidateModule(ModuleType.Articles))
            {
                GlobalProgressHelper.ReportIndeterminate("Cargando Articulos");
                ArticleListViewModel articleListViewModel = new();
                while (!ArticleCache.Instance.HasData && !ArticleCache.ReadingOk)
                    await Task.Delay(10);
            }
            if (AutorizeOperationHelper.ValidateModule(ModuleType.Sales))
            {
                GlobalProgressHelper.ReportIndeterminate("Cargando Ventas");
                SaleListViewModel saleListViewModel = new();
                while (!SaleCache.Instance.HasData && !SaleCache.ReadingOk)
                    await Task.Delay(10);
                await GlobalProgressHelper.CompleteAsync();
            }
        }






        /*
         * 
           private List<MenuItemModel> CreateMenuItem()
        {

            List<MenuItemModel> menuItems = [];

            if (AutorizeOperationHelper.ValidateModule(ModuleType.Clients))
                menuItems.Add(new MenuItemModel
                {
                    Title = "Clientes",
                    Icon = "/Images/Clients 32.png",
                    Children =
                    [
                        new() { Title = "Clientes ", Icon = "/Images/Clients 32.png", Tag = "Clients" },
                    ]
                });
            if (AutorizeOperationHelper.ValidateModule(ModuleType.Providers))
                menuItems.Add(new MenuItemModel
                {
                    Title = "Proveedores",
                    Icon = "/Images/Provider 32.png",
                    Children =
                    [
                        new() { Title = "Proveedores ", Icon = "/Images/Provider 32.png", Tag = "Providers" },
                    ]
                });
            if (AutorizeOperationHelper.ValidateModule(ModuleType.Articles))
                menuItems.Add(new MenuItemModel
                {
                    Title = "Stock",
                    Icon = "/Images/Products 32.png",
                    Children =
                    [
                        new() { Title = "Articulos", Icon = "/Images/Products 32.png", Tag = "Stock" },
                        new() {
                            Title = "Reportes",
                            Icon = "/Images/Report 20.png",
                            Children =
                            [
                                new() { Title = "Reporte Stock", Icon = "/Images/Report 32.png", Tag = "Stock_Report" },
                            ]
                        },

                    ]
                });
            if (AutorizeOperationHelper.ValidateModule(ModuleType.Sales))
                menuItems.Add(new MenuItemModel
                {
                    Title = "Ventas",
                    Icon = "/Images/Sales 32.png",
                    Children =
                    [
                        //TODO: Ver permisos subItems
                        new() { Title = "Nueva Venta", Icon = "/Images/Sales 32.png", Tag = "NewSale" },
                        new() { Title = "Lista de ventas", Icon = "/Images/Sale List 32.png", Tag = "ListSales" },
                        new() {
                            Title = "Reportes",
                            Icon = "/Images/Report 20.png",
                            Children =
                            [
                                new() { Title = "Reporte Ventas", Icon = "/Images/Report 32.png", Tag = "Sale_Report" },
                            ]
                        },
                    ]
                });
            if (AutorizeOperationHelper.ValidateModule(ModuleType.Banks))
                menuItems.Add(new MenuItemModel
                {
                    Title = "Cajas y Bancos",
                    Icon = "/Images/Bank 32.png",
                    Children =
                    [
                        new() { Title = "Listado de Cajas Bancos", Icon = "/Images/Bank 32.png", Tag = "Banks" },
                        new() { Title = "Parametros de acreditación", Icon = "/Images/Bank 32.png", Tag = "Banks_Parameter" },
                    ]
                });
            if (AutorizeOperationHelper.ValidateModule(ModuleType.Accountings))
                menuItems.Add(new MenuItemModel
                {
                    Title = "Cuentas Contables",
                    Icon = "/Images/Acounting Book 32.png",
                    Children =
                    [
                        new() { Title = "Tipos de cuenta", Icon = "/Images/Acounting Book 32.png", Tag = "AccountTypes" },
                        new() { Title = "Cuentas", Icon = "/Images/Acounting Book 32.png", Tag = "Accounts" },
                    ]
                });
            if (AutorizeOperationHelper.ValidateModule(ModuleType.Settings))
                menuItems.Add(new MenuItemModel
                {
                    Title = "Configuraciones",
                    Icon = "/Images/Config 32.png",
                    Children =
                    [
                        //TODO: Ver permisos subItems
                        new() {
                            Title = "Maestros",
                            Icon = "/Images/Control Panel 32.png",
                            Children =
                            [
                                new ()
                                {
                                    Title="Stock",
                                    Icon = "/Images/Products 32.png",
                                    Children =
                                    [
                                        new() { Title = "Listas de Precios", Icon = "/Images/Price List 32.png", Tag = "PriceLists" },
                                        new() { Title = "Rubros", Icon = "/Images/Product Category 32.png", Tag = "Categories" },
                                    ]
                                },
                                new()
                                {
                                    Title = "Seguridad",
                                    Icon = "/Images/Security 32.png",
                                    Children =
                                    [
                                        new() { Title = "Usuarios", Icon = "/Images/Users 32.png", Tag = "Users" },
                                        new() { Title = "Permisos", Icon = "/Images/Security 32.png", Tag = "Permissions" },
                                    ]
                                },

                                new()
                                {
                                    Title = "Empresa",
                                    Icon = "/Images/Bienes.png",
                                    Children =
                                    [
                                        new() { Title = "Datos de la empresa", Icon = "/Images/Data Commerce 32.png", Tag = "CommerceData" },
                                        new() { Title = "Datos Fiscales", Icon = "/Images/Billing Data Commerce 32.png", Tag = "Billing" },
                                    ]
                                },

                            ]
                        },
                        new() {
                            Title = "Parámetros",
                            Icon = "/Images/Setting Config 32.png",
                            Children =
                            [
                                new() {
                                    Title = "Parámetros Generales",
                                    Icon ="/Images/Setting 32.png",
                                    Tag = "GeneralParameter_Setup",
                                },
                                new() {
                                    Title = "Parámetros de PC",
                                    Icon ="/Images/Setting 32.png",
                                    Tag = "PcParameter_Setup",
                                },


                            ]
                        },


                    ]
                });

            menuItems.Add(new MenuItemModel
            {
                Title = "Cerrar Sesión",
                Icon = "/Images/Block 32.png",
                Tag = "LogOut",
            });
            menuItems.Add(new MenuItemModel
            {
                Title = "Salir",
                Icon = "/Images/Exit 32.png",
                Tag = "Close",
            });

            return menuItems;
        }



        */
    }
}