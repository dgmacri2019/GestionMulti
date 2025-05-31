using GestionComercial.Desktop.Services;
using GestionComercial.Domain.DTOs.Accounts;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace GestionComercial.Desktop.Controls.Accounts
{
    /// <summary>
    /// Lógica de interacción para ListAccountControlView.xaml
    /// </summary>
    public partial class ListAccountControlView : UserControl
    {
        private readonly AccountsApiService _accountsApiService;

        public ListAccountControlView()
        {
            _accountsApiService = new AccountsApiService();
            InitializeComponent();
            this.DataContext = new AccountViewModel(); // O lo que uses
        }



        private async Task CargarArbolAsync()
        {
            var cuentas = await _accountsApiService.GetAllAsync(true, false, true);

            foreach (var cuenta in cuentas)
            {
                TreeViewItem item = CrearTreeViewItem(cuenta);
                TreeCuentas.Items.Add(item);
            }
        }

        private TreeViewItem CrearTreeViewItem(AccountViewModel cuenta)
        {
            var item = new TreeViewItem
            {
                Header = new TextBlock
                {
                    Text = cuenta.Name,
                    FontWeight = cuenta.IsFirstLevel ? FontWeights.Bold : FontWeights.Normal
                },
                Tag = cuenta // Guardamos la cuenta para acceder a su descripción
            };

            foreach (var hijo in cuenta.Children)
            {
                item.Items.Add(CrearTreeViewItem(hijo));
            }

            return item;
        }

        private void TreeView_MouseMove(object sender, MouseEventArgs e)
        {
            var elemento = e.OriginalSource as DependencyObject;
            while (elemento != null && !(elemento is TreeViewItem))
                elemento = VisualTreeHelper.GetParent(elemento);

            if (elemento is TreeViewItem item)
            {
                if (item.Tag is AccountViewModel cuenta)
                {
                    DescripcionTextBlock.Text = cuenta.Description;
                }
            }
        }

        private void TreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            // Opcional: mostrar descripción al hacer clic también
            if (e.NewValue is TreeViewItem item && item.Tag is AccountViewModel cuenta)
            {
                DescripcionTextBlock.Text = cuenta.Description;
            }
        }

        private List<AccountViewModel> ObtenerDatosDesdeDB()
        {
            // Acá deberías consultar la base de datos y construir la jerarquía
            // Simulamos datos por ahora
            return new List<AccountViewModel>
            {
                new AccountViewModel
                {
                Name = "ACTIVO",
                Description = "Recursos que posee la empresa.",
                IsFirstLevel = true,
                Children = new List<AccountViewModel>
                {
                    new AccountViewModel
                    {
                        Name = "ACTIVO CORRIENTE",
                        Description = "Bienes líquidos o que se convierten en efectivo fácilmente.",
                        Children = new List<AccountViewModel>
                        {
                            new AccountViewModel
                            {
                                Name = "DISPONIBILIDADES",
                                Description = "Fondos inmediatamente disponibles.",
                                Children = new List<AccountViewModel>
                                {
                                    new AccountViewModel
                                    {
                                        Name = "CAJAS EFECTIVO",
                                        Description = "Caja general de la empresa.",
                                        Children = new List<AccountViewModel>
                                        {
                                            new AccountViewModel
                                            {
                                                Name = "ADMINISTRACION",
                                                Description = "Caja de administración.",
                                                Children = new List<AccountViewModel>
                                                {
                                                    new AccountViewModel { Name = "PESOS", Description = "Caja en pesos." },
                                                    new AccountViewModel { Name = "DOLARES", Description = "Caja en dólares." },
                                                    new AccountViewModel { Name = "EUROS", Description = "Caja en euros." }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                }
            };
        }

        private async void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            await CargarArbolAsync();
        }

        private void TreeCuentas_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            // Obtené el elemento visual más cercano del tipo TreeViewItem
            DependencyObject obj = e.OriginalSource as DependencyObject;

            while (obj != null && !(obj is TreeViewItem))
            {
                obj = VisualTreeHelper.GetParent(obj);
            }

            if (obj is TreeViewItem item)
            {
                // Marcar como manejado para evitar que suba al Main
                e.Handled = true;

                // Obtener la cuenta desde la propiedad Tag
                if (item.Tag is AccountViewModel cuenta)
                {
                    TreeCuentas.Items.Clear();
                    DescripcionTextBlock.Text = "Pasá el mouse por una cuenta para ver su descripción";
                    TreeCuentas.Visibility = Visibility.Hidden;
                    Comentario.Visibility = Visibility.Hidden;
                    TreeCuentas.DataContext = null;
                    PanelEdicion.Visibility = Visibility.Visible;
                    btnAddAccount.Visibility = Visibility.Hidden;
                    lblHeader.Content = "Editar cuenta contable";
                    var ventana = new EditAccountControlView(cuenta.Id);
                    ventana.CuentaActualizada += () =>
                    {
                        CargarArbolAsync();
                        TreeCuentas.Visibility = Visibility.Visible;
                        Comentario.Visibility = Visibility.Visible;
                        PanelEdicion.Content = null;
                        PanelEdicion.Visibility = Visibility.Hidden;
                        btnAddAccount.Visibility = Visibility.Visible;
                        lblHeader.Content = "Cuentas contables";
                    };
                    PanelEdicion.Content = ventana;
                }
            }
        }

        private void btnAddAccount_Click(object sender, RoutedEventArgs e)
        {
            TreeCuentas.Visibility = Visibility.Hidden;
            Comentario.Visibility = Visibility.Hidden;
            TreeCuentas.Items.Clear();
            DescripcionTextBlock.Text = "Pasá el mouse por una cuenta para ver su descripción";
            PanelEdicion.Visibility = Visibility.Visible;
            btnAddAccount.Visibility = Visibility.Hidden;
            lblHeader.Content = "Nueva cuenta contable";
            var ventana = new EditAccountControlView(0);
            ventana.CuentaActualizada += () =>
            {
                CargarArbolAsync();
                TreeCuentas.Visibility = Visibility.Visible;
                Comentario.Visibility = Visibility.Visible;
                PanelEdicion.Content = null;
                PanelEdicion.Visibility = Visibility.Hidden;
                btnAddAccount.Visibility = Visibility.Visible;
                lblHeader.Content = "Cuentas contables";
            };
            PanelEdicion.Content = ventana;
        }
    }
}
