using GestionComercial.Domain.DTOs.Stock;
using GestionComercial.Domain.Entities.Afip;
using GestionComercial.Domain.Entities.Masters;
using GestionComercial.Domain.Entities.Stock;

namespace GestionComercial.Domain.Helpers
{
    public static class ConverterHelper
    {
        public static Article ToArticle(ArticleViewModel articleViewMoodel, bool isNew)
        {
            return new Article
            {
                Id = isNew ? 0 : articleViewMoodel.Id,
                BarCode = articleViewMoodel.BarCode,
                UpdateUser = articleViewMoodel.UpdateUser,
                UpdateDate = articleViewMoodel.UpdateDate,
                Umbral = articleViewMoodel.Umbral,
                TaxId = articleViewMoodel.TaxId,
                StockCheck = articleViewMoodel.StockCheck,
                Stock = articleViewMoodel.Stock,
                Bonification = articleViewMoodel.Bonification /100,
                CategoryId = articleViewMoodel.CategoryId,
                ChangePoint = articleViewMoodel.ChangePoint,
                Clarifications = articleViewMoodel.Clarifications,
                Code = articleViewMoodel.Code,
                Cost = articleViewMoodel.Cost,
                CreateDate = DateTime.Now,
                CreateUser = articleViewMoodel.CreateUser,
                Description = articleViewMoodel.Description,
                InternalTax = articleViewMoodel.InternalTax,
                IsDeleted = articleViewMoodel.IsDeleted,
                IsEnabled = articleViewMoodel.IsEnabled,
                IsWeight = articleViewMoodel.IsWeight,
                MeasureId = articleViewMoodel.MeasureId,
                MinimalStock = articleViewMoodel.MinimalStock,
                RealCost = articleViewMoodel.RealCost,
                Remark = articleViewMoodel.Remark,
                Replacement = articleViewMoodel.Replacement,
                SalePoint = articleViewMoodel.SalePoint,                 
            };
        }

        public static ArticleViewModel ToArticleViewModel(Article? article, ICollection<Tax> taxes, ICollection<Measure> measures, ICollection<Category> categories)
        {
            return new ArticleViewModel
            {
                Id = article.Id,
                BarCode = article.BarCode,
                Bonification = article.Bonification * 100,
                CategoryId = article.CategoryId,
                ChangePoint = article.ChangePoint,
                Clarifications = article.Clarifications,
                Code = article.Code,
                Cost = article.Cost,
                CreateDate = article.CreateDate,
                CreateUser = article.CreateUser,
                Description = article.Description,
                InternalTax = article.InternalTax,
                IsDeleted = article.IsDeleted,
                IsEnabled = article.IsEnabled,
                IsWeight = article.IsWeight,
                MeasureId = article.MeasureId,
                MinimalStock = article.MinimalStock,
                RealCost = article.RealCost,
                Remark = article.Remark,
                Replacement = article.Replacement,
                SalePoint = article.SalePoint,
                Stock = article.Stock,
                StockCheck = article.StockCheck,
                TaxId = article.TaxId,
                Umbral = article.Umbral,
                UpdateDate = article.UpdateDate,
                UpdateUser = article.UpdateUser,
                Categories = categories,
                Measures = measures,
                Taxes = taxes,
            };
        }
    }
}
