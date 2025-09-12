using GestionComercial.Applications.Interfaces;
using GestionComercial.Domain.DTOs.Stock;
using GestionComercial.Domain.Entities.Stock;
using GestionComercial.Domain.Helpers;
using GestionComercial.Domain.Response;
using GestionComercial.Domain.Statics;
using GestionComercial.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace GestionComercial.Applications.Services
{
    public class ArticleService : IArticleService
    {
        private readonly AppDbContext _context;
        private readonly DBHelper _dBHelper;

        #region Constructor

        public ArticleService(AppDbContext context)
        {
            _context = context;
            _dBHelper = new DBHelper();
        }

        #endregion


        public async Task<GeneralResponse> DeleteAsync(int id)
        {
            Article? product = await _context.Articles.FindAsync(id);
            if (product != null)
            {
                _context.Articles.Remove(product);
                await _context.SaveChangesAsync();
            }
            return new GeneralResponse { Success = false, Message = "Articulo no encontrado" };
        }

        public async Task<ArticleResponse> GetAllAsync(int page, int pageSize)
        {
            try
            {
                List<PriceList> priceLists = await _context.PriceLists
                       .Where(pl => pl.IsEnabled && !pl.IsDeleted)
                       .ToListAsync();
                List<Article> articles = await _context.Articles
                    .AsNoTracking()
                    .Include(p => p.Tax)
                    .Include(m => m.Measure)
                    .Include(c => c.Category)
                    .OrderBy(a => a.Code)
                    .ThenBy(a => a.Description)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                List<IGrouping<string, Article>> articlesGrouped = articles
                    .GroupBy(c => c.Category.Description)
                    .ToList();


                var totalRegisters = await _context.Articles.AsNoTracking().CountAsync();

                return new ArticleResponse
                {
                    Success = true,
                    TotalRegisters = totalRegisters,
                    ArticleViewModels = ToListPriceDto(articlesGrouped, priceLists),
                };
            }
            catch (Exception ex)
            {
                return new ArticleResponse
                {
                    Success = false,
                    Message = ex.Message,
                };
            }
        }

        public async Task<ArticleViewModel?> GetByIdAsync(int id)
        {
            List<PriceList> priceLists = await _context.PriceLists
                       .Where(pl => pl.IsEnabled && !pl.IsDeleted)
                       .ToListAsync();
            Article? article = await _context.Articles
               .AsNoTracking()
               .Include(c => c.Category)
               .Include(t => t.Tax)
               .Include(m => m.Measure)
               .Where(a => a.Id == id)
               .FirstOrDefaultAsync();

            return id == 0 || article == null ?
                new ArticleViewModel
                {
                    IsDeleted = false,
                    IsEnabled = true,
                    CreateDate = DateTime.Now,
                }
                :
                ConverterHelper.ToArticleViewModel(article, priceLists);
        }

        public async Task<GeneralResponse> UpdatePricesAsync(IProgress<int> progress, int categoryId, int percentage)
        {
            while (StaticCommon.ContextInUse)
                await Task.Delay(50);
            StaticCommon.ContextInUse = true;
            using (var transacction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    List<Article> Articles = categoryId == 0 ?
                          await _context.Articles.ToListAsync()
                           :
                          await _context.Articles.Where(p => p.CategoryId == categoryId).ToListAsync();

                    StaticCommon.ContextInUse = false;
                    int totalItems = Articles.Count(); // Número total de elementos a actualizar
                    int itemsProcessed = 0;

                    foreach (Article product in Articles)
                    {
                        product.Cost += (product.Cost * percentage / 100);
                        product.RealCost = product.Cost + (product.Cost * product.Bonification / 100);
                        _context.Entry(product).State = EntityState.Modified;

                        GeneralResponse resultUpdate = await _dBHelper.SaveChangesAsync(_context);
                        if (!resultUpdate.Success)
                        {
                            await transacction.RollbackAsync();
                            return resultUpdate;
                        }
                        itemsProcessed++;
                        double percentageReturn = (itemsProcessed / (double)totalItems) * 100;
                        progress.Report((int)percentageReturn);
                    }

                    await transacction.CommitAsync();
                    return new GeneralResponse
                    {
                        Success = true,
                    };
                }
                catch (Exception ex)
                {
                    await transacction.RollbackAsync();
                    return new GeneralResponse
                    {
                        Message = ex.Message,
                        Success = false,
                    };
                }
                finally
                {
                    StaticCommon.ContextInUse = false;
                }
            }
        }

        public async Task<ArticleResponse> GenerateNewBarCodeAsync()
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
                    while (StaticCommon.ContextInUse)
                        await Task.Delay(100);
                    StaticCommon.ContextInUse = true;
                    bool result = await _context.Articles.Where(p => p.BarCode == ean).FirstOrDefaultAsync() == null;
                    StaticCommon.ContextInUse = false;
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
                StaticCommon.ContextInUse = false;
                return new ArticleResponse { Success = false, Message = ex.Message };
            }
        }




        #region Private Methods

        private ArticleViewModel ToPriceDto(Article article, ICollection<PriceList> priceLists)
        {
            return new()
            {
                Id = article.Id,
                Stock = article.Stock,
                Code = article.Code,
                Description = article.Description,
                Category = article.Category.Description,
                CategoryColor = article.Category.Color,
                CostWithTaxes = article.CostWithTaxes,
                Cost = article.Cost,
                Bonification = article.Bonification,
                RealCost = article.RealCost,
                BarCode = article.BarCode,
                CategoryId = article.CategoryId,
                ChangePoint = article.ChangePoint,
                Clarifications = article.Clarifications,
                CreateDate = article.CreateDate,
                CreateUser = article.CreateUser,
                InternalTax = article.InternalTax,
                IsDeleted = article.IsDeleted,
                IsEnabled = article.IsEnabled,
                IsWeight = article.IsWeight,
                MeasureId = article.MeasureId,
                MinimalStock = article.MinimalStock,
                Remark = article.Remark,
                Replacement = article.Replacement,
                SalePoint = article.SalePoint,
                StockCheck = article.StockCheck,
                TaxId = article.TaxId,
                Umbral = article.Umbral,
                UpdateDate = article.UpdateDate,
                UpdateUser = article.UpdateUser,
                Utility = article.Utility,
                //Categories = categories,
                //Measures = measures,
                //Taxes = taxes,
                TaxesPrice =
                [
                     new TaxePriceDto
                     {
                         Description = $"I.V.A. {article.Tax.Description}",
                         Utility = article.Tax.Rate,
                         Price = article.RealCost * article.Tax.Rate /100,
                     },
                     new TaxePriceDto
                     {
                         Description = string.Format("Impuestos internos {0}%", article.InternalTax),
                         Utility =article.InternalTax,
                         Price = article.RealCost * article.InternalTax /100,
                     }
                ],
                PriceLists = priceLists.Select(pl => new PriceListItemDto
                {
                    Id = pl.Id,
                    Description = pl.Description,
                    Utility = pl.Utility,
                    FinalPrice = article.CostWithTaxes + (article.CostWithTaxes * article.Utility / 100) + (article.CostWithTaxes * pl.Utility / 100)
                })
                .OrderBy(pl => pl.Utility)
                .ToList() // Ordenamos para que la lista 1 (utility=0) aparezca primero, luego las que ofrecen descuentos
            };
        }


        private List<ArticleViewModel> ToListPriceDto(List<IGrouping<string, Article>> articles,
            List<PriceList> priceLists)
        {
            return articles
                .SelectMany(group => group.Select(article => new ArticleViewModel
                {
                    Id = article.Id,
                    Stock = article.Stock,
                    Code = article.Code,
                    Description = article.Description,
                    Category = article.Category.Description,
                    CategoryColor = article.Category.Color,
                    CostWithTaxes = article.CostWithTaxes,
                    Cost = article.Cost,
                    Bonification = article.Bonification,
                    RealCost = article.RealCost,
                    BarCode = article.BarCode,
                    CategoryId = article.CategoryId,
                    ChangePoint = article.ChangePoint,
                    Clarifications = article.Clarifications,
                    CreateDate = article.CreateDate,
                    CreateUser = article.CreateUser,
                    InternalTax = article.InternalTax,
                    IsDeleted = article.IsDeleted,
                    IsEnabled = article.IsEnabled,
                    IsWeight = article.IsWeight,
                    MeasureId = article.MeasureId,
                    MinimalStock = article.MinimalStock,
                    Remark = article.Remark,
                    Replacement = article.Replacement,
                    SalePoint = article.SalePoint,
                    StockCheck = article.StockCheck,
                    TaxId = article.TaxId,
                    Umbral = article.Umbral,
                    UpdateDate = article.UpdateDate,
                    UpdateUser = article.UpdateUser,
                    Utility = article.Utility,
                    SalePrice = article.SalePrice,
                    SalePriceWithTaxes = article.SalePriceWithTaxes,
                    //Categories = categories,
                    //Measures = measures,
                    //Taxes = taxes,
                    TaxesPrice =
                    [
                        new TaxePriceDto
                        {
                            Description = $"I.V.A. {article.Tax.Description}",
                            Utility = article.Tax.Rate,
                            Price = article.RealCost * article.Tax.Rate /100,
                        },
                        new TaxePriceDto
                        {
                            Description = string.Format("Imp. int. {0}%", article.InternalTax),
                            Utility =article.InternalTax,
                            Price = article.RealCost * article.InternalTax /100,
                        }
                    ],
                    PriceLists = priceLists
                    .Select(pl => new PriceListItemDto
                    {
                        Id = pl.Id,
                        Description = pl.Description,
                        Utility = pl.Utility,
                        FinalPrice = article.SalePriceWithTaxes + (article.SalePriceWithTaxes * pl.Utility / 100),
                    })
                    .OrderBy(pl => pl.Utility)
                    .ToList()// Ordenamos para que la lista 1 (utility=0) aparezca primero, luego las que ofrecen descuentos
                }))
                .ToList();
        }


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

        #endregion
    }
}
