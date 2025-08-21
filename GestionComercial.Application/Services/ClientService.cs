using GestionComercial.Applications.Interfaces;
using GestionComercial.Domain.Constant;
using GestionComercial.Domain.DTOs.Client;
using GestionComercial.Domain.DTOs.Stock;
using GestionComercial.Domain.Entities.Afip;
using GestionComercial.Domain.Entities.Masters;
using GestionComercial.Domain.Helpers;
using GestionComercial.Domain.Response;
using GestionComercial.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using static GestionComercial.Domain.Constant.Enumeration;

namespace GestionComercial.Applications.Services
{
    public class ClientService : IClientService
    {
        private readonly AppDbContext _context;
        private readonly DBHelper _dBHelper;


        public ClientService(AppDbContext context)
        {
            _context = context;
            _dBHelper = new DBHelper();
        }


        public async Task<GeneralResponse> DeleteAsync(int id)
        {
            Client? client = await _context.Clients.FindAsync(id);
            if (client != null)
            {
                _context.Clients.Remove(client);
                await _context.SaveChangesAsync();
            }
            return new GeneralResponse { Success = false, Message = "Cliente no encontrado" };
        }

        public async Task<IEnumerable<ClientViewModel>> GetAllAsync()
        {
            // Incluimos las listas de precios; asegúrate de que la propiedad esté activa en Product
            ICollection<PriceList> priceLists = await _context.PriceLists
              .Where(pl => pl.IsEnabled && !pl.IsDeleted)
              .ToListAsync();

            List<IGrouping<string, Client>> clients = await _context.Clients
                 .Include(c => c.PriceList)
                 .Include(c => c.State)
                 .Include(ic => ic.IvaCondition)
                 .Include(dt => dt.DocumentType)
                 //.Where(p => p.IsEnabled == isEnabled && p.IsDeleted == isDeleted)
                 .GroupBy(c => c.PriceList.Description)
                 .ToListAsync();

            ICollection<State> states = await _context.States
               .Where(pl => pl.IsEnabled && !pl.IsDeleted)
               .ToListAsync();

            ICollection<IvaCondition> ivaConditions = await _context.IvaConditions
               .Where(pl => pl.IsEnabled && !pl.IsDeleted)
               .ToListAsync();

            ICollection<DocumentType> documentTypes = await _context.DocumentTypes
               .Where(pl => pl.IsEnabled && !pl.IsDeleted)
               .ToListAsync();


            states.Add(new State { Id = 0, Name = "Seleccione la provincia" });
            priceLists.Add(new PriceList { Id = 0, Description = "Seleccione la lista de precios" });
            ivaConditions.Add(new IvaCondition { Id = 0, Description = "Seleccione la condición de IVA" });
            documentTypes.Add(new DocumentType { Id = 0, Description = "Seleccione el tipo de documento" });
            ObservableCollection<SaleCondition> saleConditions = [.. (SaleCondition[])Enum.GetValues(typeof(SaleCondition))];

            //ObservableCollection<TaxCondition> taxConditions = [.. (TaxCondition[])Enum.GetValues(typeof(TaxCondition))];

            //ObservableCollection<DocumentType> documentTypes = [.. (DocumentType[])Enum.GetValues(typeof(DocumentType))];
            return ToClientViewModelAndPriceList(clients, priceLists, documentTypes, saleConditions, states, ivaConditions);
        }


        public async Task<ClientViewModel?> GetByIdAsync(int id)
        {
            // Incluimos las listas de precios; asegúrate de que la propiedad esté activa en Product
            ICollection<PriceList> priceLists = await _context.PriceLists
                .Where(pl => pl.IsEnabled && !pl.IsDeleted)
                .ToListAsync();
            ICollection<State> states = await _context.States
                .Where(pl => pl.IsEnabled && !pl.IsDeleted)
                .ToListAsync();

            ICollection<DocumentType> documentTypes = await _context.DocumentTypes
                .Where(pl => pl.IsEnabled && !pl.IsDeleted)
                .ToListAsync();

            ICollection<IvaCondition> ivaConditions = await _context.IvaConditions
                .Where(pl => pl.IsEnabled && !pl.IsDeleted)
                .ToListAsync();

            states.Add(new State { Id = 0, Name = "Seleccione la provincia" });
            priceLists.Add(new PriceList { Id = 0, Description = "Seleccione la lista de precios" });
            documentTypes.Add(new DocumentType { Id = 0, Description = "Seleccione el tipo de documento" });
            ivaConditions.Add(new IvaCondition { Id = 0, Description = "Seleccione la condición de IVA" });

            ObservableCollection<SaleCondition> saleConditions = [.. (SaleCondition[])Enum.GetValues(typeof(SaleCondition))];

            //ObservableCollection<TaxCondition> taxConditions = [.. (TaxCondition[])Enum.GetValues(typeof(TaxCondition))];

            //ObservableCollection<DocumentType> documentTypes = [.. (DocumentType[])Enum.GetValues(typeof(DocumentType))];

            if (id == 0)
                return new ClientViewModel
                {
                    SaleConditions = saleConditions,
                    IvaConditions = ivaConditions,
                    DocumentTypes = documentTypes,
                    States = states,
                    PriceLists = priceLists,
                    IsDeleted = false,
                    IsEnabled = true,
                    CreateDate = DateTime.Now,
                };


            Client? client = await _context.Clients
                .Include(p => p.PriceList)
                .Include(s => s.State)
                .Include(dt => dt.DocumentType)
                .Include(ic => ic.IvaCondition)
                .Where(a => a.Id == id)
                .FirstOrDefaultAsync();

            priceLists.Add(new PriceList { Id = 0, Description = "Seleccione la lista de precios" });
            states.Add(new State { Id = 0, Name = "Seleccione la provincia" });

            return client == null ? null : ConverterHelper.ToClientViewModel(client, priceLists, states,
                saleConditions, ivaConditions, documentTypes);
        }
        /*
       public async Task<IEnumerable<ClientViewModel>> SearchToListAsync(string name, bool isEnabled, bool isDeleted)
       {
           // Incluimos las listas de precios; asegúrate de que la propiedad esté activa en Product
           ICollection<PriceList> priceLists = await _context.PriceLists
             .Where(pl => pl.IsEnabled && !pl.IsDeleted)
             .ToListAsync();

           List<IGrouping<string, Client>> clients = string.IsNullOrEmpty(name) ?
               await _context.Clients
                .Include(c => c.PriceList)
                .Include(c => c.State)
                .Where(p => p.IsEnabled == isEnabled && p.IsDeleted == isDeleted)
                .GroupBy(c => c.PriceList.Description)
                .ToListAsync()
               :
               await _context.Clients
                .Include(c => c.PriceList)
                .Include(s => s.State)
                .Where(p => p.IsEnabled == isEnabled && p.IsDeleted == isDeleted && (p.BusinessName.Contains(name) || p.FantasyName.Contains(name) || p.DocumentNumber.Contains(name)))
                .GroupBy(c => c.PriceList.Description)
                .ToListAsync();


           return ToClientViewModelAndPriceList(clients, priceLists);
       }

     */



        private IEnumerable<ClientViewModel> ToClientViewModelAndPriceList(List<IGrouping<string, Client>> clients, ICollection<PriceList> priceLists, ICollection<DocumentType> documentTypes,
            ObservableCollection<SaleCondition> saleConditions, ICollection<State> states, ICollection<IvaCondition> ivaConditions)
        {
            return clients.SelectMany(group => group.Select(client => new ClientViewModel
            {
                Id = client.Id,
                BusinessName = client.BusinessName,
                FantasyName = client.FantasyName,
                DocumentTypeString = client.DocumentType.Description,
                DocumentNumber = client.DocumentNumber,
                IvaConditionString = client.IvaCondition.Description,
                Address = client.Address,
                PostalCode = client.PostalCode,
                City = client.City,
                StateId = client.StateId,
                Phone = client.Phone,
                Phone1 = client.Phone1,
                Phone2 = client.Phone2,
                Email = client.Email,
                WebSite = client.WebSite,
                Remark = client.Remark,
                SaleConditionString = EnumExtensionService.GetDisplayName(client.SaleCondition),
                PayDay = client.PayDay,
                LegendInvoices = client.LegendInvoices,
                LastPuchase = client.LastPuchase,
                LegendBudget = client.LegendBudget,
                LegendOrder = client.LegendOrder,
                LegendRemit = client.LegendRemit,
                Sold = client.Sold,
                PriceListId = client.PriceListId,
                DocumentTypeId = client.DocumentTypeId,
                SaleCondition = client.SaleCondition,
                IvaConditionId = client.IvaConditionId,
                State = client.State.Name,
                PriceList = client.PriceList.Description,
                CreateDate = client.CreateDate,
                CreateUser = client.CreateUser,
                UpdateDate = client.UpdateDate,
                UpdateUser = client.UpdateUser,
                IsDeleted = client.IsDeleted,
                IsEnabled = client.IsEnabled,
                DocumentTypes = documentTypes,
                PriceLists = priceLists,
                SaleConditions = saleConditions,
                States = states,
                IvaConditions = ivaConditions,


                PriceListsDTO = priceLists.Select(pl => new PriceListItemDto
                {
                    Description = pl.Description,
                    Utility = pl.Utility,
                })
                .OrderBy(pl => pl.Utility)
                .ToList()// Ordenamos para que la lista 1 (utility=0) aparezca primero, luego las que ofrecen descuentos
            }));
        }
    }
}
