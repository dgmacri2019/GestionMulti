using GestionComercial.Applications.Interfaces;
using GestionComercial.Domain.Entities.Afip;
using GestionComercial.Domain.Entities.Masters;
using GestionComercial.Domain.Entities.Stock;
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

        public async Task<IEnumerable<Category>> GetAllCategoriesAsync(bool isEnabled, bool isDeleted)
        {
            return await _context.Categories
                .Where(p => p.IsEnabled == isEnabled && p.IsDeleted == isDeleted)
                .ToListAsync();
        }

        public async Task<Category?> GetCategoryByIdAsync(int id)
        {
            return await _context.Categories
                .Where(c => c.Id == id)
                .Include(a => a.Articles)
                .FirstOrDefaultAsync();
                
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

        
    }
}
