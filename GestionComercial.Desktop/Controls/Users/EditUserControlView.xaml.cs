using GestionComercial.Desktop.Services;
using GestionComercial.Domain.Cache;
using GestionComercial.Domain.DTOs.User;
using GestionComercial.Domain.Entities.Masters;
using GestionComercial.Domain.Helpers;
using GestionComercial.Domain.Response;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace GestionComercial.Desktop.Controls.Users
{
    /// <summary>
    /// Lógica de interacción para EditUserControlView.xaml
    /// </summary>
    public partial class EditUserControlView : UserControl
    {
        private readonly UsersApiService _apiService;
        public event Action UsuarioActualizado;
        private readonly string UserId;

        private UserViewModel UserViewModel { get; set; }


        private List<UserRoleDto> Roles =
                [
                    new UserRoleDto { Id = 0, Name = "Seleccione el Rol" },
                    new UserRoleDto { Id = 1, Name = "Developer" },
                    new UserRoleDto { Id = 2, Name = "Administrator" },
                    new UserRoleDto { Id = 3, Name = "Supervisor" },
                    new UserRoleDto { Id = 4, Name = "Operator" },
                    new UserRoleDto { Id = 5, Name = "Cashier" },
                ];

        public EditUserControlView(string userId = "")
        {
            InitializeComponent();
            UserId = userId;
            _apiService = new UsersApiService();

            if (string.IsNullOrEmpty(userId))
            {
                btnAdd.Visibility = Visibility.Visible;
                btnUpdate.Visibility = Visibility.Hidden;
                UserViewModel = new UserViewModel { IsEnabled = true };

            }
            else
            {
                UserViewModel? viewModel = UserCache.Instance.GetAll().FirstOrDefault(u => u.Id == UserId);
                if (viewModel != null)
                {
                    UserViewModel = viewModel;
                    btnAdd.Visibility = Visibility.Hidden;
                    btnUpdate.Visibility = Visibility.Visible;
                    UserViewModel.RoleId = Roles.FirstOrDefault(r => r.Name == UserViewModel.RoleName).Id;
                    txtUserName.IsEnabled = false;
                }
                else
                    lblError.Text = "No se reconoce el Usuario";
            }
            if (UserViewModel != null)
            {

                UserViewModel.UserRoleDtos.Clear();
                UserViewModel.UserRoleDtos.Add(new UserRoleDto { Id = 0, Name = "Seleccione el Rol" });
                int roleId = Roles.First(r => r.Name == LoginUserCache.UserRole).Id;

                foreach (var rol in Roles.Where(r => r.Id >= roleId))
                {
                    //if (rol.Id >= Roles.FirstOrDefault(r => r.Name == LoginUserCache.UserRole).Id)
                    UserViewModel.UserRoleDtos.Add(rol);
                }
            }
            DataContext = UserViewModel;
        }

        private void miUserControl_Loaded(object sender, RoutedEventArgs e)
        {
            GridGeneral.MaxWidth = this.ActualWidth;
            lblError.MaxWidth = this.ActualWidth;
        }


        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            UserViewModel.UserRoleDtos.AddRange(Roles);

            UsuarioActualizado?.Invoke(); // para notificar a la vista principal
        }

        private async void BtnUpdate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                lblError.Text = string.Empty;

                if (ValidateUser())
                {
                    btnUpdate.IsEnabled = false;
                    UserViewModel.RoleName = Roles.FirstOrDefault(r => r.Id == UserViewModel.RoleId).Name;
                    UserViewModel.Password = txtPassword.Password;
                    GeneralResponse resultUpdate = await _apiService.UpdateAsync(UserViewModel);
                    if (resultUpdate.Success)
                    {
                        UserViewModel.UserRoleDtos.AddRange(Roles);
                        UsuarioActualizado?.Invoke(); // para notificar a la vista principal
                    }
                    else
                        msgError(resultUpdate.Message);
                    btnUpdate.IsEnabled = true;
                }
            }
            catch (Exception ex)
            {
                msgError(ex.Message);
            }
        }

       
        private async void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                lblError.Text = string.Empty;

                if (ValidateUser())
                {
                    btnAdd.IsEnabled = false;
                    UserViewModel.RoleName = Roles.FirstOrDefault(r => r.Id == UserViewModel.RoleId).Name;
                    UserViewModel.Password = txtPassword.Password;
                    if (string.IsNullOrEmpty(UserViewModel.Password))
                    {
                        msgError("El Password es un campo obligatorio");
                    }
                    else
                    {
                        GeneralResponse resultUpdate = await _apiService.AddAsync(UserViewModel);
                        if (resultUpdate.Success)
                        {
                            UserViewModel.UserRoleDtos.AddRange(Roles);
                            UsuarioActualizado?.Invoke(); // para notificar a la vista principal
                        }
                        else
                            msgError(resultUpdate.Message);
                    }
                }
                btnAdd.IsEnabled = true;
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
            }
        }



        private void TextBox_SelectAll(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox tb)
            {
                tb.SelectAll();
            }
        }

        // Salta al siguiente control al presionar Enter
        private void TextBox_KeyDown_MoveNext(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                e.Handled = true; // evita el sonido del Enter
                if (sender is UIElement element)
                {
                    element.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
                }
            }
        }

        // Selecciona todo al hacer click con el mouse (si aún no tenía foco)
        private void TextBox_PreviewMouseLeftButtonDown_SelectAll(object sender, MouseButtonEventArgs e)
        {
            if (sender is TextBox tb && !tb.IsKeyboardFocusWithin)
            {
                e.Handled = true; // evita que WPF cambie el foco primero
                tb.Focus();
                tb.SelectAll();
            }
        }




        private bool ValidateUser()
        {
            bool result = true;

            if (string.IsNullOrEmpty(txtUserName.Text))
            {
                result = false;
                msgError("El Nombre de Usuario es un campo obligatorio");
            }
            if (string.IsNullOrEmpty(txtFirstName.Text))
            {
                result = false;
                msgError("El Nombre es un campo obligatorio");
            }
            if (string.IsNullOrEmpty(txtLastName.Text))
            {
                result = false;
                msgError("El Apellido es un campo obligatorio");
            }
            if (string.IsNullOrEmpty(txtEmail.Text))
            {
                result = false;
                msgError("El Email es un campo obligatorio");
            }
            if (!string.IsNullOrEmpty(txtEmail.Text) && !ValidatorHelper.ValidateEmail(txtEmail.Text))
            {
                result = false;
                msgError("El Email ingresador no tiene un formato válido");
            }
            if (Convert.ToInt32(cbRoles.SelectedValue) == 0)
            {
                result = false;
                msgError("Debe seleccionar el Rol");
            }


            return result;
        }




        private void msgError(string msg)
        {
            lblError.Text = msg;
        }
    }
}
