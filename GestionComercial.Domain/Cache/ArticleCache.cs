using GestionComercial.Domain.DTOs.Client;
using GestionComercial.Domain.DTOs.Stock;
using GestionComercial.Domain.Entities.Masters;
using GestionComercial.Domain.Helpers;
using GestionComercial.Domain.Response;
using GestionComercial.Domain.Statics;

namespace GestionComercial.Domain.Cache
{
    public class ArticleCache : ICache
    {
        private static ArticleCache _instance;
        public static ArticleCache Instance => _instance ??= new ArticleCache();

        private List<ArticleViewModel> _articles;

        public static bool Reading { get; set; } = false;

        private ArticleCache()
        {
            CacheManager.Register(this);
        }

        public List<ArticleViewModel> GetAll()
        {
            return _articles;
        }
        public List<ArticleViewModel> Search(string name, bool isEnabled, bool isDeleted)
        {
            return _articles != null ? _articles
                .Where(a => a.IsEnabled == isEnabled && a.IsDeleted == isDeleted
                       && ((a.Description?.ToLower().Contains(name.ToLower()) ?? false)
                        || (a.Code?.ToLower().Contains(name.ToLower()) ?? false)
                        || (a.BarCode?.ToLower().Contains(name.ToLower()) ?? false)))
                .ToList()
                :
                [];
        }
        public void Set(List<ArticleViewModel> articles)
        {
            _articles = articles;
        }
        public void Set(ArticleViewModel article)
        {
            try
            {
                _articles.Add(article);
            }
            catch (Exception)
            {

                throw;
            }
        }
        public ArticleViewModel? FindByCodeOrBarCode(string code)
        {
            return _articles != null && !string.IsNullOrEmpty(code) ? _articles
                //.Where(a => a.IsEnabled && !a.IsDeleted)
                .FirstOrDefault(a => a.Code?.ToLower() == code.ToLower()
                                  || a.BarCode?.ToLower() == code.ToLower())
                :
                new ArticleViewModel();
        }
        public ArticleViewModel? FindByBarCode(string barCode)
        {
            return _articles != null && !string.IsNullOrEmpty(barCode) ? _articles
                //.Where(a => a.IsEnabled && !a.IsDeleted)
                .FirstOrDefault(a => a.BarCode?.ToLower() == barCode.ToLower())
                :
                new ArticleViewModel();
        }
        public ArticleViewModel? FindById(int id)
        {
            try
            {
                return _articles != null ?
                                _articles.FirstOrDefault(c => c.Id == id)
                               :
                               null;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public void Update(ArticleViewModel article)
        {
            try
            {
                ArticleViewModel? articleViewModel = _articles.FirstOrDefault(c => c.Id == article.Id);
                if (articleViewModel != null)
                {
                    _articles.Remove(articleViewModel);
                    _articles.Add(article);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        public void Remove(ArticleViewModel article)
        {
            try
            {
                ArticleViewModel? articleViewModel = _articles.FirstOrDefault(c => c.Id == article.Id);
                if (articleViewModel != null)
                    _articles.Remove(articleViewModel);
            }
            catch (Exception)
            {

                throw;
            }
        }
        
        public ArticleResponse GenerateNewBarCode()
        {
            try
            {
                string commerceCode = string.Empty;
                if (StaticCommerceData.CommerceData.CUIT.ToString().Length == 11)
                    commerceCode = StaticCommerceData.CommerceData.CUIT.ToString().Substring(6, 4);
                else if (StaticCommerceData.CommerceData.CUIT.ToString().Length == 8)
                    commerceCode = StaticCommerceData.CommerceData.CUIT.ToString().Substring(4, 4);
                else
                    commerceCode = "9898";
                int cont = 0;
            Line0: string ean = "779";
                ean += commerceCode;
                if (cont == 5)
                    return new ArticleResponse
                    {
                        Success = false,
                        Message = "Se ha superado la cantidad de intentos permitidos para general código de barras"
                    };
                int code = RandomGeneratorHelper.RandomNumber(1, 99999);
                ean += code.ToString();
                int controlDigit = CalculateBarCodeDigitControl(ean);
                if (controlDigit == -1)
                {
                    cont++;
                    goto Line0;
                }
                ean += controlDigit.ToString();
                if (ean.Length == 13)
                {
                    bool result = _articles.Where(p => p.BarCode == ean).FirstOrDefault() == null;
                    if (result)
                    {
                        return new ArticleResponse { BarCode = ean, Success = true };
                    }
                    else
                    {
                        cont++;
                        goto Line0;
                    }
                }
                else
                {
                    cont++;
                    goto Line0;
                }
            }
            catch (Exception ex)
            {
                return new ArticleResponse { Success = false, Message = ex.Message };
            }
        }

        



        public void ClearCache()
        {
            _articles.Clear();
        }
        public bool HasData => _articles != null && _articles.Any() && !Reading;





        private int CalculateBarCodeDigitControl(string ean)
        {
            try
            {
                int sum = 0;
                int sumOdd = 0;
                var digit = 0;

                for (int i = ean.Length; i >= 1; i--)
                {
                    digit = Convert.ToInt32(ean.Substring(i - 1, 1));
                    if (i % 2 != 0)
                    {
                        sumOdd += digit;
                    }
                    else
                    {
                        sum += digit;
                    }
                }

                digit = (sumOdd) + (sum * 3);

                int checkSum = (10 - (digit % 10)) % 10;

                return checkSum;
            }
            catch (Exception)
            {
                return -1;
            }
        }

        
    }
}
