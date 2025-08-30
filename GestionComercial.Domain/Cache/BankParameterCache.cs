using GestionComercial.Domain.DTOs.Banks;

namespace GestionComercial.Domain.Cache
{
    public class BankParameterCache : ICache
    {
        private static BankParameterCache _instance;
        public static BankParameterCache Instance => _instance ??= new BankParameterCache();

        private List<BankParameterViewModel> _bankParameterViewModels;

        private BankParameterCache()
        {
            CacheManager.Register(this);
        }

        public List<BankParameterViewModel> GetAllBankParameters()
        {
            return _bankParameterViewModels;
        }

        public List<BankParameterViewModel> SearchBankParameters(string name, bool isEnabled, bool isDeleted)
        {
            try
            {
                return _bankParameterViewModels != null ? _bankParameterViewModels
                              .Where(p => p.IsEnabled == isEnabled
                                       && p.IsDeleted == isDeleted
                                       && (p.BankName?.ToLower().Contains(name.ToLower()) ?? false))
                              .ToList()
                              :
                              [];
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void SetBankParameters(List<BankParameterViewModel> boxAndBanks)
        {
            try
            {
                _bankParameterViewModels = boxAndBanks;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public void SetBankParameter(BankParameterViewModel bankAndBox)
        {
            try
            {
                _bankParameterViewModels.Add(bankAndBox);
            }
            catch (Exception)
            {

                throw;
            }
        }


        public void ClearCache()
        {
            _bankParameterViewModels.Clear();
        }

        public bool HasData => _bankParameterViewModels != null && _bankParameterViewModels.Any();
    }
}
