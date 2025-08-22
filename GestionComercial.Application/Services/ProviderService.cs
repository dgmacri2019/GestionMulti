using GestionComercial.Applications.Interfaces;
using GestionComercial.Domain.DTOs.Provider;
using GestionComercial.Domain.Entities.Afip;
using GestionComercial.Domain.Entities.Masters;
using GestionComercial.Domain.Helpers;
using GestionComercial.Domain.Response;
using GestionComercial.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

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
            List<Provider> providers = await _context.Providers
                 .Include(c => c.State)
                 .Include(ic => ic.IvaCondition)
                 .Include(dt => dt.DocumentType)
                 .Include(sc => sc.SaleCondition)
                 .Where(p => p.IsEnabled == isEnabled && p.IsDeleted == isDeleted)
                 .ToListAsync();


            return ToProviderViewModelList(providers);
        }

        public async Task<ProviderViewModel?> GetByIdAsync(int id, bool isEnabled, bool isDeleted)
        {
            // Incluimos las listas de precios; asegúrate de que la propiedad esté activa en Product           
            ICollection<State> states = await _context.States
                .Where(pl => pl.IsEnabled && !pl.IsDeleted)
                .OrderBy(s => s.Name)
                .ToListAsync();
            ICollection<IvaCondition> ivaConditions = await _context.IvaConditions
                .Where(pl => pl.IsEnabled && !pl.IsDeleted)
                .OrderBy(s => s.Description)
                .ToListAsync();
            ICollection<DocumentType> documentTypes = await _context.DocumentTypes
                .Where(pl => pl.IsEnabled && !pl.IsDeleted)
                .OrderBy(s => s.Description)
                .ToListAsync();
            ICollection<SaleCondition> saleConditions = await _context.SaleConditions
                .Where(sc => sc.IsEnabled && !sc.IsDeleted)
                .OrderBy(sc => sc.Description)
                .ToListAsync();

            saleConditions.Add(new SaleCondition { Id = 0, Description = "Seleccione la condición de venta" });
            states.Add(new State { Id = 0, Name = "Seleccione la provincia" });
            ivaConditions.Add(new IvaCondition { Id = 0, Description = "Seleccione la condición de IVA" });
            documentTypes.Add(new DocumentType { Id = 0, Description = "Seleccione el tipo de documento" });



            if (id == 0)
                return new ProviderViewModel
                {
                    SaleConditions = saleConditions,
                    IvaConditions = ivaConditions,
                    DocumentTypes = documentTypes,
                    States = states,
                    IsDeleted = false,
                    IsEnabled = true,
                    CreateDate = DateTime.Now,
                };


            Provider? provider = await _context.Providers
                .Include(s => s.State)
                .Include(ic => ic.IvaCondition)
                .Include(dt => dt.DocumentType)
                .Include(sc => sc.SaleCondition)
                .Where(a => a.Id == id && a.IsEnabled == isEnabled && a.IsDeleted == isDeleted)
                .FirstOrDefaultAsync();


            return provider == null ? null : ConverterHelper.ToProviderViewModel(provider, states,
                saleConditions, ivaConditions, documentTypes);
        }

        public async Task<IEnumerable<ProviderViewModel>> SearchToListAsync(string name, bool isEnabled, bool isDeleted)
        {
            // Incluimos las listas de precios; asegúrate de que la propiedad esté activa en Product

            List<Provider> providers = string.IsNullOrEmpty(name) ?
                await _context.Providers
                 .Include(c => c.State)
                 .Include(ic => ic.IvaCondition)
                 .Include(dt => dt.DocumentType)
                 .Include(sc => sc.SaleCondition)
                 .Where(p => p.IsEnabled == isEnabled && p.IsDeleted == isDeleted)
                 .ToListAsync()
                :
                await _context.Providers
                 .Include(s => s.State)
                 .Include(ic => ic.IvaCondition)
                 .Include(dt => dt.DocumentType)
                 .Include(sc => sc.SaleCondition)
                 .Where(p => p.IsEnabled == isEnabled && p.IsDeleted == isDeleted && (p.BusinessName.Contains(name) || p.FantasyName.Contains(name) || p.DocumentNumber.Contains(name)))
                 .ToListAsync();


            return ToProviderViewModelList(providers);
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
                DocumentTypeString = provider.DocumentType.Description,
                SaleConditionId = provider.SaleConditionId,
                IvaConditionString = provider.IvaCondition.Description,
                State = provider.State.Name,
                CreateDate = provider.CreateDate,
                CreateUser = provider.CreateUser,
                UpdateDate = provider.UpdateDate,
                UpdateUser = provider.UpdateUser,
                IsDeleted = provider.IsDeleted,
                IsEnabled = provider.IsEnabled,
            });
        }
    }
}
