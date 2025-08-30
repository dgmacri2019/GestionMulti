using GestionComercial.Domain.DTOs.Banks;

namespace GestionComercial.Domain.Cache
{
    public class BoxAndBankCache : ICache
    {
        private static BoxAndBankCache _instance;
        public static BoxAndBankCache Instance => _instance ??= new BoxAndBankCache();

        private List<BankAndBoxViewModel> _boxAndBanks;

        private BoxAndBankCache()
        {
            CacheManager.Register(this);
        }

        public List<BankAndBoxViewModel> GetAllBoxAndBanks()
        {
            return _boxAndBanks;
        }

        public List<BankAndBoxViewModel> SearchBoxAndBanks(string name, bool isEnabled, bool isDeleted)
        {
            try
            {
                return _boxAndBanks != null ? _boxAndBanks
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

        public void SetBoxAndBanks(List<BankAndBoxViewModel> boxAndBanks)
        {
            try
            {
                _boxAndBanks = boxAndBanks;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public void SetBoxAndBank(BankAndBoxViewModel bankAndBox)
        {
            try
            {
                _boxAndBanks.Add(bankAndBox);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public void UpdateBoxAndBank(BankAndBoxViewModel bankAndBox)
        {
            try
            {
                BankAndBoxViewModel bankAndBoxViewModel = _boxAndBanks.FirstOrDefault(c => c.Id == bankAndBox.Id);
                if (bankAndBoxViewModel != null)
                {
                    _boxAndBanks.Remove(bankAndBoxViewModel);
                    _boxAndBanks.Add(bankAndBox);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        public void RemoveBoxAndBank(BankAndBoxViewModel bankAndBox)
        {
            try
            {
                BankAndBoxViewModel bankAndBoxViewModel = _boxAndBanks.FirstOrDefault(c => c.Id == bankAndBox.Id);
                if (bankAndBoxViewModel != null)
                    _boxAndBanks.Remove(bankAndBoxViewModel);
            }
            catch (Exception)
            {

                throw;
            }

        }


        public void ClearCache()
        {
            _boxAndBanks.Clear();
        }

        public bool HasData => _boxAndBanks != null && _boxAndBanks.Any();
    }
}
