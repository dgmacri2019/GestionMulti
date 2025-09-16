using GestionComercial.Desktop.Controls.Accounts;
using GestionComercial.Desktop.Controls.Articles;
using GestionComercial.Desktop.Controls.Banks;
using GestionComercial.Desktop.Controls.Clients;
using GestionComercial.Desktop.Controls.Maters.Configurations.Maestros.Stock;
using GestionComercial.Desktop.Controls.Maters.Configurations.Parameters.PcParameters;
using GestionComercial.Desktop.Controls.Permissions;
using GestionComercial.Desktop.Controls.Providers;
using GestionComercial.Desktop.Controls.Sales;
using GestionComercial.Desktop.Controls.Users;
using GestionComercial.Desktop.Helpers;
using GestionComercial.Desktop.ViewModels;
using GestionComercial.Desktop.ViewModels.Client;
using GestionComercial.Desktop.ViewModels.Master;
using GestionComercial.Desktop.ViewModels.Parameter;
using GestionComercial.Desktop.ViewModels.Sale;
using GestionComercial.Desktop.ViewModels.Stock;
using GestionComercial.Desktop.Views.Masters;
using GestionComercial.Domain.Cache;
using GestionComercial.Domain.DTOs.Menu;
using System.Threading.Tasks;
using System.Windows;

namespace GestionComercial.Desktop.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
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
            return
                [
                new()
                {
                    Title = "Clientes",
                    Icon = "/Images/Clients 32.png",
                    Children =
                    [
                        new() { Title = "Clientes ", Icon = "/Images/Clients 32.png", Tag = "Clients" },
                    ]
                },
                new MenuItemModel
                {
                    Title = "Proveedores",
                    Icon = "/Images/Provider 32.png",
                    Children =
                    [
                        new() { Title = "Proveedores ", Icon = "/Images/Provider 32.png", Tag = "Providers" },
                    ]
                },
                new MenuItemModel
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
                },
                new MenuItemModel
                {
                    Title = "Ventas",
                    Icon = "/Images/Sales 32.png",
                    Children =
                    [
                        new() { Title = "Nueva Venta", Icon = "/Images/Sales 32.png", Tag = "Sales" },
                        new() {
                            Title = "Reportes",
                            Icon = "/Images/Report 20.png",
                            Children =
                            [
                                new() { Title = "Reporte Ventas", Icon = "/Images/Report 32.png", Tag = "Sale_Report" },
                            ]
                        },

                    ]
                },
                new MenuItemModel
                {
                    Title = "Cajas y Bancos",
                    Icon = "/Images/Bank 32.png",
                    Children =
                    [
                        new() { Title = "Listado de Cajas Bancos", Icon = "/Images/Bank 32.png", Tag = "Banks" },
                        new() { Title = "Parametros de acreditación", Icon = "/Images/Bank 32.png", Tag = "Banks_Parameter" },
                    ]
                },
                new MenuItemModel
                {
                    Title = "Cuentas Contables",
                    Icon = "/Images/Acounting Book 32.png",
                    Children =
                    [
                        new() { Title = "Tipos de cuenta", Icon = "/Images/Acounting Book 32.png", Tag = "AccountTypes" },
                        new() { Title = "Cuentas", Icon = "/Images/Acounting Book 32.png", Tag = "Accounts" },
                    ]
                },
                new MenuItemModel
                {
                    Title = "Configuraciones",
                    Icon = "/Images/Config 32.png",
                    Children =
                    [
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
                },
                new MenuItemModel
                {
                    Title = "Cerrar Sesión",
                    Icon = "/Images/Block 32.png",
                    Tag = "LogOut",
                    //Children =
                    //[
                    //    new() { Title = "Cerrar Sesión ", Icon = "/Images/Block 32.png", Tag = "LogOut" },
                    //]
                },
            ];
        }

        private void NavigationTree_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (e.NewValue is MenuItemModel selected && !string.IsNullOrEmpty(selected.Tag))
            {
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
                        MainContent.Content = new ListPermissionsControlView();
                        break;
                    case "Sales":
                        MainContent.Content = new ListSaleControlView();
                        break;
                    case "Sales_Report":
                        break;
                    case "GeneralParameter_Setup":
                        break;
                    case "PcParameter_Setup":
                        MainContent.Content = new PcParametersControlView();
                        break;
                    case "Categories":
                        MainContent.Content = new ListCategoryControlView();
                        break;
                    case "CommerceData":
                        var window = new CommerceDataWindow() { Owner = Window.GetWindow(this) };
                        window.ShowDialog();
                        break;
                    case "Billing":
                        MainContent.Content = new ListCategoryControlView();
                        break;
                    case "LogOut":
                        LogOut();
                        break;
                }
            }
        }

        private void LogOut()
        {
            // Limpiar sesión
            App.AuthToken = string.Empty;
            App.UserName = string.Empty;
            App.UserRole = string.Empty;
            App.Password = string.Empty;
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
            GlobalProgressHelper.ReportIndeterminate("Cargando Lista de precios");
            PriceListListViewModel priceListViewModel = new();
            while (!PriceListCache.Instance.HasData)
                await Task.Delay(10);
            GlobalProgressHelper.ReportIndeterminate("Cargando clase maestra");
            MasterClassListViewModel masterClassListViewModel = new();
            while (!MasterCache.Instance.HasData)
                await Task.Delay(10);
            GlobalProgressHelper.ReportIndeterminate("Cargando Rubros");
            CategoryListViewModel categoryListViewModel = new();
            while (!CategoryCache.Instance.HasData)
                await Task.Delay(10);
            GlobalProgressHelper.ReportIndeterminate("Cargando Clientes");
            ClientListViewModel clientListViewModel = new();
            while (!ClientCache.Instance.HasData)
                await Task.Delay(10);
            GlobalProgressHelper.ReportIndeterminate("Cargando Articulos");
            ArticleListViewModel articleListViewModel = new();
            while (!ArticleCache.Instance.HasData)
                await Task.Delay(10);
            GlobalProgressHelper.ReportIndeterminate("Cargando Parametros");
            ParameterListViewModel parameterListViewModel = new();
            while (!ParameterCache.Instance.HasDataPCParameters || !ParameterCache.Instance.HasDataGeneralParameters)
                await Task.Delay(10);
            GlobalProgressHelper.ReportIndeterminate("Cargando Ventas");
            SaleListViewModel saleListViewModel = new();
            while (!ClientCache.Instance.HasData)
                await Task.Delay(10);
            await GlobalProgressHelper.CompleteAsync();
        }
    }
}