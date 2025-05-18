using GestionComercial.Desktop.Services;
using GestionComercial.Domain.DTOs.Banks;
using System.Collections.ObjectModel;

namespace GestionComercial.Desktop.ViewModels.Bank
{
    internal class BankParameterListViewModel
    {
        private readonly BanksApiService _bankApiService;

        public ObservableCollection<BankParameterViewModel> BankParameters { get; set; } = [];

        public BankParameterListViewModel(string name, bool isEnabled, bool isDeleted)
        {
            _bankApiService = new BanksApiService();
            SearchAsync(name, isEnabled, isDeleted);
        }

        private async Task SearchAsync(string name, bool isEnabled, bool isDeleted)
        {
            try
            {
                List<BankParameterViewModel> model = await _bankApiService.SearchBankParameterAsync(name, isEnabled, isDeleted);
                BankParameters.Clear();
                foreach (var p in model)
                {
                    BankParameters.Add(p);
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
