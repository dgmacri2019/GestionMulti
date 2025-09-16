using GestionComercial.Applications.Interfaces;
using GestionComercial.Domain.DTOs.PriceLists;
using GestionComercial.Domain.Entities.Afip;
using GestionComercial.Domain.Entities.Masters;
using GestionComercial.Domain.Entities.Stock;
using GestionComercial.Domain.Helpers;
using GestionComercial.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace GestionComercial.Applications.Services
{
    public class MasterClassService : IMasterClassService
    {
        private readonly AppDbContext _context;
        private readonly DBHelper _dBHelper;

        public MasterClassService(AppDbContext context)
        {
            _context = context;
            _dBHelper = new DBHelper();

        }


        public async Task<CommerceData?> GetCommerceDataAsync()
        {
            return await _context.CommerceDatas.FirstOrDefaultAsync();
        }




        public async Task<List<int>> GetAllArticlesId()
        {
            return await _context.Articles
                .AsNoTracking()
                .Select(a => a.Id)
                .ToListAsync();
        }


        public async Task<IEnumerable<Category>> GetAllCategoriesAsync()
        {
            return await _context.Categories
                .Include(a => a.Articles)
                .ToListAsync();
        }
        public async Task<Category?> GetCategoryByIdAsync(int id)
        {
            return await _context.Categories
                .Where(c => c.Id == id)
                .Include(a => a.Articles)
                .FirstOrDefaultAsync();

        }



        public async Task<IEnumerable<PriceList>> GetAllPriceListAsync()
        {
            return await _context.PriceLists.ToListAsync();
        }
        public async Task<PriceList?> GetPriceListByIdAsync(int id)
        {
            return await _context.PriceLists.FindAsync(id);
        }











        public async Task<IEnumerable<DocumentType>> GetAllDocumentTypesAsync(bool isEnabled, bool isDeleted)
        {
            return await _context.DocumentTypes
                .Where(p => p.IsEnabled == isEnabled && p.IsDeleted == isDeleted)
                .ToListAsync();
        }

        public async Task<IEnumerable<IvaCondition>> GetAllIvaConditionsAsync(bool isEnabled, bool isDeleted)
        {
            return await _context.IvaConditions
                .Where(p => p.IsEnabled == isEnabled && p.IsDeleted == isDeleted)
                .ToListAsync();
        }

        public async Task<IEnumerable<Measure>> GetAllMeasuresAsync(bool isEnabled, bool isDeleted)
        {
            return await _context.Measures
                .Where(p => p.IsEnabled == isEnabled && p.IsDeleted == isDeleted)
                .ToListAsync();
        }

        public async Task<IEnumerable<SaleCondition>> GetAllSaleConditionsAsync(bool isEnabled, bool isDeleted)
        {
            return await _context.SaleConditions
                 .Where(p => p.IsEnabled == isEnabled && p.IsDeleted == isDeleted)
                 .ToListAsync();
        }

        public async Task<IEnumerable<State>> GetAllStatesAsync(bool isEnabled, bool isDeleted)
        {
            return await _context.States
                .Where(p => p.IsEnabled == isEnabled && p.IsDeleted == isDeleted)
                .ToListAsync();
        }

        public async Task<IEnumerable<Tax>> GetAllTaxesAsync(bool isEnabled, bool isDeleted)
        {
            return await _context.Taxes
                .Where(p => p.IsEnabled == isEnabled && p.IsDeleted == isDeleted)
                .ToListAsync();
        }











        private IEnumerable<PriceListViewModel> ToPriceListViewModelList(List<PriceList> priceLists)
        {
            return priceLists.Select(provider => new PriceListViewModel
            {
                Id = provider.Id,
                Description = provider.Description,
                Utility = provider.Utility,
                CreateDate = provider.CreateDate,
                CreateUser = provider.CreateUser,
                UpdateDate = provider.UpdateDate,
                UpdateUser = provider.UpdateUser,
                IsDeleted = provider.IsDeleted,
                IsEnabled = provider.IsEnabled
            });
        }


    }
}
