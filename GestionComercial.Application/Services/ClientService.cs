using GestionComercial.Applications.Interfaces;
using GestionComercial.Domain.DTOs.Client;
using GestionComercial.Domain.Entities.Masters;
using GestionComercial.Domain.Helpers;
using GestionComercial.Domain.Response;
using GestionComercial.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

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

        public async Task<ClientViewModel?> GetByIdAsync(int id)
        {
            Client? client = await _context.Clients
                .AsNoTracking()
                .Include(p => p.PriceList)
                .Include(s => s.State)
                .Include(dt => dt.DocumentType)
                .Include(ic => ic.IvaCondition)
                .Where(a => a.Id == id)
                .FirstOrDefaultAsync();

            return id == 0 || client == null ?
                new ClientViewModel
                {
                    IsDeleted = false,
                    IsEnabled = true,
                    CreateDate = DateTime.Now,
                }
                :
                ConverterHelper.ToClientViewModel(client);
        }
        public async Task<ClientResponse> GetAllAsync(int page, int pageSize)
        {
            try
            {
                List<Client> clients = await _context.Clients
                    .AsNoTracking()
                    .Include(c => c.PriceList)
                    .Include(c => c.State)
                    .Include(c => c.IvaCondition)
                    .Include(c => c.DocumentType)
                    .OrderBy(c => c.BusinessName)
                    .ThenBy(c => c.FantasyName ?? "")
                    .ThenBy(c => c.OptionalCode ?? "")
                    .ThenBy(c => c.PriceList.Description ?? "")
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                var totalRegisters = await _context.Clients.AsNoTracking().CountAsync();

                return new ClientResponse
                {
                    Success = true,
                    ClientViewModels = ToClientViewModelLists(clients),
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




        private List<ClientViewModel> ToClientViewModelLists(List<Client> clients)
        {
            return clients
                .Select(client => new ClientViewModel
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
                    //SaleConditionId = client.SaleConditionId,
                    //SaleConditionString = client.SaleCondition.Description,
                    //DocumentTypes = documentTypes,
                    //PriceLists = priceLists,
                    //SaleConditions = saleConditions,
                    //States = states,
                    //IvaConditions = ivaConditions,
                })
                .OrderBy(c => c.BusinessName)
                .ThenBy(c => c.FantasyName ?? "")
                .ThenBy(c => c.OptionalCode ?? "")
                .ThenBy(c => c.PriceList ?? "")
                .ToList();
        }
    }
}
