using GestionComercial.Applications.Interfaces;
using GestionComercial.Domain.DTOs.Client;
using GestionComercial.Domain.DTOs.Stock;
using GestionComercial.Domain.Entities.Afip;
using GestionComercial.Domain.Entities.Masters;
using GestionComercial.Domain.Entities.Stock;
using GestionComercial.Domain.Helpers;
using GestionComercial.Domain.Response;
using GestionComercial.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Xml.Linq;

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
        /*
        public async Task<ClientResponse> GetAllAsync(int page, int pageSize)
        {
            try
            {
                // Incluimos las listas de precios; asegúrate de que la propiedad esté activa en Product
                ICollection<PriceList> priceLists = await _context.PriceLists
                  .Where(pl => pl.IsEnabled && !pl.IsDeleted)
                  .ToListAsync();
                // orden obligatorio para paginación consistente

                List<IGrouping<string, Client>> clients = await _context.Clients
                     .Include(c => c.PriceList)
                     .Include(c => c.State)
                     .Include(ic => ic.IvaCondition)
                     .Include(dt => dt.DocumentType)
                     .OrderBy(c => c.Id)
                     .ThenBy(c => c.BusinessName)
                     .Skip((page - 1) * pageSize)
                     .Take(pageSize)
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

                ICollection<SaleCondition> saleConditions = await _context.SaleConditions
                    .Where(sc => sc.IsEnabled && !sc.IsDeleted)
                    .OrderBy(sc => sc.Description)
                    .ToListAsync();

                saleConditions.Add(new SaleCondition { Id = 0, Description = "Seleccione la condición de venta" });
                states.Add(new State { Id = 0, Name = "Seleccione la provincia" });
                priceLists.Add(new PriceList { Id = 0, Description = "Seleccione la lista de precios" });
                ivaConditions.Add(new IvaCondition { Id = 0, Description = "Seleccione la condición de IVA" });
                documentTypes.Add(new DocumentType { Id = 0, Description = "Seleccione el tipo de documento" });

                return new ClientResponse
                {
                    Success = true,
                    ClientViewModels = ToClientViewModelAndPriceList(clients, priceLists, documentTypes, saleConditions, states, ivaConditions),
                    TotalRegisters = clients.Count
                };
            }
            catch (Exception ex)
            {
                return new ClientResponse
                {
                    Success = false,
                    Message = ex.Message,
                };
            }
        }
        */

        public async Task<ClientViewModel?> GetByIdAsync(int id)
        {
            // Incluimos las listas de precios; asegúrate de que la propiedad esté activa en Product
            ICollection<PriceList> priceLists = await _context.PriceLists
                .Where(pl => pl.IsEnabled && !pl.IsDeleted)
                .OrderBy(pl => pl.Description)
                .ToListAsync();
            ICollection<State> states = await _context.States
                .Where(pl => pl.IsEnabled && !pl.IsDeleted)
                .OrderBy(s => s.Name)
                .ToListAsync();

            ICollection<DocumentType> documentTypes = await _context.DocumentTypes
                .Where(pl => pl.IsEnabled && !pl.IsDeleted)
                .OrderBy(s => s.Description)
                .ToListAsync();

            ICollection<IvaCondition> ivaConditions = await _context.IvaConditions
                .Where(pl => pl.IsEnabled && !pl.IsDeleted)
                .OrderBy(s => s.Description)
                .ToListAsync();

            ICollection<SaleCondition> saleConditions = await _context.SaleConditions
                .Where(pl => pl.IsEnabled && !pl.IsDeleted)
                .OrderBy(s => s.Description)
                .ToListAsync();

            states.Add(new State { Id = 0, Name = "Seleccione la provincia" });
            priceLists.Add(new PriceList { Id = 0, Description = "Seleccione la lista de precios" });
            documentTypes.Add(new DocumentType { Id = 0, Description = "Seleccione el tipo de documento" });
            ivaConditions.Add(new IvaCondition { Id = 0, Description = "Seleccione la condición de IVA" });
            saleConditions.Add(new SaleCondition { Id = 0, Description = "Seleccione la condición de venta" });

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


        public async Task<ClientResponse> GetAllAsync(int page, int pageSize)
        {
            try
            {
                var priceLists = await _context.PriceLists
      .AsNoTracking()
      .Where(pl => pl.IsEnabled && !pl.IsDeleted)
      .ToListAsync();

                var states = await _context.States
                    .AsNoTracking()
                    .Where(s => s.IsEnabled && !s.IsDeleted)
                    .ToListAsync();

                var ivaConditions = await _context.IvaConditions
                    .AsNoTracking()
                    .Where(i => i.IsEnabled && !i.IsDeleted)
                    .ToListAsync();

                var documentTypes = await _context.DocumentTypes
                    .AsNoTracking()
                    .Where(d => d.IsEnabled && !d.IsDeleted)
                    .ToListAsync();

                var saleConditions = await _context.SaleConditions
                    .AsNoTracking()
                    .Where(sc => sc.IsEnabled && !sc.IsDeleted)
                    .OrderBy(sc => sc.Description)
                    .ToListAsync();

                var clients = await _context.Clients
                    .AsNoTracking()
                    .Include(c => c.PriceList)
                    .Include(c => c.State)
                    .Include(c => c.IvaCondition)
                    .Include(c => c.DocumentType)
                    .OrderBy(c => c.PriceList.Description)
                    .ThenBy(c => c.Id)
                    .ThenBy(c => c.BusinessName)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                var totalRegisters = await _context.Clients.AsNoTracking().CountAsync();

               

                saleConditions.Add(new SaleCondition { Id = 0, Description = "Seleccione la condición de venta" });
                states.Add(new State { Id = 0, Name = "Seleccione la provincia" });
                priceLists.Add(new PriceList { Id = 0, Description = "Seleccione la lista de precios" });
                ivaConditions.Add(new IvaCondition { Id = 0, Description = "Seleccione la condición de IVA" });
                documentTypes.Add(new DocumentType { Id = 0, Description = "Seleccione el tipo de documento" });

                

                // mapeo a ViewModel
                var clientViewModels = clients.Select(client => new ClientViewModel
                {
                    Id = client.Id,
                    BusinessName = client.BusinessName,
                    FantasyName = client.FantasyName,
                    OptionalCode = client.OptionalCode,
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
                    PayDay = client.PayDay,
                    LegendInvoices = client.LegendInvoices,
                    LastPuchase = client.LastPuchase,
                    LegendBudget = client.LegendBudget,
                    LegendOrder = client.LegendOrder,
                    LegendRemit = client.LegendRemit,
                    Sold = client.Sold,
                    PriceListId = client.PriceListId,
                    DocumentTypeId = client.DocumentTypeId,
                    CreditLimit = client.CreditLimit,
                    IvaConditionId = client.IvaConditionId,
                    State = client.State.Name,
                    PriceList = client.PriceList.Description,
                    CreateDate = client.CreateDate,
                    CreateUser = client.CreateUser,
                    UpdateDate = client.UpdateDate,
                    UpdateUser = client.UpdateUser,
                    IsDeleted = client.IsDeleted,
                    IsEnabled = client.IsEnabled,

                    PriceListsDTO = priceLists.Select(pl => new PriceListItemDto
                    {
                        Description = pl.Description,
                        Utility = pl.Utility,
                    })
                    .OrderBy(pl => pl.Utility)
                    .ToList()
                }).ToList();

                return new ClientResponse
                {
                    Success = true,
                    ClientViewModels = clientViewModels,
                    PriceLists = priceLists,
                    States = states,
                    IvaConditions = ivaConditions,
                    DocumentTypes = documentTypes,
                    SaleConditions = saleConditions,
                    TotalRegisters = totalRegisters
                };
            }
            catch (Exception ex)
            {
                return new ClientResponse
                {
                    Success = false,
                    Message = ex.Message,
                };
            }
        }



        private IEnumerable<ClientViewModel> ToClientViewModelAndPriceList(List<IGrouping<string, Client>> clients,
            ICollection<PriceList> priceLists, ICollection<DocumentType> documentTypes, ICollection<SaleCondition> saleConditions,
            ICollection<State> states, ICollection<IvaCondition> ivaConditions)
        {
            return clients.SelectMany(group => group.Select(client => new ClientViewModel
            {
                Id = client.Id,
                BusinessName = client.BusinessName,
                FantasyName = client.FantasyName,
                OptionalCode = client.OptionalCode,
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
                PayDay = client.PayDay,
                LegendInvoices = client.LegendInvoices,
                LastPuchase = client.LastPuchase,
                LegendBudget = client.LegendBudget,
                LegendOrder = client.LegendOrder,
                LegendRemit = client.LegendRemit,
                Sold = client.Sold,
                PriceListId = client.PriceListId,
                DocumentTypeId = client.DocumentTypeId,
                //SaleConditionId = client.SaleConditionId,
                //SaleConditionString = client.SaleCondition.Description,
                CreditLimit = client.CreditLimit,
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
