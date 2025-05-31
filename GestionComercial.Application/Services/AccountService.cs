using GestionComercial.Applications.Interfaces;
using GestionComercial.Domain.DTOs.Accounts;
using GestionComercial.Domain.Entities.AccountingBook;
using GestionComercial.Domain.Helpers;
using GestionComercial.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace GestionComercial.Applications.Services
{
    public class AccountService : IAccountService
    {
        private readonly AppDbContext _context;
        private readonly DBHelper _dBHelper;

        #region Constructor
        public AccountService(AppDbContext context)
        {
            _context = context;
            _dBHelper = new DBHelper();
        }


        #endregion



        public async Task<IEnumerable<AccountViewModel>> GetAllAsync(bool isEnabled, bool isDeleted, bool all)
        {
            // Incluimos las listas de precios; asegúrate de que la propiedad esté activa en Product

            List<AccountType> accountTypes = await _context.AccountTypes.ToListAsync();

            List<Account> accounts = all ?
                await _context.Accounts
                //.Where(p => p.IsEnabled == isEnabled && p.IsDeleted == isDeleted)
                .OrderBy(c => c.AccountTypeId)
                .ThenBy(c => c.AccountSubGroupNumber1)
                .ThenBy(c => c.AccountSubGroupNumber2)
                .ThenBy(c => c.AccountSubGroupNumber3)
                .ThenBy(c => c.AccountSubGroupNumber4)
                .ThenBy(c => c.AccountSubGroupNumber5)
                .ToListAsync()
                :
                await _context.Accounts
                .Where(p => p.IsEnabled == isEnabled && p.IsDeleted == isDeleted)
                .OrderBy(c => c.AccountTypeId)
                .ThenBy(c => c.AccountSubGroupNumber1)
                .ThenBy(c => c.AccountSubGroupNumber2)
                .ThenBy(c => c.AccountSubGroupNumber3)
                .ThenBy(c => c.AccountSubGroupNumber4)
                .ThenBy(c => c.AccountSubGroupNumber5)
                .ToListAsync();


            return ToAccountViewModelList(accounts, accountTypes);
        }

        public async Task<AccountViewModel?> GetByIdAsync(int id)
        {
            if (id == 0)
                return new AccountViewModel
                {
                    IsDeleted = false,
                    IsEnabled = true,
                    CreateDate = DateTime.Now,
                };

            Account? account = await _context.Accounts
                .Where(a => a.Id == id)
                .FirstOrDefaultAsync();

            return account == null ? null : ConverterHelper.ToAccountViewModel(account);
        }

        public async Task<IEnumerable<AccountViewModel>> SearchToListAsync(string name, bool isEnabled, bool isDeleted)
        {
            // Incluimos las listas de precios; asegúrate de que la propiedad esté activa en Product

            List<AccountType> accountTypes = await _context.AccountTypes.ToListAsync();

            List<Account> accounts = string.IsNullOrEmpty(name) ?
              await _context.Accounts
               .Where(p => p.IsEnabled == isEnabled && p.IsDeleted == isDeleted)
               .OrderBy(c => c.AccountTypeId)
               .ThenBy(c => c.AccountSubGroupNumber1)
               .ThenBy(c => c.AccountSubGroupNumber2)
               .ThenBy(c => c.AccountSubGroupNumber3)
               .ThenBy(c => c.AccountSubGroupNumber4)
               .ThenBy(c => c.AccountSubGroupNumber5)
               .ToListAsync()
              :
              await _context.Accounts
               .Where(p => p.IsEnabled == isEnabled && p.IsDeleted == isDeleted && (p.Name.Contains(name)))
               .OrderBy(c => c.AccountTypeId)
               .ThenBy(c => c.AccountSubGroupNumber1)
               .ThenBy(c => c.AccountSubGroupNumber2)
               .ThenBy(c => c.AccountSubGroupNumber3)
               .ThenBy(c => c.AccountSubGroupNumber4)
               .ThenBy(c => c.AccountSubGroupNumber5)
               .ToListAsync();


            return ToAccountViewModelList(accounts, accountTypes);
        }

        public async Task<IEnumerable<AccountType>> GetAllAccountTypesAsync(bool isEnabled, bool isDeleted, bool all)
        {
            List<AccountType> accountTypes =
            [
                new AccountType
                {
                    Id = 0,
                    Name = "Seleccione el tipo de cuenta",
                },
            ];

            accountTypes.AddRange(all ?
                await _context.AccountTypes
                .ToListAsync()
                :
                await _context.AccountTypes
                .Where(ac => ac.IsEnabled == isEnabled && ac.IsDeleted == isDeleted)
                .ToListAsync());
            return accountTypes;
        }

        public async Task<IEnumerable<Account>> GetAllAccountsAsync(bool isEnabled, bool isDeleted, bool all)
        {
            List<Account> accounts =
            [
                new Account
                {
                    Id = 0,
                    Name = "Seleccione la cuenta",
                }
,
            ];
            accounts.AddRange(all ?
                await _context.Accounts
                .ToListAsync()
                :
                await _context.Accounts
                .Where(ac => ac.IsEnabled == isEnabled && ac.IsDeleted == isDeleted)
                .ToListAsync());
            return accounts;
        }

        public async Task<IEnumerable<Account>> GetAccountGroup1Async(int accountType, bool isEnabled, bool isDeleted, bool all)
        {
            List<Account> accounts =
            [
                new Account
                {
                    Id = 0,
                    Name = "Seleccione la SubCuenta 1",
                }
,
            ];
            accounts.AddRange(all ?
                await _context.Accounts
                .Where(a => a.AccountTypeId == accountType)
                .ToListAsync()
                :
                await _context.Accounts
                .Where(a => a.IsEnabled == isEnabled && a.IsDeleted == isDeleted && a.AccountTypeId == accountType)
                .ToListAsync());
            return accounts;
        }

        public async Task<IEnumerable<Account>> GetAccountGroup2Async(int accountType, int accountGroup1, bool isEnabled, bool isDeleted, bool all)
        {
            List<Account> accounts = 
            [
                new Account
                {
                    Id = 0,
                    Name = "Seleccione la SubCuenta 2",
                }
,
            ];
            accounts.AddRange(all ?
                await _context.Accounts
                .Where(a => a.AccountTypeId == accountType && a.AccountSubGroupNumber1 == accountGroup1)
                .ToListAsync()
                :
                await _context.Accounts
                .Where(a => a.IsEnabled == isEnabled && a.IsDeleted == isDeleted && a.AccountTypeId == accountType && a.AccountSubGroupNumber1 == accountGroup1)
                .ToListAsync());
            return accounts;
        }

        public async Task<IEnumerable<Account>> GetAccountGroup3Async(int accountType, int accountGroup1, int accountGroup2, bool isEnabled, bool isDeleted, bool all)
        {
            List<Account> accounts =
            [
                new Account
                {
                    Id = 0,
                    Name = "Seleccione la SubCuenta 3",
                }
,
            ];
            accounts.AddRange(all ?
                await _context.Accounts
                .Where(a => a.AccountTypeId == accountType && a.AccountSubGroupNumber1 == accountGroup1
                && a.AccountSubGroupNumber2 == accountGroup2)
                .ToListAsync()
                :
                await _context.Accounts
                .Where(a => a.IsEnabled == isEnabled && a.IsDeleted == isDeleted && a.AccountTypeId == accountType && a.AccountSubGroupNumber1 == accountGroup1
                && a.AccountSubGroupNumber2 == accountGroup2)
                .ToListAsync());
            return accounts;
        }

        public async Task<IEnumerable<Account>> GetAccountGroup4Async(int accountType, int accountGroup1, int accountGroup2, int accountGroup3, bool isEnabled, bool isDeleted,
            bool all)
        {
            List<Account> accounts =
            [
                new Account
                {
                    Id = 0,
                    Name = "Seleccione la SubCuenta 4",
                }
,
            ];
            accounts.AddRange(all ?
                await _context.Accounts
               .Where(a => a.AccountTypeId == accountType && a.AccountSubGroupNumber1 == accountGroup1
               && a.AccountSubGroupNumber2 == accountGroup2 && a.AccountSubGroupNumber3 == accountGroup3)
               .ToListAsync()
                :
                await _context.Accounts
               .Where(a => a.IsEnabled == isEnabled && a.IsDeleted == isDeleted && a.AccountTypeId == accountType && a.AccountSubGroupNumber1 == accountGroup1
               && a.AccountSubGroupNumber2 == accountGroup2 && a.AccountSubGroupNumber3 == accountGroup3)
               .ToListAsync());
            return accounts;
        }

        public async Task<IEnumerable<Account>> GetAccountGroup5Async(int accountType, int accountGroup1, int accountGroup2, int accountGroup3, int accountGroup4,
            bool isEnabled, bool isDeleted, bool all)
        {
            List<Account> accounts =
            [
                new Account
                {
                    Id = 0,
                    Name = "Seleccione la SubCuenta 5",
                }
,
            ];
            accounts.AddRange(all ?
                await _context.Accounts
              .Where(a => a.AccountTypeId == accountType && a.AccountSubGroupNumber1 == accountGroup1
              && a.AccountSubGroupNumber2 == accountGroup2 && a.AccountSubGroupNumber3 == accountGroup3 && a.AccountSubGroupNumber4 == accountGroup4)
              .ToListAsync()
                :
                await _context.Accounts
              .Where(a => a.IsEnabled == isEnabled && a.IsDeleted == isDeleted && a.AccountTypeId == accountType && a.AccountSubGroupNumber1 == accountGroup1
              && a.AccountSubGroupNumber2 == accountGroup2 && a.AccountSubGroupNumber3 == accountGroup3 && a.AccountSubGroupNumber4 == accountGroup4)
              .ToListAsync());
            return accounts;
        }












        private IEnumerable<AccountViewModel> ToAccountViewModelList(List<Account> accounts, List<AccountType> accountTypes)
        {
            return accountTypes.Select(group => new AccountViewModel
            {
                Id= group.Id,
                AccountTypeId = group.Id,
                Name = group.Name,
                Description = group.Name,
                IsFirstLevel = true,
                //AccountGroupNumber = group.Id,
                Children = accounts.Where(a => a.AccountTypeId == group.Id && a.AccountSubGroupNumber2 == 0 && a.AccountSubGroupNumber2 == 0 && a.AccountSubGroupNumber3 == 0
                && a.AccountSubGroupNumber4 == 0 && a.AccountSubGroupNumber5 == 0).Select(group1 => new AccountViewModel //grupo 1
                {
                    Id= group1.Id,
                    Name = group1.Name,
                    Description = group1.Description,
                    IsFirstLevel = false,
                    Children = accounts.Where(a => a.AccountTypeId == group.Id && a.AccountSubGroupNumber1 == group1.AccountSubGroupNumber1 && a.AccountSubGroupNumber2 != 0
                    && a.AccountSubGroupNumber3 == 0 && a.AccountSubGroupNumber4 == 0 && a.AccountSubGroupNumber5 == 0).Select(group2 => new AccountViewModel //grupo 2
                    {
                        Id= group2.Id,
                        Name = group2.Name,
                        Description = group2.Description,
                        IsFirstLevel = false,
                        Children = accounts.Where(a => a.AccountTypeId == group.Id && a.AccountSubGroupNumber1 == group1.AccountSubGroupNumber1
                        && a.AccountSubGroupNumber2 == group2.AccountSubGroupNumber2 && a.AccountSubGroupNumber3 != 0 && a.AccountSubGroupNumber4 == 0
                        && a.AccountSubGroupNumber5 == 0).Select(group3 => new AccountViewModel //grupo 3
                        {
                            Id = group3.Id,
                            Name = group3.Name,
                            Description = group3.Description,
                            IsFirstLevel = false,
                            Children = accounts
                            .Where(a => a.AccountTypeId == group.Id && a.AccountSubGroupNumber1 == group1.AccountSubGroupNumber1
                            && a.AccountSubGroupNumber2 == group2.AccountSubGroupNumber2 && a.AccountSubGroupNumber3 == group3.AccountSubGroupNumber3
                            && a.AccountSubGroupNumber4 != 0 && a.AccountSubGroupNumber5 == 0)
                            .Select(group4 => new AccountViewModel //grupo 4
                            {
                                Id = group4.Id,
                                Name = group4.Name,
                                Description = group4.Description,
                                IsFirstLevel = false,
                                Children = accounts
                                .Where(a => a.AccountTypeId == group.Id && a.AccountSubGroupNumber1 == group1.AccountSubGroupNumber1
                                && a.AccountSubGroupNumber2 == group2.AccountSubGroupNumber2 && a.AccountSubGroupNumber3 == group3.AccountSubGroupNumber3
                                && a.AccountSubGroupNumber4 == group4.AccountSubGroupNumber4 && a.AccountSubGroupNumber5 != 0)
                                .Select(group5 => new AccountViewModel
                                {
                                    Id = group5.Id,
                                    Name = group5.Name,
                                    Description = group5.Description,
                                    IsFirstLevel = false,
                                }).ToList(),
                            }).ToList(),
                        }).ToList(),
                    }).ToList(),
                }).ToList(),
            });
        }
    }
}
