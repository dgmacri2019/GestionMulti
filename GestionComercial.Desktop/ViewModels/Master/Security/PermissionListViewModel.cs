using GestionComercial.Desktop.Helpers;
using GestionComercial.Desktop.Services;
using GestionComercial.Domain.Cache;
using GestionComercial.Domain.Constant;
using GestionComercial.Domain.DTOs.Security;
using GestionComercial.Domain.DTOs.User;
using GestionComercial.Domain.Entities.Masters.Security;
using GestionComercial.Domain.Response;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Reflection;
using static GestionComercial.Domain.Constant.Enumeration;

namespace GestionComercial.Desktop.ViewModels.Master.Security
{
    public class PermissionListViewModel : INotifyPropertyChanged
    {
        private List<UserRoleDto> UserRoleDtos =
       [
           new UserRoleDto { Id = 0, Name = "Seleccione el Rol" },
            new UserRoleDto { Id = 1, Name = "Developer" },
            new UserRoleDto { Id = 2, Name = "Administrador" },
            new UserRoleDto { Id = 3, Name = "Supervisor" },
            new UserRoleDto { Id = 4, Name = "Operador"},
            new UserRoleDto { Id = 5, Name = "Cajero" }
       ];
        private readonly PermissionsApiService _permissionsApiService;
        public ObservableCollection<UserViewModel> Users { get; set; }
        public ObservableCollection<PermissionGroupViewModel> PermissionGroups { get; set; }

        private UserViewModel _selectedUser;
        public UserViewModel SelectedUser
        {
            get => _selectedUser;
            set
            {
                _selectedUser = value;
                OnPropertyChanged(nameof(SelectedUser));
            }
        }


        private bool _selectAllRead;
        public bool SelectAllRead
        {
            get => _selectAllRead;
            set
            {
                if (_selectAllRead != value)
                {
                    _selectAllRead = value;
                    OnPropertyChanged(nameof(SelectAllRead));
                    foreach (var module in PermissionGroups)
                        if (module.IsEnabledCanRead)
                            module.CanRead = value;
                }
            }
        }

        private bool _selectAllAdd;
        public bool SelectAllAdd
        {
            get => _selectAllAdd;
            set
            {
                if (_selectAllAdd != value)
                {
                    _selectAllAdd = value;
                    OnPropertyChanged(nameof(SelectAllAdd));
                    foreach (var module in PermissionGroups)
                        if (module.IsEnabledCanAdd) module.CanAdd = value;
                }
            }
        }

        private bool _selectAllEdit;
        public bool SelectAllEdit
        {
            get => _selectAllEdit;
            set
            {
                if (_selectAllEdit != value)
                {
                    _selectAllEdit = value;
                    OnPropertyChanged(nameof(SelectAllEdit));
                    foreach (var module in PermissionGroups)
                        if (module.IsEnabledCanEdit)
                            module.CanEdit = value;
                }
            }
        }

        private bool _selectAllDelete;
        public bool SelectAllDelete
        {
            get => _selectAllDelete;
            set
            {
                if (_selectAllDelete != value)
                {
                    _selectAllDelete = value;
                    OnPropertyChanged(nameof(SelectAllDelete));
                    foreach (var module in PermissionGroups)
                        if (module.IsEnabledCanDelete)
                            module.CanDelete = value;
                }
            }
        }
        public PermissionListViewModel()
        {
            int roleId = UserRoleDtos.First(urd => urd.Name == LoginUserCache.UserRole).Id;
            Users = new ObservableCollection<UserViewModel>(UserCache.Instance.GetAll().Where(u => u.RoleId >= roleId));
            _permissionsApiService = new PermissionsApiService();
            PermissionGroups = [];
        }


        public async Task LoadPermissionsForUserAsync(string userId)
        {
            try
            {
                PermissionGroups.Clear();
        
                List<UserPermission> userPermissions;

                PermissionResponse permissionResponse = await _permissionsApiService.GetUserPermissionsForUserAsync(userId);

                if (permissionResponse.Success)
                {
                    userPermissions = permissionResponse.UserPermissions;

                    foreach (var module in Enum.GetValues(typeof(ModuleType)).Cast<ModuleType>())
                    {
                        if (AutorizeOperationHelper.ValidateModule(module))
                        {
                            PermissionGroupViewModel group = new(module)
                            {
                                CanRead = userPermissions.First(p => p.Permission.ModuleType == module && p.Permission.Name.EndsWith("Lectura") && p.Permission.IsEnabled).IsEnabled,
                                IsEnabledCanRead = AutorizeOperationHelper.ValidateOperation(module, string.Format("{0}-Lectura", EnumExtensionService.GetDisplayName(module))),
                                CanAdd = userPermissions.First(p => p.Permission.ModuleType == module && p.Permission.Name.EndsWith("Agregar") && p.Permission.IsEnabled).IsEnabled,
                                IsEnabledCanAdd = AutorizeOperationHelper.ValidateOperation(module, string.Format("{0}-Agregar", EnumExtensionService.GetDisplayName(module))),
                                CanEdit = userPermissions.First(p => p.Permission.ModuleType == module && p.Permission.Name.EndsWith("Editar") && p.Permission.IsEnabled).IsEnabled,
                                IsEnabledCanEdit = AutorizeOperationHelper.ValidateOperation(module, string.Format("{0}-Editar", EnumExtensionService.GetDisplayName(module))),
                                CanDelete = userPermissions.First(p => p.Permission.ModuleType == module && p.Permission.Name.EndsWith("Borrar") && p.Permission.IsEnabled).IsEnabled,
                                IsEnabledCanDelete = AutorizeOperationHelper.ValidateOperation(module, string.Format("{0}-Borrar", EnumExtensionService.GetDisplayName(module))),
                            };

                            PermissionGroups.Add(group);
                        }
                    }
                }
                else
                    MsgBoxAlertHelper.MsgAlertError(permissionResponse.Message);
            }
            catch (Exception ex)
            {
                MsgBoxAlertHelper.MsgAlertError(ex.Message);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string prop) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }

}
