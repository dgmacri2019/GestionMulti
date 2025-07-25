using GestionComercial.Desktop.Services;
using GestionComercial.Domain.DTOs.User;
using System.Collections.ObjectModel;

namespace GestionComercial.Desktop.ViewModels.Users
{
    internal class UserListViewModel : BaseViewModel
    {
        private readonly UsersApiService _usersApiService;
        public ObservableCollection<UserViewModel> Users { get; set; } = [];

        public UserListViewModel(bool isEnabled, bool all)
        {
            _usersApiService = new UsersApiService();
            GetAllAsync(isEnabled, all);
        }

        public UserListViewModel(string name, bool isEnabled)
        {
            _usersApiService = new UsersApiService();
            SearchToListAsync(name, isEnabled);
        }

        private async Task<ObservableCollection<UserViewModel>> GetAllAsync(bool isEnabled, bool all)
        {
            try
            {
                List<UserViewModel> users = await _usersApiService.GetAllAsync(isEnabled, all);
                Users.Clear();
                foreach (var u in users)
                {
                    Users.Add(u);
                }

                return Users;
            }
            catch (Exception ex)
            {
                return Users;
            }
        }

        private async Task SearchToListAsync(string name, bool isEnabled)
        {
            try
            {
                List<UserViewModel> users = await _usersApiService.SearchToListAsync(name, isEnabled);
                Users.Clear();
                foreach (var u in users)
                {
                    Users.Add(u);
                }
            }
            catch (Exception ex)
            {

            }
        }
    }
}
