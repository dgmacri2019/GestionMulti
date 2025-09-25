using GestionComercial.Desktop.Services;
using GestionComercial.Desktop.ViewModels.Master.Security;
using GestionComercial.Domain.Cache;
using GestionComercial.Domain.Entities.Masters.Security;
using GestionComercial.Domain.Response;
using System.Windows;
using System.Windows.Input;

namespace GestionComercial.Desktop.Views.Masters
{
    /// <summary>
    /// Lógica de interacción para UserPermissionsWindow.xaml
    /// </summary>
    public partial class UserPermissionsWindow : Window
    {
        private readonly PermissionsApiService _apiService;
        private readonly PermissionListViewModel _viewModel;

        public UserPermissionsWindow()
        {
            InitializeComponent();
            _apiService = new PermissionsApiService();
            _viewModel = new PermissionListViewModel();
            DataContext = _viewModel;
        }

        private async void ListBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            lblConfirm.Text = string.Empty;
            lblError.Text = string.Empty;
            if (_viewModel.SelectedUser != null)
            {
                await _viewModel.LoadPermissionsForUserAsync(_viewModel.SelectedUser.Id);
            }
        }

        private async void BtnUpdate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                lblError.Text = string.Empty;
                lblConfirm.Text = string.Empty;
                if (_viewModel.SelectedUser == null)
                    return;
                btnUpdate.IsEnabled = false;
                List<UserPermission> userPermissions = new();
                PermissionResponse permissionResponse = await _apiService.GetAllPermissionsAsync();
                if (!permissionResponse.Success)
                {
                    btnUpdate.IsEnabled = true;
                    msgError(permissionResponse.Message);
                    return;
                }

                List<Permission> permissions = permissionResponse.Permissions;
                foreach (var module in _viewModel.PermissionGroups)
                {
                    Permission permisoLectura = permissions.First(p => p.ModuleType == module.Module && p.Name.EndsWith("Lectura"));
                    Permission permisoEscritura = permissions.First(p => p.ModuleType == module.Module && p.Name.EndsWith("Agregar"));
                    Permission permisoEditar = permissions.First(p => p.ModuleType == module.Module && p.Name.EndsWith("Editar"));
                    Permission permisoBorrar = permissions.First(p => p.ModuleType == module.Module && p.Name.EndsWith("Borrar"));


                    // Lectura
                    UserPermission userPermissionLectura = permisoLectura.UserPermissions
                        .First(up => up.PermissionId == permisoLectura.Id && up.UserId == _viewModel.SelectedUser.Id);
                    userPermissionLectura.UpdateDate = DateTime.Now;
                    userPermissionLectura.UpdateUser = LoginUserCache.UserName;
                    userPermissionLectura.IsEnabled = module.CanRead;
                    userPermissions.Add(userPermissionLectura);

                    // Agregar
                    UserPermission userPermissionEscritura = permisoEscritura.UserPermissions
                         .First(up => up.PermissionId == permisoEscritura.Id && up.UserId == _viewModel.SelectedUser.Id);
                    userPermissionEscritura.UpdateDate = DateTime.Now;
                    userPermissionEscritura.UpdateUser = LoginUserCache.UserName;
                    userPermissionEscritura.IsEnabled = module.CanAdd;
                    userPermissions.Add(userPermissionEscritura);


                    // Editar
                    UserPermission userPermissionEditar = permisoEditar.UserPermissions
                        .First(up => up.PermissionId == permisoEditar.Id && up.UserId == _viewModel.SelectedUser.Id);
                    userPermissionEditar.UpdateDate = DateTime.Now;
                    userPermissionEditar.UpdateUser = LoginUserCache.UserName;
                    userPermissionEditar.IsEnabled = module.CanEdit;
                    userPermissions.Add(userPermissionEditar);


                    // Borrar
                    UserPermission userPermissionBorrar = permisoBorrar.UserPermissions
                      .First(up => up.PermissionId == permisoBorrar.Id && up.UserId == _viewModel.SelectedUser.Id);
                    userPermissionBorrar.UpdateDate = DateTime.Now;
                    userPermissionBorrar.UpdateUser = LoginUserCache.UserName;
                    userPermissionBorrar.IsEnabled = module.CanDelete;
                    userPermissions.Add(userPermissionBorrar);
                }

                // Llamo al servicio/repositorio que guarda en la base
                PermissionResponse resultUpdate = await _apiService.UpdateUserPermissionsAsync(userPermissions);
                if (!resultUpdate.Success)
                    msgError(resultUpdate.Message);
                else
                    msgConfirm(resultUpdate.Message);

                btnUpdate.IsEnabled = true;
            }
            catch (Exception ex)
            {
                msgError(ex.Message);
                btnUpdate.IsEnabled = true;
            }
        }



        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            Close(); // para notificar a la vista principal
        }


        private void msgError(string msg)
        {
            lblError.Text = msg;
        }

        private void msgConfirm(string message)
        {
            lblConfirm.Text = message;
        }
    }
}
