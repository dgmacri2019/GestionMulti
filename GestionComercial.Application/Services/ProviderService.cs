using GestionComercial.Applications.Interfaces;
using GestionComercial.Domain.DTOs.Provider;
using GestionComercial.Domain.Entities.Masters;
using GestionComercial.Domain.Helpers;
using GestionComercial.Domain.Response;
using GestionComercial.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using static GestionComercial.Domain.Constant.Enumeration;

namespace GestionComercial.Applications.Services
{
    public class ProviderService : IProviderService
    {
        private readonly AppDbContext _context;
        private readonly DBHelper _dBHelper;

        public ProviderService(AppDbContext context)
        {
            _context = context;
            _dBHelper = new DBHelper();
        }


        public async Task<GeneralResponse> AddAsync(Provider provider)
        {
            _context.Providers.Add(provider);
            return await _dBHelper.SaveChangesAsync(_context);
        }

        public async Task<GeneralResponse> DeleteAsync(int id)
        {
            Provider? provider = await _context.Providers.FindAsync(id);
            if (provider != null)
            {
                _context.Providers.Remove(provider);
                await _context.SaveChangesAsync();
            }
            return new GeneralResponse { Success = false, Message = "Cliente no encontrado" };
        }

        public async Task<IEnumerable<ProviderViewModel>> GetAllAsync(bool isEnabled, bool isDeleted)
        {
            // Incluimos las listas de precios; asegúrate de que la propiedad esté activa en Product
            List<Provider> providers = await _context.Providers
                  .Include(c => c.State)
                  .Where(p => p.IsEnabled == isEnabled && p.IsDeleted == isDeleted)
                  .ToListAsync();


            return ToProviderViewModelList(providers);
        }

        public async Task<ProviderViewModel?> GetByIdAsync(int id, bool isEnabled, bool isDeleted)
        {
            // Incluimos las listas de precios; asegúrate de que la propiedad esté activa en Product           
            ICollection<State> states = await _context.States
                .Where(pl => pl.IsEnabled && !pl.IsDeleted)
                .ToListAsync();

            states.Add(new State { Id = 0, Name = "Seleccione la provincia" });

            ObservableCollection<SaleCondition> saleConditions = [.. (SaleCondition[])Enum.GetValues(typeof(SaleCondition))];

            ObservableCollection<TaxCondition> taxConditions = [.. (TaxCondition[])Enum.GetValues(typeof(TaxCondition))];

            ObservableCollection<DocumentType> documentTypes = [.. (DocumentType[])Enum.GetValues(typeof(DocumentType))];

            if (id == 0)
                return new ProviderViewModel
                {
                    SaleConditions = saleConditions,
                    TaxConditions = taxConditions,
                    DocumentTypes = documentTypes,
                    States = states,
                    IsDeleted = false,
                    IsEnabled = true,
                    CreateDate = DateTime.Now,
                };


            Provider? provider = await _context.Providers
                .Include(s => s.State)
                .Where(a => a.Id == id && a.IsEnabled == isEnabled && a.IsDeleted == isDeleted)
                .FirstOrDefaultAsync();

            states.Add(new State { Id = 0, Name = "Seleccione la provincia" });

            return provider == null ? null : ConverterHelper.ToProviderViewModel(provider, states,
                saleConditions, taxConditions, documentTypes);
        }

        public async Task<IEnumerable<ProviderViewModel>> SearchToListAsync(string name, bool isEnabled, bool isDeleted)
        {
            // Incluimos las listas de precios; asegúrate de que la propiedad esté activa en Product

            List<Provider> providers = string.IsNullOrEmpty(name) ?
                await _context.Providers
                 .Include(c => c.State)
                 .Where(p => p.IsEnabled == isEnabled && p.IsDeleted == isDeleted)
                 .ToListAsync()
                :
                await _context.Providers
                 .Include(s => s.State)
                 .Where(p => p.IsEnabled == isEnabled && p.IsDeleted == isDeleted && (p.BusinessName.Contains(name) || p.FantasyName.Contains(name) || p.DocumentNumber.Contains(name)))
                 .ToListAsync();


            return ToProviderViewModelList(providers);
        }

        public async Task<GeneralResponse> UpdateAsync(Provider provider)
        {
            _context.Providers.Update(provider);
            return await _dBHelper.SaveChangesAsync(_context);
        }





        private IEnumerable<ProviderViewModel> ToProviderViewModelList(List<Provider> providers)
        {
            return providers.Select(provider => new ProviderViewModel
            {
                Id = provider.Id,
                BusinessName = provider.BusinessName,
                FantasyName = provider.FantasyName,
                DocumentNumber = provider.DocumentNumber,
                Address = provider.Address,
                PostalCode = provider.PostalCode,
                City = provider.City,
                StateId = provider.StateId,
                Phone = provider.Phone,
                Phone1 = provider.Phone1,
                Phone2 = provider.Phone2,
                Email = provider.Email,
                WebSite = provider.WebSite,
                Remark = provider.Remark,
                PayDay = provider.PayDay,
                LastPuchase = provider.LastPuchase,
                Sold = provider.Sold,
                DocumentType = provider.DocumentType,
                SaleCondition = provider.SaleCondition,
                TaxCondition = provider.TaxCondition,
                State = provider.State.Name,
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
