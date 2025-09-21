using GestionComercial.Desktop.Controls.Providers;
using GestionComercial.Desktop.ViewModels.Provider;
using GestionComercial.Desktop.ViewModels.Users;
using GestionComercial.Domain.DTOs.Provider;
using GestionComercial.Domain.DTOs.User;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;

namespace GestionComercial.Desktop.Controls.Users
{
    /// <summary>
    /// Lógica de interacción para ListUsersControlView.xaml
    /// </summary>
    public partial class ListUsersControlView : UserControl
    {
        public ListUsersControlView()
        {
            InitializeComponent();
            DataContext = new UserListViewModel();
        }


        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            DgUsers.Visibility = Visibility.Hidden;
            PanelSearch.Visibility = Visibility.Hidden;
            PanelEdicion.Visibility = Visibility.Visible;
            btnAdd.Visibility = Visibility.Hidden;
            btnEnables.Visibility = Visibility.Hidden;
            lblHeader.Content = "Nuevo Usuario";
            var ventana = new EditUserControlView(string.Empty);
            ventana.UsuarioActualizado += () =>
            {
                DgUsers.Visibility = Visibility.Visible;
                PanelSearch.Visibility = Visibility.Visible;
                PanelEdicion.Content = null;
                PanelEdicion.Visibility = Visibility.Hidden;
                btnAdd.Visibility = Visibility.Visible;
                btnEnables.Visibility = Visibility.Visible;
                lblHeader.Content = "Usuarios";
            };

            PanelEdicion.Content = ventana;
        }

        private void DataGrid_Loaded(object sender, RoutedEventArgs e)
        {

        }


        private void DataGrid_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (DgUsers.SelectedItem is UserViewModel user)
            {
                DgUsers.Visibility = Visibility.Hidden;
                PanelSearch.Visibility = Visibility.Hidden;
                PanelEdicion.Visibility = Visibility.Visible;
                btnEnables.Visibility = Visibility.Hidden;
                btnAdd.Visibility = Visibility.Hidden;
                lblHeader.Content = "Editar Usuario";
                var ventana = new EditUserControlView(user.Id);
                ventana.UsuarioActualizado += () =>
                {
                    DgUsers.Visibility = Visibility.Visible;
                    PanelSearch.Visibility = Visibility.Visible;
                    PanelEdicion.Content = null;
                    PanelEdicion.Visibility = Visibility.Hidden;
                    btnAdd.Visibility = Visibility.Visible;
                    btnEnables.Visibility = Visibility.Visible;
                    lblHeader.Content = "Usuarios";
                    if (!string.IsNullOrEmpty(SearchBox.Text))
                        SearchBox.Focus();
                };

                PanelEdicion.Content = ventana;

            }
        }

        private void Email_RequestNavigate(object sender, System.Windows.Navigation.RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo($"mailto:{e.Uri}")
            {
                UseShellExecute = true
            });
            e.Handled = true;
        }


    }
}
