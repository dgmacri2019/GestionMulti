using GestionComercial.Desktop.Services;
using GestionComercial.Domain.DTOs.Provider;
using System.Collections.ObjectModel;

namespace GestionComercial.Desktop.ViewModels.Provider
{
    internal class ProviderListViewModel : BaseViewModel
    {
        private readonly ProvidersApiService _providersApiService;

        public ObservableCollection<ProviderViewModel> Providers { get; set; } = [];


        public ProviderListViewModel(bool isEnabled, bool isDeleted)
        {
            _providersApiService = new ProvidersApiService();
            GetAllAsync(isEnabled, isDeleted);
        }


        public ProviderListViewModel(string name, bool isEnabled, bool isDeleted)
        {
            _providersApiService = new ProvidersApiService();
            SearchAsync(name, isEnabled, isDeleted);
        }

        private async Task SearchAsync(string name, bool isEnabled, bool isDeleted)
        {
            try
            {
                List<ProviderViewModel> providers = await _providersApiService.SearchAsync(name, isEnabled, isDeleted);
                Providers.Clear();
                foreach (var p in providers)
                {
                    Providers.Add(p);
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        private async Task<ObservableCollection<ProviderViewModel>> GetAllAsync(bool isEnabled, bool isDeleted)
        {
            List<ProviderViewModel> providers = await _providersApiService.GetAllAsync(isEnabled, isDeleted);
            Providers.Clear();
            foreach (var p in providers)
            {
                Providers.Add(p);
            }

            return Providers;
        }
    }
}
