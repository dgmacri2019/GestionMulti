﻿using GestionComercial.Applications.Interfaces;
using GestionComercial.Domain.DTOs.Stock;
using GestionComercial.Domain.Entities.Afip;
using GestionComercial.Domain.Entities.Masters;
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



        public async Task<ArticleWithPricesDto?> FindByBarCodeAsync(string barCode)
        {
            // Incluimos las listas de precios; asegúrate de que la propiedad esté activa en Product
            ICollection<PriceList> priceLists = await _context.PriceLists
                .Where(pl => pl.IsEnabled && !pl.IsDeleted)
                .ToListAsync();
            Article? product = await _context.Articles.Where(p => p.BarCode == barCode).FirstOrDefaultAsync();

            return product == null ? null : ToPriceDto(product, priceLists);
        }



        public async Task<ArticleWithPricesDto?> FindByCodeOrBarCodeAsync(string code)
        {
            // Incluimos las listas de precios; asegúrate de que la propiedad esté activa en Product
            ICollection<PriceList> priceLists = await _context.PriceLists
                .Where(pl => pl.IsEnabled && !pl.IsDeleted)
                .ToListAsync();
            Article? product = string.IsNullOrEmpty(code) ? null : await _context.Articles
                .Where(p => p.Code == code || p.BarCode == code)
                .FirstOrDefaultAsync();

            return product == null ? null : ToPriceDto(product, priceLists);
        }



        public async Task<IEnumerable<ArticleWithPricesDto>> GetAllAsync(bool isEnabled, bool isDeleted)
        {
            // Incluimos las listas de precios; asegúrate de que la propiedad esté activa en Product
            ICollection<PriceList> priceLists = await _context.PriceLists
                .Where(pl => pl.IsEnabled && !pl.IsDeleted)
                .ToListAsync();
            List<IGrouping<string, Article>> Articles = await _context.Articles
                .Include(p => p.Tax)
                .Include(m => m.Measure)
                .Include(c => c.Category)
                .Where(p => p.IsEnabled == isEnabled && p.IsDeleted == isDeleted)
                .GroupBy(c => c.Category.Description)
                .ToListAsync();

            return ToListPriceDto(Articles, priceLists);
        }



        public async Task<ArticleViewModel?> GetByIdAsync(int id)
        {
            // Incluimos las listas de precios; asegúrate de que la propiedad esté activa en Product
            ICollection<PriceList> priceLists = await _context.PriceLists
               .Where(pl => pl.IsEnabled && !pl.IsDeleted)
               .ToListAsync();

            ICollection<Tax> taxes = await _context.Taxes
                .Where(pl => pl.IsEnabled && !pl.IsDeleted)
                .ToListAsync();
            ICollection<Measure> measures = await _context.Measures
                .Where(pl => pl.IsEnabled && !pl.IsDeleted)
                .ToListAsync();
            ICollection<Category> categories = await _context.Categories
                .Where(pl => pl.IsEnabled && !pl.IsDeleted)
                .ToListAsync();
            taxes.Add(new Tax { Id = 0, Description = "Seleccione el I.V.A." });
            measures.Add(new Measure { Id = 0, Description = "Seleccione la unidad de medida" });
            categories.Add(new Category { Id = 0, Description = "Seleccione la categoría" });

            if (id == 0)
                return new ArticleViewModel
                {
                    Taxes = taxes,
                    Measures = measures,
                    Categories = categories,
                    IsDeleted = false,
                    IsEnabled = true,
                    CreateDate = DateTime.Now,
                };

            Article? article = await _context.Articles
                .Include(c => c.Category)
                .Include(t => t.Tax)
                .Include(m => m.Measure)
                .Where(a => a.Id == id)
                .FirstOrDefaultAsync();

            return article == null ? null : ConverterHelper.ToArticleViewModel(article, taxes, measures, categories);
        }



        public async Task<IEnumerable<ArticleWithPricesDto>> SearchToListAsync(string description, bool isEnabled, bool isDeleted)
        {
            // Incluimos las listas de precios; asegúrate de que la propiedad esté activa en Product
            ICollection<PriceList> priceLists = await _context.PriceLists
                .Where(pl => pl.IsEnabled && !pl.IsDeleted)
                .ToListAsync();
            List<IGrouping<string, Article>> Articles = await _context.Articles
                 .Include(p => p.Tax)
                 .Include(m => m.Measure)
                 .Include(c => c.Category)
                 .Where(c => c.IsDeleted == isDeleted && c.IsEnabled == isEnabled && (c.Description.Contains(description) || c.Code.Contains(description) || c.Category.Description.Contains(description) || c.BarCode.Contains(description)))
                 .GroupBy(c => c.Category.Description)
                 .ToListAsync();

            return ToListPriceDto(Articles, priceLists);
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

        private ArticleWithPricesDto ToPriceDto(Article article, ICollection<PriceList> priceLists)
        {
            return new()
            {
                Id = article.Id,
                Stock = article.Stock,
                Code = article.Code,
                Description = article.Description,
                Category = article.Category.Description,
                Cost = article.Cost,
                Bonification = article.Bonification,
                RealCost = article.RealCost,
                PriceWithTax = article.PriceWithTaxes,
                CategoryColor = article.Category.Color,
                BarCode = article.BarCode,
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
                    Description = pl.Description,
                    Utility = pl.Utility,
                    FinalPrice = article.PriceWithTaxes + (article.PriceWithTaxes * pl.Utility / 100)
                })
                .OrderBy(pl => pl.Utility)
                .ToList() // Ordenamos para que la lista 1 (utility=0) aparezca primero, luego las que ofrecen descuentos
            };
        }


        private IEnumerable<ArticleWithPricesDto> ToListPriceDto(List<IGrouping<string, Article>> Articles, ICollection<PriceList> priceLists)
        {
            return Articles.SelectMany(group => group.Select(article => new ArticleWithPricesDto
            {
                Id = article.Id,
                Stock = article.Stock,
                Code = article.Code,
                Description = article.Description,
                Category = article.Category.Description,
                CategoryColor = article.Category.Color,
                PriceWithTax = article.PriceWithTaxes,
                Cost = article.Cost,
                Bonification = article.Bonification,
                RealCost = article.RealCost,
                BarCode = article.BarCode,
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
                    Description = pl.Description,
                    Utility = pl.Utility,
                    FinalPrice = article.PriceWithTaxes + (article.PriceWithTaxes * pl.Utility / 100)
                })
                .OrderBy(pl => pl.Utility)
                .ToList()// Ordenamos para que la lista 1 (utility=0) aparezca primero, luego las que ofrecen descuentos
            }));
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
