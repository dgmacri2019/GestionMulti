using GestionComercial.Desktop.Helpers;
using GestionComercial.Desktop.Services;
using GestionComercial.Desktop.Services.Hub;
using GestionComercial.Desktop.Utils;
using GestionComercial.Domain.Cache;
using GestionComercial.Domain.DTOs.User;
using GestionComercial.Domain.Response;
using System.Collections.ObjectModel;
using System.Windows.Input;
using static GestionComercial.Domain.Notifications.UserChangeNotification;

namespace GestionComercial.Desktop.ViewModels.Users
{
    internal class UserListViewModel : BaseViewModel
    {
        private readonly UsersApiService _usersApiService;
        private readonly UsersHubService _hubService;

        public ObservableCollection<UserViewModel> Users { get; set; } = [];

        // 🔹 Propiedades de filtros
        private string _nameFilter = string.Empty;
        public string NameFilter
        {
            get => _nameFilter;
            set
            {
                if (_nameFilter != value)
                {
                    _nameFilter = value;
                    OnPropertyChanged();
                    _ = LoadUsersAsync(); // 🔹 ejecuta búsqueda al escribir
                }
            }
        }

        private bool _isEnabledFilter = true;
        public bool IsEnabledFilter
        {
            get => _isEnabledFilter;
            set
            {
                if (_isEnabledFilter != value)
                {
                    _isEnabledFilter = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool _isDeletedFilter = false;
        public bool IsDeletedFilter
        {
            get => _isDeletedFilter;
            set
            {
                if (_isDeletedFilter != value)
                {
                    _isDeletedFilter = value;
                    OnPropertyChanged();
                }
            }
        }

        // 🔹 Command para buscar
        public ICommand SearchCommand { get; }
        public ICommand ToggleEnabledCommand { get; }


        public string ToggleEnabledText => IsEnabledFilter ? "Ver Inhabilitados" : "Ver Habilitados";
        public UserListViewModel()
        {
            _usersApiService = new UsersApiService();
            var hubUrl = string.Format("{0}hubs/users", App.Configuration["ApiSettings:BaseUrl"]);
            _hubService = new UsersHubService(hubUrl);
            _hubService.UsuarioCambiado += OnUsuarioCambiado;
            _ = _hubService.StartAsync();
            ToggleEnabledCommand = new RelayCommand1(async _ => await ToggleEnabled());
            SearchCommand = new RelayCommand1(async _ => await LoadUsersAsync());
            _ = LoadUsersAsync();
        }


        private async Task ToggleEnabled()
        {
            IsEnabledFilter = !IsEnabledFilter;
            OnPropertyChanged(nameof(ToggleEnabledText));
            await LoadUsersAsync();
        }



        private async Task LoadUsersAsync()
        {
            try
            {
                if (!UserCache.Instance.HasData)
                {
                    UserCache.Reading = true;

                    UserResponse userResponse = await _usersApiService.GetAllAsync();
                    if (userResponse.Success)
                        UserCache.Instance.Set(userResponse.UserViewModels);
                    else
                        MsgBoxAlertHelper.MsgAlertError($"Error al cargar usuarios, el error fue:\n{userResponse.Message}");


                    UserCache.Reading = false;
                }

                List<UserViewModel> filtered = UserCache.Instance.Search(NameFilter, IsEnabledFilter, IsDeletedFilter);

                App.Current.Dispatcher.Invoke(() =>
                {
                    Users.Clear();
                    foreach (var p in filtered
                                        .OrderBy(u => u.RoleName)
                                        .ThenBy(u => u.FullName))

                        Users.Add(p);
                });
            }
            catch (Exception ex)
            {
                MsgBoxAlertHelper.MsgAlertError(ex.Message);
            }
        }


        private async void OnUsuarioCambiado(UsuarioChangeNotification notification)
        {
            UserResponse userResponse = await _usersApiService.GetAllAsync();
            if (userResponse.Success)
                await App.Current.Dispatcher.InvokeAsync(() =>
                {
                    UserCache.Instance.ClearCache();
                    UserCache.Instance.Set(userResponse.UserViewModels);

                    _ = LoadUsersAsync();
                });
        }


    }
}
