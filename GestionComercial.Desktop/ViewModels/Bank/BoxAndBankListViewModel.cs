using GestionComercial.Desktop.Services;
using GestionComercial.Domain.DTOs.Bank;
using System.Collections.ObjectModel;

namespace GestionComercial.Desktop.ViewModels.Bank
{
    internal class BoxAndBankListViewModel
    {
        private readonly BanksApiService _bankApiService;

        public ObservableCollection<BankAndBoxViewModel> BoxAndBanks { get; set; } = [];


        public BoxAndBankListViewModel(string name, bool isEnabled, bool isDeleted)
        {
            _bankApiService = new BanksApiService();
            SearchAsync(name, isEnabled, isDeleted);
        }

        private async Task SearchAsync(string name, bool isEnabled, bool isDeleted)
        {
            try
            {
                List<BankAndBoxViewModel> model = await _bankApiService.SearchAsync(name, isEnabled, isDeleted);
                BoxAndBanks.Clear();
                foreach (var p in model)
                {
                    BoxAndBanks.Add(p);
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
