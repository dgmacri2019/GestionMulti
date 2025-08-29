using GestionComercial.Desktop.Cache;
using GestionComercial.Desktop.Controls.Accounts;
using GestionComercial.Desktop.Controls.Articles;
using GestionComercial.Desktop.Controls.Banks;
using GestionComercial.Desktop.Controls.Clients;
using GestionComercial.Desktop.Controls.Permissions;
using GestionComercial.Desktop.Controls.PriceLists;
using GestionComercial.Desktop.Controls.Providers;
using GestionComercial.Desktop.Controls.Sales;
using GestionComercial.Desktop.Controls.Users;
using GestionComercial.Desktop.ViewModels;
using GestionComercial.Desktop.ViewModels.Client;
using GestionComercial.Desktop.ViewModels.Stock;
using GestionComercial.Domain.DTOs.Menu;
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
        }







        private void Stock_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new ListAticleControlView();
        }

        private void Client_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new ListClientControlView();
        }

        private void Provider_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new ListProviderControlView();
        }

        private void PriceList_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new ListPriceListControlView();
        }

        private void Bank_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new ListBankControlView();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            NavigationTree.ItemsSource = CreateMenuItem();
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
                        new() { Title = "Listadas de Precios", Icon = "/Images/Details.png", Tag = "PriceLists" },
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
                        //new() { Title = "Listadas de Precios", Icon = "/Images/Details.png", Tag = "PriceLists" },
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
                    Title = "Configuración",
                    Icon = "/Images/Setting Config 32.png",
                    Children =
                    [
                        new() {
                            Title = "Parametros",
                            Icon = "/Images/Setting Config 32.png",
                            Children =
                            [
                                new() {
                                    Title = "Generales",
                                    Icon ="/Images/Setting 32.png",
                                    Children =
                                    [
                                         new() { Title = "Email", Icon = "/Images/Email Setting 32.png", Tag = "Email_Setup" },
                                        new() { Title = "Sistema", Icon = "/Images/GetParameter 32.png", Tag = "System_Setup" },
                                    ]
                                },
                                new() { Title = "Impresión", Icon = "/Images/Printer Setup 32.png", Tag = "Printer_Setup" },

                            ]
                        },
                        new() {
                            Title = "Seguridad",
                            Icon = "/Images/Security 32.png",
                            Children =
                            [
                                new() { Title = "Usuarios", Icon = "/Images/Users 32.png", Tag = "Users" },
                                new() { Title = "Permisos", Icon = "/Images/Security 32.png", Tag = "Permissions" },
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
                        MainContent.Content = new SalesControlView(0);
                        break;
                    case "Sales_Report":
                        break;
                    case "Email_Setup":
                        break;
                    case "System_Setup":
                        break;
                    case "Printer_Setup":
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

        private void StackPanel_Loaded(object sender, RoutedEventArgs e)
        {
            Thread thread = new(CargarCache);
            thread.Start();
        }

        private void CargarCache(object? obj)
        {
            ClientListViewModel clientListViewModel = new();
            ArticleListViewModel articleListViewModel = new();
        }
    }
}