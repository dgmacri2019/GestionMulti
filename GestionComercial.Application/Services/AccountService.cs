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
                .OrderBy(c => c.AccountGroupNumber)
                .ThenBy(c => c.AccountSubGroupNumber1)
                .ThenBy(c => c.AccountSubGroupNumber2)
                .ThenBy(c => c.AccountSubGroupNumber3)
                .ThenBy(c => c.AccountSubGroupNumber4)
                .ThenBy(c => c.AccountSubGroupNumber5)
                .ToListAsync()
                :
                await _context.Accounts
                .Where(p => p.IsEnabled == isEnabled && p.IsDeleted == isDeleted)
                .OrderBy(c => c.AccountGroupNumber)
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
               .OrderBy(c => c.AccountGroupNumber)
               .ThenBy(c => c.AccountSubGroupNumber1)
               .ThenBy(c => c.AccountSubGroupNumber2)
               .ThenBy(c => c.AccountSubGroupNumber3)
               .ThenBy(c => c.AccountSubGroupNumber4)
               .ThenBy(c => c.AccountSubGroupNumber5)
               .ToListAsync()
              :
              await _context.Accounts
               .Where(p => p.IsEnabled == isEnabled && p.IsDeleted == isDeleted && (p.Name.Contains(name)))
               .OrderBy(c => c.AccountGroupNumber)
               .ThenBy(c => c.AccountSubGroupNumber1)
               .ThenBy(c => c.AccountSubGroupNumber2)
               .ThenBy(c => c.AccountSubGroupNumber3)
               .ThenBy(c => c.AccountSubGroupNumber4)
               .ThenBy(c => c.AccountSubGroupNumber5)
               .ToListAsync();


            return ToAccountViewModelList(accounts, accountTypes);
        }



        private IEnumerable<AccountViewModel> ToAccountViewModelList(List<Account> accounts, List<AccountType> accountTypes)
        {

            IEnumerable<AccountViewModel> raiz = new List<AccountViewModel>();
            var x = accountTypes.Select(group => new AccountViewModel
            {
                AccountTypeId = group.Id,
                Name = group.Name,
                Description = group.Name,
                IsFirstLevel = true,
                AccountGroupNumber = group.Id,
                Children = accounts.Where(a => a.AccountTypeId == group.Id && a.AccountSubGroupNumber2 == 0 && a.AccountSubGroupNumber2 == 0 && a.AccountSubGroupNumber3 == 0
                && a.AccountSubGroupNumber4 == 0 && a.AccountSubGroupNumber5 == 0).Select(group1 => new AccountViewModel //grupo 1
                {
                    Name = group1.Name,
                    Description = group1.Description,
                    IsFirstLevel = false,
                    Children = accounts.Where(a => a.AccountTypeId == group.Id && a.AccountSubGroupNumber1 == group1.AccountSubGroupNumber1 && a.AccountSubGroupNumber2 != 0
                    && a.AccountSubGroupNumber3 == 0 && a.AccountSubGroupNumber4 == 0 && a.AccountSubGroupNumber5 == 0).Select(group2 => new AccountViewModel //grupo 2
                    {
                        Name = group2.Name,
                        Description = group2.Description,
                        IsFirstLevel = false,
                        Children = accounts.Where(a => a.AccountTypeId == group.Id && a.AccountSubGroupNumber1 == group1.AccountSubGroupNumber1
                        && a.AccountSubGroupNumber2 == group2.AccountSubGroupNumber2 && a.AccountSubGroupNumber3 != 0 && a.AccountSubGroupNumber4 == 0
                        && a.AccountSubGroupNumber5 == 0).Select(group3 => new AccountViewModel //grupo 3
                        {
                            Name = group3.Name,
                            Description = group3.Description,
                            IsFirstLevel = false,
                            Children = accounts
                            .Where(a => a.AccountTypeId == group.Id && a.AccountSubGroupNumber1 == group1.AccountSubGroupNumber1
                            && a.AccountSubGroupNumber2 == group2.AccountSubGroupNumber2 && a.AccountSubGroupNumber3 == group3.AccountSubGroupNumber3
                            && a.AccountSubGroupNumber4 != 0 && a.AccountSubGroupNumber5 == 0)
                            .Select(group4 => new AccountViewModel //grupo 4
                            {
                                Name = group4.Name,
                                Description = group4.Description,
                                IsFirstLevel = false,
                                Children = accounts
                                .Where(a => a.AccountTypeId == group.Id && a.AccountSubGroupNumber1 == group1.AccountSubGroupNumber1
                                && a.AccountSubGroupNumber2 == group2.AccountSubGroupNumber2 && a.AccountSubGroupNumber3 == group3.AccountSubGroupNumber3
                                && a.AccountSubGroupNumber4 == group4.AccountSubGroupNumber4 && a.AccountSubGroupNumber5 != 0)
                                .Select(group5 => new AccountViewModel
                                {
                                    Name = group5.Name,
                                    Description = group5.Description,
                                    IsFirstLevel = false,
                                }).ToList(),
                            }).ToList(),
                        }).ToList(),
                    }).ToList(),
                }).ToList(),
            });

            return x;
            //foreach (var cuenta in accounts)
            //{
            //    // Nivel 0 - Grupo principal
            //    var grupo = raiz.FirstOrDefault(x => x.Name == cuenta.AccountGroupNumber.ToString());
            //    //var grupo = raiz.FirstOrDefault(x => x.Name == cuenta.Name);
            //    if (grupo == null)
            //    {
            //        grupo = new AccountViewModel
            //        {
            //            Name = cuenta.Name,
            //            Description = cuenta.Description,
            //            IsFirstLevel = true
            //        };
            //        raiz.Add(grupo);
            //    }

            //    // Nivel 1
            //    var sub1 = grupo.Children.FirstOrDefault(x => x.Name == cuenta.AccountSubGroupNumber1.ToString());
            //    if (sub1 == null)
            //    {
            //        sub1 = new AccountViewModel
            //        {
            //            Name = cuenta.Name,
            //            Description = cuenta.Description
            //        };
            //        grupo.Children.Add(sub1);
            //    }

            //    // Nivel 2
            //    var sub2 = sub1.Children.FirstOrDefault(x => x.Name == cuenta.AccountSubGroupNumber2.ToString());
            //    if (sub2 == null)
            //    {
            //        sub2 = new AccountViewModel
            //        {
            //            Name = cuenta.Name,
            //            Description = cuenta.Description
            //        };
            //        sub1.Children.Add(sub2);
            //    }

            //    // Nivel 3
            //    var sub3 = sub2.Children.FirstOrDefault(x => x.Name == cuenta.AccountSubGroupNumber3.ToString());
            //    if (sub3 == null)
            //    {
            //        sub3 = new AccountViewModel
            //        {
            //            Name = cuenta.Name,
            //            Description = cuenta.Description
            //        };
            //        sub2.Children.Add(sub3);
            //    }

            //    // Nivel 4
            //    var sub4 = sub3.Children.FirstOrDefault(x => x.Name == cuenta.AccountSubGroupNumber4.ToString());
            //    if (sub4 == null)
            //    {
            //        sub4 = new AccountViewModel
            //        {
            //            Name = cuenta.Name,
            //            Description = cuenta.Description
            //        };
            //        sub3.Children.Add(sub4);
            //    }

            //    // Nivel 5 (Cuenta final)
            //    var sub5 = sub4.Children.FirstOrDefault(x => x.Name == cuenta.AccountSubGroupNumber5.ToString());
            //    if (sub5 == null)
            //    {
            //        sub5 = new AccountViewModel
            //        {
            //            Name = cuenta.Name, // Mostramos el nombre completo de la cuenta final
            //            Description = cuenta.Description
            //        };
            //        sub4.Children.Add(sub5);
            //    }
            //}

            return raiz;






            return accounts.Select(account => new AccountViewModel
            {
                Id = account.Id,
                Description = account.Description,
                Name = account.Name,
                CreateDate = account.CreateDate,
                CreateUser = account.CreateUser,
                UpdateDate = account.UpdateDate,
                UpdateUser = account.UpdateUser,
                IsDeleted = account.IsDeleted,
                IsEnabled = account.IsEnabled,
                AccountGroupNumber = account.AccountGroupNumber,
                AccountSubGroupNumber1 = account.AccountSubGroupNumber1,
                AccountSubGroupNumber2 = account.AccountSubGroupNumber2,
                AccountSubGroupNumber3 = account.AccountSubGroupNumber3,
                AccountSubGroupNumber4 = account.AccountSubGroupNumber4,
                AccountSubGroupNumber5 = account.AccountSubGroupNumber5,
                AccountTypeId = account.AccountTypeId,
                IsReference = account.IsReference,
                IsFirstLevel = account.AccountSubGroupNumber1 == 0 && account.AccountSubGroupNumber2 == 0 && account.AccountSubGroupNumber3 == 0 &&
                account.AccountSubGroupNumber4 == 0 && account.AccountSubGroupNumber5 == 0,
            });
        }
    }
}
