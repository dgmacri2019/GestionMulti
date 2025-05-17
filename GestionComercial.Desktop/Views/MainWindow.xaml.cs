using GestionComercial.Desktop.Controls.Articles;
using GestionComercial.Desktop.Controls.Banks;
using GestionComercial.Desktop.Controls.Clients;
using GestionComercial.Desktop.Controls.PriceLists;
using GestionComercial.Desktop.Controls.Providers;
using GestionComercial.Desktop.ViewModels;
using GestionComercial.Domain.DTOs.Menu;
using System.Windows;
using System.Windows.Controls.Primitives;

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
            
            var menu = new List<MenuItemModel>
            {
                new MenuItemModel
                 {
                     Title = "Clientes",
                     Icon = "/Images/Clients 32.png",
                     Children = new List<MenuItemModel>
                     {
                         new MenuItemModel { Title = "Clientes ", Icon = "/Images/Clients 32.png", Tag = "Clients" },
                     }
                 },
                new MenuItemModel
                {
                    Title = "Proveedores",
                    Icon = "/Images/Provider 32.png",
                    Children = new List<MenuItemModel>
                    {
                        new MenuItemModel { Title = "Proveedores ", Icon = "/Images/Provider 32.png", Tag = "Providers" },
                    }
                },
                new MenuItemModel
                {
                    Title = "Stock",
                    Icon = "/Images/Products 32.png",
                    Children = new List<MenuItemModel>
                    {
                        new MenuItemModel { Title = "Articulos", Icon = "/Images/Products 32.png", Tag = "Stock" },
                        new MenuItemModel { Title = "Listadas de Precios", Icon = "/Images/Details.png", Tag = "PriceLists" },
                        new MenuItemModel 
                        { 
                            Title = "Reportes", 
                            Icon = "/Images/Report 20.png",
                            Children = new List<MenuItemModel>
                            {
                                new MenuItemModel { Title = "Reporte Stock", Icon = "/Images/Report 32.png", Tag = "Stock_Report" },
                            }
                        },

                    }
                },
                new MenuItemModel
                {
                    Title = "Cajas y Bancos",
                    Icon = "/Images/Bank 32.png",
                    Children = new List<MenuItemModel>
                    {
                        new MenuItemModel { Title = "Listado de Cajas Bancos", Icon = "/Images/Bank 32.png", Tag = "Banks" },
                        new MenuItemModel { Title = "Parametros de acreditación", Icon = "/Images/Bank 32.png", Tag = "Banks_Parameter" },
                    }
                },

                new MenuItemModel
                {
                    Title = "Cerrar Sesión",
                    Icon = "/Images/Block 32.png",
                    Tag = "LogOut",
                    Children = new List<MenuItemModel>
                    {
                        new MenuItemModel { Title = "Cerrar Sesión ", Icon = "/Images/Block 32.png", Tag = "LogOut" },
                    //    //new MenuItemModel { Title = "Apertura / Cierre", Icon = "/Images/openclose.png", Tag = "CajaApertura" }
                    }
                },
            };
            
            // Podés filtrar los ítems según el usuario logueado


           
            NavigationTree.ItemsSource = menu;
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
                    case "LogOut":
                        LogOut();
                        break;
                        // más opciones según el tag...
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
    }
}