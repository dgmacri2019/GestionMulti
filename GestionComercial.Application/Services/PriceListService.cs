using GestionComercial.Applications.Interfaces;
using GestionComercial.Domain.DTOs.PriceLists;
using GestionComercial.Domain.Entities.Stock;
using GestionComercial.Domain.Helpers;
using GestionComercial.Domain.Response;
using GestionComercial.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace GestionComercial.Applications.Services
{
    public class PriceListService : IPriceListService
    {
        private readonly AppDbContext _context;
        private readonly DBHelper _dBHelper;

        public PriceListService(AppDbContext context)
        {
            _context = context;
            _dBHelper = new DBHelper();

        }



        public async Task<GeneralResponse> DeleteAsync(int id)
        {
            PriceList? priceList = await _context.PriceLists.FindAsync(id);
            if (priceList != null)
            {
                _context.PriceLists.Remove(priceList);
                await _context.SaveChangesAsync();
            }
            return new GeneralResponse { Success = false, Message = "Lista de precios no encontrada" };
        }

        public async Task<IEnumerable<PriceList>> GetAllAsync(bool isEnabled, bool isDeleted)
        {
            return await _context.PriceLists
                 .Where(p => p.IsEnabled == isEnabled && p.IsDeleted == isDeleted)
                 .ToListAsync();


            //return ToPriceListViewModelList(priceLists);
        }

        public async Task<PriceListViewModel?> GetByIdAsync(int id, bool isEnabled, bool isDeleted)
        {

            if (id == 0)
                return new PriceListViewModel
                {
                    IsDeleted = false,
                    IsEnabled = true,
                    CreateDate = DateTime.Now,
                };

            PriceList? priceList = await _context.PriceLists
                .Where(a => a.Id == id && a.IsEnabled == isEnabled && a.IsDeleted == isDeleted)
                .FirstOrDefaultAsync();

            return priceList == null ? null : ConverterHelper.ToPriceListViewModel(priceList);
        }

        public async Task<IEnumerable<PriceListViewModel>> SearchToListAsync(string name, bool isEnabled, bool isDeleted)
        {
            List<PriceList> priceLists = string.IsNullOrEmpty(name) ?
               await _context.PriceLists
                .Where(p => p.IsEnabled == isEnabled && p.IsDeleted == isDeleted)
                .ToListAsync()
               :
               await _context.PriceLists
                .Where(p => p.IsEnabled == isEnabled && p.IsDeleted == isDeleted && (p.Description.Contains(name)))
                .ToListAsync();


            return ToPriceListViewModelList(priceLists);
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
