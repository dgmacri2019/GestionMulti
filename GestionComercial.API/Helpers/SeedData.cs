using ExcelDataReader;
using GestionComercial.Domain.Constant;
using GestionComercial.Domain.Entities.AccountingBook;
using GestionComercial.Domain.Entities.Afip;
using GestionComercial.Domain.Entities.BoxAndBank;
using GestionComercial.Domain.Entities.Masters;
using GestionComercial.Domain.Entities.Masters.Security;
using GestionComercial.Domain.Entities.Stock;
using GestionComercial.Domain.Response;
using GestionComercial.Domain.Statics;
using GestionComercial.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Data;
using System.Reflection.Metadata.Ecma335;
using static GestionComercial.Domain.Constant.Enumeration;

namespace GestionComercial.API.Helpers
{
    public static class SeedData
    {
        public static async Task<GeneralResponse> InitializeAsync(IServiceProvider serviceProvider)
        {
            try
            {
                using var scope = serviceProvider.CreateScope();
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
                //var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                GeneralResponse resultResponse = new GeneralResponse { Success = false };

                string developerEmail = "macri.diego@gmail.com";
                string adminEmail = "admin@admin.com";
                IdentityUser developerUser = await userManager.FindByEmailAsync(developerEmail);
                IdentityUser adminUser = await userManager.FindByEmailAsync(adminEmail);

                if (developerUser == null)
                {
                    User user = new()
                    {
                        UserName = "dgmacri",
                        Email = developerEmail,
                        EmailConfirmed = true,
                        LastName = "MACRI",
                        FirstName = "Diego Gaston",
                        Enabled = true
                    };

                    IdentityResult result = await userManager.CreateAsync(user, "@Diego248");
                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(user, "DEVELOPER");
                    }
                }

                if (adminUser == null)
                {
                    User user = new()
                    {
                        UserName = "admin",
                        Email = adminEmail,
                        EmailConfirmed = true,
                        LastName = "DEL SISTEMA",
                        FirstName = "Administrador",
                        Enabled = true
                    };

                    IdentityResult result = await userManager.CreateAsync(user, "@Admin123");
                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(user, "ADMINISTRADOR");
                    }
                }




                // Acá podés hacer seed a otras tablas también
                AppDbContext dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                if (!dbContext.States.Any())
                {
                    GeneralResponse reusltStates = await UploadStatesAsync(dbContext);
                    if (!reusltStates.Success)
                        return reusltStates;
                }
                if (!dbContext.AccountTypes.Any())
                {
                    GeneralResponse resultAccountTypes = await CreateAccountTypesAsync(dbContext);
                    if (!resultAccountTypes.Success)
                        return resultAccountTypes;
                }
                if (!dbContext.Accounts.Any())
                {
                    GeneralResponse resultAccount = await CreateAccountsAsync(dbContext);
                    if (!resultAccount.Success)
                        return resultAccount;
                }
                if (!dbContext.CbteTypes.Any(cp => cp.AfipId == 999))
                {
                    try
                    {
                        dbContext.CbteTypes.Add(new CbteType
                        {
                            //  Id = 999,
                            AfipId = 999,
                            CreateDate = DateTime.Now,
                            CreateUser = "System",
                            Description = "Presupuesto",
                            IsEnabled = true,
                        });
                        await dbContext.SaveChangesAsync();
                    }
                    catch (Exception ex)
                    {
                        resultResponse.Message = ex.Message;
                        return resultResponse;
                    }
                }
                if (!dbContext.Banks.Any())
                {
                    GeneralResponse resultBank = await CreateBanksAsync(dbContext);
                    if (!resultBank.Success)
                        return resultBank;
                }
                if (!dbContext.Boxes.Any())
                {
                    GeneralResponse resultBox = await CreateBoxesAsync(dbContext);
                    if (!resultBox.Success)
                        return resultBox;
                }
                if (!dbContext.PriceLists.Any())
                {
                    GeneralResponse resultPriceList = await CreatePriceListAsync(dbContext);
                    if (!resultPriceList.Success)
                        return resultPriceList;
                }
                if (!dbContext.Categories.Any())
                {
                    GeneralResponse resultCategory = await CreateCategoriesAsync(dbContext);
                    if (!resultCategory.Success)
                        return resultCategory;
                }
                if (!dbContext.Measures.Any())
                {
                    GeneralResponse resultMeasure = await CreateMeasuresAsync(dbContext);
                    if (!resultMeasure.Success)
                        return resultMeasure;
                }
                if (!dbContext.Taxes.Any())
                {
                    GeneralResponse resultTaxes = await CreateTaxesAsync(dbContext);
                    if (!resultTaxes.Success)
                        return resultTaxes;
                }
                GeneralResponse resultPermisionInModules = await CheckPermisionInModulesAsync(dbContext);
                if (!resultPermisionInModules.Success)
                    return resultPermisionInModules;

                resultResponse.Success = true;
                return resultResponse;

            }
            catch (Exception ex)
            {
                return new GeneralResponse { Success = false, Message = ex.Message };
            }
        }

        private static async Task<GeneralResponse> CreateBoxesAsync(AppDbContext _context)
        {
            GeneralResponse result = new() { Success = false };
            try
            {
                while (StaticCommon.ContextInUse)
                    await Task.Delay(100);

                StaticCommon.ContextInUse = true;

                _context.Boxes.AddRange(new List<Box>
                    {
                        new Box
                        {
                            BoxName = "pesos".ToUpper(),
                            CreateDate = DateTime.Now,
                            CreateUser = "System",
                            FromDebit = 0m,
                            FromCredit = 0m,
                            IsDeleted = false,
                            IsEnabled = true,
                            Sold = 0m,
                            SaleCondition = SaleCondition.Efectivo_Peso,
                            AccountId = _context.Accounts
                            .Where(a=> a.AccountGroupNumber == 1 && a.AccountSubGroupNumber1 == 1 && a.AccountSubGroupNumber2 == 1 && a.AccountSubGroupNumber3 == 1 && a.AccountSubGroupNumber4 == 1)
                            .FirstOrDefault().Id,
                        },
                        new Box
                        {
                            BoxName = "dólares".ToUpper(),
                            CreateDate = DateTime.Now,
                            CreateUser = "System",
                            FromDebit = 0m,
                            FromCredit = 0m,
                            IsDeleted = false,
                            IsEnabled = true,
                            Sold = 0m,
                            SaleCondition = SaleCondition.Efectivo_Dolar,
                            AccountId = _context.Accounts
                             .Where(a=> a.AccountGroupNumber == 1 && a.AccountSubGroupNumber1 == 1 && a.AccountSubGroupNumber2 == 1 && a.AccountSubGroupNumber3 == 1 && a.AccountSubGroupNumber4 == 2)
                            .FirstOrDefault().Id,
                        },
                        new Box
                        {
                            BoxName = "Reales".ToUpper(),
                            CreateDate = DateTime.Now,
                            CreateUser = "System",
                            FromDebit = 0m,
                            FromCredit = 0m,
                            IsDeleted = false,
                            IsEnabled = true,
                            Sold = 0m,
                            SaleCondition = SaleCondition.Efectivo_Real,
                            AccountId = _context.Accounts
                             .Where(a=> a.AccountGroupNumber == 1 && a.AccountSubGroupNumber1 == 1 && a.AccountSubGroupNumber2 == 1 && a.AccountSubGroupNumber3 == 1 && a.AccountSubGroupNumber4 == 4)
                            .FirstOrDefault().Id,
                        },
                        new Box
                        {
                            BoxName = "Euros".ToUpper(),
                            CreateDate = DateTime.Now,
                            CreateUser = "System",
                            FromDebit = 0m,
                            FromCredit = 0m,
                            IsDeleted = false,
                            IsEnabled = true,
                            Sold = 0m,
                            SaleCondition = SaleCondition.Efectivo_Euro,
                            AccountId = _context.Accounts
                             .Where(a=> a.AccountGroupNumber == 1 && a.AccountSubGroupNumber1 == 1 && a.AccountSubGroupNumber2 == 1 && a.AccountSubGroupNumber3 == 1 && a.AccountSubGroupNumber4 == 3)
                            .FirstOrDefault().Id,
                        },
                        new Box
                        {
                            BoxName = "otro".ToUpper(),
                            CreateDate = DateTime.Now,
                            CreateUser = "System",
                            FromDebit = 0m,
                            FromCredit = 0m,
                            IsDeleted = false,
                            IsEnabled = true,
                            Sold = 0m,
                            SaleCondition = SaleCondition.Efectivo_Otro,
                            AccountId = _context.Accounts
                             .Where(a=> a.AccountGroupNumber == 1 && a.AccountSubGroupNumber1 == 1 && a.AccountSubGroupNumber2 == 1 && a.AccountSubGroupNumber3 == 1 && a.AccountSubGroupNumber4 == 9)
                            .FirstOrDefault().Id,
                        },
                    });

                StaticCommon.ContextInUse = false;
                await _context.SaveChangesAsync();
                result.Success = true;
                return result;
            }
            catch (Exception ex)
            {
                StaticCommon.ContextInUse = false;
                result.Message = ex.Message;
                return result;
            }
        }

        private static async Task<GeneralResponse> CreateBanksAsync(AppDbContext _context)
        {
            GeneralResponse result = new() { Success = false };

            try
            {
                while (StaticCommon.ContextInUse)
                    await Task.Delay(100);

                StaticCommon.ContextInUse = true;

                State? state = await _context.States.FirstOrDefaultAsync();
                Account? accountBank1 = await _context.Accounts
                        .Where(a => a.AccountGroupNumber == 1 && a.AccountSubGroupNumber1 == 1 && a.AccountSubGroupNumber2 == 1 && a.AccountSubGroupNumber3 == 2 && a.AccountSubGroupNumber4 == 1)
                        .FirstOrDefaultAsync();
                Account? accountBank2 = await _context.Accounts
                        .Where(a => a.AccountGroupNumber == 1 && a.AccountSubGroupNumber1 == 1 && a.AccountSubGroupNumber2 == 1 && a.AccountSubGroupNumber3 == 2 && a.AccountSubGroupNumber4 == 2)
                        .FirstOrDefaultAsync();

                Bank bank = new()
                {
                    CreateDate = DateTime.Now,
                    CreateUser = "System",
                    IsDeleted = false,
                    IsEnabled = true,
                    AccountNumber = "0123456789",
                    Address = "Domicilio",
                    Alias = "Alias de Cuanta",
                    BankName = "Banco".ToUpper(),
                    CBU = "1111111111111111111111",
                    City = "San Martín",
                    Email = "email@email.com",
                    FromCredit = 0m,
                    FromDebit = 0m,
                    Phone = "123456789",
                    Phone1 = "123456789",
                    PostalCode = "1650",
                    Sold = 0m,
                    StateId = state.Id,
                    State = state,
                    WebSite = "www.banco.com",
                    AccountId = accountBank1.Id,
                    Account = accountBank1,
                };
                _context.Banks.Add(bank);

                Bank bank1 = new()
                {
                    CreateDate = DateTime.Now,
                    CreateUser = "System",
                    IsDeleted = false,
                    IsEnabled = true,
                    AccountNumber = "0123456789",
                    Address = "Arias 3751 piso 7mo",
                    Alias = "Alias de Cuanta",
                    BankName = "MERCADO PAGO".ToUpper(),
                    CBU = "1111111111111111111111",
                    City = "Ciudad Autónoma de Buenos Aires",
                    Email = "ayuda@mercadopago.com.ar",
                    FromCredit = 0m,
                    FromDebit = 0m,
                    Phone = "123456789",
                    Phone1 = "123456789",
                    PostalCode = "1430",
                    Sold = 0m,
                    StateId = state.Id,
                    State = state,
                    WebSite = "https://www.mercadopago.com.ar",
                    AccountId = accountBank2.Id,
                    Account = accountBank2,
                };
                _context.Banks.Add(bank1);
                await _context.SaveChangesAsync();
                _context.BankParameters.AddRange(new List<BankParameter>
                    {
                        new BankParameter
                        {
                            Bank = bank,
                            BankId = bank.Id,
                            AcreditationDay = 0,
                            CreateDate = DateTime.Now,
                            CreateUser = "System",
                            IsDeleted = false,
                            IsEnabled = true,
                            Rate = 0m,
                            SaleCondition = SaleCondition.Cheque,
                        },
                        new BankParameter
                        {
                            Bank = bank,
                            BankId = bank.Id,
                            AcreditationDay = 0,
                            CreateDate = DateTime.Now,
                            CreateUser = "System",
                            IsDeleted = false,
                            IsEnabled = true,
                            Rate = 0m,
                            SaleCondition = SaleCondition.Deposito,
                        },
                        new BankParameter
                        {
                            Bank = bank,
                            BankId = bank.Id,
                            AcreditationDay = 0,
                            CreateDate = DateTime.Now,
                            CreateUser = "System",
                            IsDeleted = false,
                            IsEnabled = true,
                            Rate = 0m,
                            SaleCondition = SaleCondition.Credito,
                        },
                        new BankParameter
                        {
                            Bank = bank,
                            BankId = bank.Id,
                            AcreditationDay = 0,
                            CreateDate = DateTime.Now,
                            CreateUser = "System",
                            IsDeleted = false,
                            IsEnabled = true,
                            Rate = 0m,
                            SaleCondition = SaleCondition.Debito,
                        },
                        new BankParameter
                        {
                            Bank = bank1,
                            BankId = bank1.Id,
                            AcreditationDay = 0,
                            CreateDate = DateTime.Now,
                            CreateUser = "System",
                            IsDeleted = false,
                            IsEnabled = true,
                            Rate = 0m,
                            SaleCondition = SaleCondition.MercadoPago_Credito,
                        },
                        new BankParameter
                        {
                            Bank = bank1,
                            BankId = bank1.Id,
                            AcreditationDay = 0,
                            CreateDate = DateTime.Now,
                            CreateUser = "System",
                            IsDeleted = false,
                            IsEnabled = true,
                            Rate = 0m,
                            SaleCondition = SaleCondition.MercadoPago_Debito,
                        },
                        new BankParameter
                        {
                            Bank = bank1,
                            BankId = bank1.Id,
                            AcreditationDay = 0,
                            CreateDate = DateTime.Now,
                            CreateUser = "System",
                            IsDeleted = false,
                            IsEnabled = true,
                            Rate = 0m,
                            SaleCondition = SaleCondition.MercadoPago_Link_Pago,
                        },
                        new BankParameter
                        {
                            Bank = bank1,
                            BankId = bank1.Id,
                            AcreditationDay = 0,
                            CreateDate = DateTime.Now,
                            CreateUser = "System",
                            IsDeleted = false,
                            IsEnabled = true,
                            Rate = 0m,
                            SaleCondition = SaleCondition.MercadoPago_OnLine,
                        },
                        new BankParameter
                        {
                            Bank = bank1,
                            BankId = bank1.Id,
                            AcreditationDay = 0,
                            CreateDate = DateTime.Now,
                            CreateUser = "System",
                            IsDeleted = false,
                            IsEnabled = true,
                            Rate = 0m,
                            SaleCondition = SaleCondition.MercadoPago_Otro,
                        },
                        new BankParameter
                        {
                            Bank = bank1,
                            BankId = bank1.Id,
                            AcreditationDay = 0,
                            CreateDate = DateTime.Now,
                            CreateUser = "System",
                            IsDeleted = false,
                            IsEnabled = true,
                            Rate = 0m,
                            SaleCondition = SaleCondition.MercadoPago_QR_Credito,
                        },
                        new BankParameter
                        {
                            Bank = bank1,
                            BankId = bank1.Id,
                            AcreditationDay = 0,
                            CreateDate = DateTime.Now,
                            CreateUser = "System",
                            IsDeleted = false,
                            IsEnabled = true,
                            Rate = 0m,
                            SaleCondition = SaleCondition.MercadoPago_QR_Dinero_Cuenta,
                        },
                        new BankParameter
                        {
                            Bank = bank1,
                            BankId = bank1.Id,
                            AcreditationDay = 0,
                            CreateDate = DateTime.Now,
                            CreateUser = "System",
                            IsDeleted = false,
                            IsEnabled = true,
                            Rate = 0m,
                            SaleCondition = SaleCondition.MercadoPago_QR_Debito,
                        },
                        new BankParameter
                        {
                            Bank = bank1,
                            BankId = bank1.Id,
                            AcreditationDay = 0,
                            CreateDate = DateTime.Now,
                            CreateUser = "System",
                            IsDeleted = false,
                            IsEnabled = true,
                            Rate = 0m,
                            SaleCondition = SaleCondition.Mercado_Pago_Transfencia,
                        },
                    });

                StaticCommon.ContextInUse = false;
                await _context.SaveChangesAsync();
                result.Success = true;
                return result;
            }
            catch (Exception ex)
            {
                StaticCommon.ContextInUse = false;
                result.Message = ex.Message;
                return result;
            }
        }

        private static async Task<GeneralResponse> CreateAccountsAsync(AppDbContext _context)
        {
            GeneralResponse result = new GeneralResponse { Success = false };

            try
            {
                while (StaticCommon.ContextInUse)
                    await Task.Delay(100);

                StaticCommon.ContextInUse = true;

                string pathAccounts = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "Pre", "Cuentas.xlsx");

                FileStream streamAccount = new FileStream(pathAccounts, FileMode.Open);
                IExcelDataReader readerAccount = null;
                readerAccount = ExcelReaderFactory.CreateOpenXmlReader(streamAccount);

                DataSet resultAccount = readerAccount.AsDataSet(new ExcelDataSetConfiguration()
                {
                    ConfigureDataTable = (_) => new ExcelDataTableConfiguration()
                    {
                        UseHeaderRow = true
                    }
                });

                readerAccount.Close();
                List<Account> accounts = new List<Account>();

                foreach (DataRow itemAccount in resultAccount.Tables[0].Rows)
                {
                    string accountName = itemAccount[0].ToString().ToUpper();
                    int accountGroupNumber = int.Parse(itemAccount[1].ToString());
                    int accountSubGroupNumber1 = int.Parse(itemAccount[2].ToString());
                    int accountSubGroupNumber2 = int.Parse(itemAccount[3].ToString());
                    int accountSubGroupNumber3 = int.Parse(itemAccount[4].ToString());
                    int accountSubGroupNumber4 = int.Parse(itemAccount[5].ToString());
                    int accountSubGroupNumber5 = int.Parse(itemAccount[6].ToString());
                    int accountTypeId = int.Parse(itemAccount[7].ToString());
                    bool accountEnabled = itemAccount[8].ToString() != "0";
                    string accountDescription = itemAccount[9].ToString();

                    accounts.Add(new Account
                    {
                        AccountGroupNumber = accountGroupNumber,
                        AccountSubGroupNumber1 = accountSubGroupNumber1,
                        AccountSubGroupNumber2 = accountSubGroupNumber2,
                        AccountSubGroupNumber3 = accountSubGroupNumber3,
                        AccountSubGroupNumber4 = accountSubGroupNumber4,
                        AccountSubGroupNumber5 = accountSubGroupNumber5,
                        AccountTypeId = accountTypeId,
                        CreateDate = DateTime.Now,
                        CreateUser = "System",
                        IsDeleted = false,
                        IsEnabled = accountEnabled,
                        Name = accountName,
                        Description = accountDescription,
                    });
                }

                _context.Accounts.AddRange(accounts);
                StaticCommon.ContextInUse = false;
                await _context.SaveChangesAsync();
                result.Success = true;
                return result;
            }
            catch (Exception ex)
            {
                StaticCommon.ContextInUse = false;
                result.Message = ex.Message;
                return result;
            }
        }

        private static async Task<GeneralResponse> CreateAccountTypesAsync(AppDbContext _context)
        {
            GeneralResponse result = new GeneralResponse { Success = false };
            try
            {
                while (StaticCommon.ContextInUse)
                    await Task.Delay(100);
                StaticCommon.ContextInUse = true;

                List<AccountType> accountTypes =
                    [
                    new AccountType
                    {
                        CreateDate = DateTime.Now,
                        CreateUser = "System",
                        IsDeleted = false,
                        IsEnabled = true,
                        IsActive = true,
                        IsNetHeritage = false,
                        IsPasive = false,
                        Name = "activo".ToUpper(),
                    },
                    new AccountType
                {
                    CreateDate = DateTime.Now,
                    CreateUser = "System",
                    IsDeleted = false,
                    IsEnabled = true,
                    IsActive = false,
                    IsNetHeritage = false,
                    IsPasive = true,
                    Name = "pasivo".ToUpper(),
                },
                    new AccountType
                {
                    CreateDate = DateTime.Now,
                    CreateUser = "System",
                    IsDeleted = false,
                    IsEnabled = true,
                    IsActive = false,
                    IsNetHeritage = true,
                    IsPasive = false,
                    Name = "patrimonio neto".ToUpper(),
                },
                    new AccountType
                {
                    CreateDate = DateTime.Now,
                    CreateUser = "System",
                    IsDeleted = false,
                    IsEnabled = false,
                    IsActive = true,
                    IsNetHeritage = true,
                    IsPasive = true,
                    Name = "Movimiento".ToUpper(),
                },
                    new AccountType
                {
                    CreateDate = DateTime.Now,
                    CreateUser = "System",
                    IsDeleted = false,
                    IsEnabled = true,
                    IsActive = true,
                    IsNetHeritage = true,
                    IsPasive = true,
                    Name = "Recursos".ToUpper(),
                },
                    new AccountType
                {
                    CreateDate = DateTime.Now,
                    CreateUser = "System",
                    IsDeleted = false,
                    IsEnabled = true,
                    IsActive = true,
                    IsNetHeritage = true,
                    IsPasive = true,
                    Name = "Gastos".ToUpper(),
                }

                    ];

                _context.AccountTypes.AddRange(accountTypes);

                StaticCommon.ContextInUse = false;
                await _context.SaveChangesAsync();
                result.Success = true;
                return result;
            }
            catch (Exception ex)
            {
                StaticCommon.ContextInUse = false;
                result.Message = ex.Message;
                return result;
            }
        }

        private static async Task<GeneralResponse> CheckPermisionInModulesAsync(AppDbContext _context)
        {
            GeneralResponse result = new GeneralResponse { Success = false };

            try
            {
                while (StaticCommon.ContextInUse)
                    await Task.Delay(100);
                StaticCommon.ContextInUse = true;

                IdentityRole developerRole = await _context.Roles.FindAsync("1");
                IdentityRole adminRole = await _context.Roles.FindAsync("2");
                IdentityRole supervisorRole = await _context.Roles.FindAsync("3");
                IdentityRole operatorRole = await _context.Roles.FindAsync("4");
                IdentityRole cashierRole = await _context.Roles.FindAsync("5");
                List<Permission> permissions = await _context.Permissions.ToListAsync();

                foreach (ModuleType moduleType in Enum.GetValues(typeof(ModuleType)))
                {
                    string operationView = $"{EnumExtensionService.GetDisplayName(moduleType)}-Lectura";
                    string operationAdd = $"{EnumExtensionService.GetDisplayName(moduleType)}-Agregar";
                    string operationEdit = $"{EnumExtensionService.GetDisplayName(moduleType)}-Editar";
                    string operationDelete = $"{EnumExtensionService.GetDisplayName(moduleType)}-Borrar";

                    if (!permissions.Any(p => p.Name == operationView && p.ModuleType == moduleType))
                    {
                        Permission permission = new Permission
                        {
                            CreateDate = DateTime.Now,
                            CreateUser = "System",
                            IsDeleted = false,
                            IsEnabled = true,
                            ModuleType = moduleType,
                            Name = operationView,
                        };
                        await _context.Permissions.AddAsync(permission);
                        await _context.SaveChangesAsync();

                        await _context.RolePermissions.AddAsync(new RolePermission
                        {
                            CreateDate = DateTime.Now,
                            CreateUser = "System",
                            IsDeleted = false,
                            IsEnabled = true,
                            PermissionId = permission.Id,
                            RoleId = developerRole.Id,
                        });
                        await _context.RolePermissions.AddAsync(new RolePermission
                        {
                            CreateDate = DateTime.Now,
                            CreateUser = "System",
                            IsDeleted = false,
                            IsEnabled = true,
                            PermissionId = permission.Id,
                            RoleId = adminRole.Id,
                        });
                        await _context.RolePermissions.AddAsync(new RolePermission
                        {
                            CreateDate = DateTime.Now,
                            CreateUser = "System",
                            IsDeleted = false,
                            IsEnabled = true,
                            PermissionId = permission.Id,
                            RoleId = supervisorRole.Id,
                        });
                        await _context.RolePermissions.AddAsync(new RolePermission
                        {
                            CreateDate = DateTime.Now,
                            CreateUser = "System",
                            IsDeleted = false,
                            IsEnabled = true,
                            PermissionId = permission.Id,
                            RoleId = operatorRole.Id,
                        });

                        await _context.SaveChangesAsync();

                        foreach (User user in await _context.Users.ToListAsync())
                        {
                            List<RolePermission> rolePermissions = await _context.RolePermissions
                                .Where(rp => rp.RoleId == _context.UserRoles.Where(ur => ur.UserId == user.Id).FirstOrDefault().RoleId)
                                .ToListAsync();

                            foreach (RolePermission rolePermission in rolePermissions)
                            {
                                UserPermission? userPermission = await _context.UserPermissions
                                    .Where(up => up.UserId == user.Id && up.PermissionId == rolePermission.PermissionId)
                                    .FirstOrDefaultAsync();
                                if (userPermission == null)
                                    await _context.UserPermissions.AddAsync(new UserPermission
                                    {
                                        CreateDate = DateTime.Now,
                                        CreateUser = "System",
                                        IsDeleted = false,
                                        IsEnabled = rolePermission.IsEnabled,
                                        UserId = user.Id,
                                        PermissionId = rolePermission.PermissionId,
                                    });
                            }
                            await _context.SaveChangesAsync();
                        }

                    }
                    if (!permissions.Any(p => p.Name == operationAdd && p.ModuleType == moduleType))
                    {
                        Permission permission = new Permission
                        {
                            CreateDate = DateTime.Now,
                            CreateUser = "System",
                            IsDeleted = false,
                            IsEnabled = true,
                            ModuleType = moduleType,
                            Name = operationAdd,
                        };
                        await _context.Permissions.AddAsync(permission);
                        await _context.SaveChangesAsync();

                        await _context.RolePermissions.AddAsync(new RolePermission
                        {
                            CreateDate = DateTime.Now,
                            CreateUser = "System",
                            IsDeleted = false,
                            IsEnabled = true,
                            PermissionId = permission.Id,
                            RoleId = developerRole.Id,
                        });
                        await _context.RolePermissions.AddAsync(new RolePermission
                        {
                            CreateDate = DateTime.Now,
                            CreateUser = "System",
                            IsDeleted = false,
                            IsEnabled = true,
                            PermissionId = permission.Id,
                            RoleId = adminRole.Id,
                        });
                        await _context.RolePermissions.AddAsync(new RolePermission
                        {
                            CreateDate = DateTime.Now,
                            CreateUser = "System",
                            IsDeleted = false,
                            IsEnabled = true,
                            PermissionId = permission.Id,
                            RoleId = supervisorRole.Id,
                        });
                        await _context.RolePermissions.AddAsync(new RolePermission
                        {
                            CreateDate = DateTime.Now,
                            CreateUser = "System",
                            IsDeleted = false,
                            IsEnabled = true,
                            PermissionId = permission.Id,
                            RoleId = operatorRole.Id,
                        });
                        await _context.SaveChangesAsync();

                        foreach (User user in await _context.Users.ToListAsync())
                        {
                            List<RolePermission> rolePermissions = await _context.RolePermissions
                                .Where(rp => rp.RoleId == _context.UserRoles.Where(ur => ur.UserId == user.Id).FirstOrDefault().RoleId)
                                .ToListAsync();

                            foreach (RolePermission rolePermission in rolePermissions)
                            {
                                UserPermission? userPermission = await _context.UserPermissions
                                    .Where(up => up.UserId == user.Id && up.PermissionId == rolePermission.PermissionId)
                                    .FirstOrDefaultAsync();
                                if (userPermission == null)
                                    await _context.UserPermissions.AddAsync(new UserPermission
                                    {
                                        CreateDate = DateTime.Now,
                                        CreateUser = "System",
                                        IsDeleted = false,
                                        IsEnabled = rolePermission.IsEnabled,
                                        UserId = user.Id,
                                        PermissionId = rolePermission.PermissionId,
                                    });
                            }
                            await _context.SaveChangesAsync();
                        }
                    }
                    if (!permissions.Any(p => p.Name == operationEdit && p.ModuleType == moduleType))
                    {
                        Permission permission = new Permission
                        {
                            CreateDate = DateTime.Now,
                            CreateUser = "System",
                            IsDeleted = false,
                            IsEnabled = true,
                            ModuleType = moduleType,
                            Name = operationEdit,
                        };
                        await _context.Permissions.AddAsync(permission);
                        await _context.SaveChangesAsync();

                        await _context.RolePermissions.AddAsync(new RolePermission
                        {
                            CreateDate = DateTime.Now,
                            CreateUser = "System",
                            IsDeleted = false,
                            IsEnabled = true,
                            PermissionId = permission.Id,
                            RoleId = developerRole.Id,
                        });
                        await _context.RolePermissions.AddAsync(new RolePermission
                        {
                            CreateDate = DateTime.Now,
                            CreateUser = "System",
                            IsDeleted = false,
                            IsEnabled = true,
                            PermissionId = permission.Id,
                            RoleId = adminRole.Id,
                        });
                        await _context.RolePermissions.AddAsync(new RolePermission
                        {
                            CreateDate = DateTime.Now,
                            CreateUser = "System",
                            IsDeleted = false,
                            IsEnabled = true,
                            PermissionId = permission.Id,
                            RoleId = supervisorRole.Id,
                        });
                        await _context.RolePermissions.AddAsync(new RolePermission
                        {
                            CreateDate = DateTime.Now,
                            CreateUser = "System",
                            IsDeleted = false,
                            IsEnabled = false,
                            PermissionId = permission.Id,
                            RoleId = operatorRole.Id,
                        });
                        await _context.SaveChangesAsync();

                        foreach (User user in await _context.Users.ToListAsync())
                        {
                            List<RolePermission> rolePermissions = await _context.RolePermissions
                                .Where(rp => rp.RoleId == _context.UserRoles.Where(ur => ur.UserId == user.Id).FirstOrDefault().RoleId)
                                .ToListAsync();

                            foreach (RolePermission rolePermission in rolePermissions)
                            {
                                UserPermission? userPermission = await _context.UserPermissions
                                    .Where(up => up.UserId == user.Id && up.PermissionId == rolePermission.PermissionId)
                                    .FirstOrDefaultAsync();
                                if (userPermission == null)
                                    await _context.UserPermissions.AddAsync(new UserPermission
                                    {
                                        CreateDate = DateTime.Now,
                                        CreateUser = "System",
                                        IsDeleted = false,
                                        IsEnabled = rolePermission.IsEnabled,
                                        UserId = user.Id,
                                        PermissionId = rolePermission.PermissionId,
                                    });
                            }
                            await _context.SaveChangesAsync();
                        }
                    }
                    if (!permissions.Any(p => p.Name == operationDelete && p.ModuleType == moduleType))
                    {
                        Permission permission = new Permission
                        {
                            CreateDate = DateTime.Now,
                            CreateUser = "System",
                            IsDeleted = false,
                            IsEnabled = true,
                            ModuleType = moduleType,
                            Name = operationDelete,
                        };
                        await _context.Permissions.AddAsync(permission);
                        await _context.SaveChangesAsync();

                        await _context.RolePermissions.AddAsync(new RolePermission
                        {
                            CreateDate = DateTime.Now,
                            CreateUser = "System",
                            IsDeleted = false,
                            IsEnabled = true,
                            PermissionId = permission.Id,
                            RoleId = developerRole.Id,
                        });
                        await _context.RolePermissions.AddAsync(new RolePermission
                        {
                            CreateDate = DateTime.Now,
                            CreateUser = "System",
                            IsDeleted = false,
                            IsEnabled = true,
                            PermissionId = permission.Id,
                            RoleId = adminRole.Id,
                        });
                        await _context.RolePermissions.AddAsync(new RolePermission
                        {
                            CreateDate = DateTime.Now,
                            CreateUser = "System",
                            IsDeleted = false,
                            IsEnabled = false,
                            PermissionId = permission.Id,
                            RoleId = supervisorRole.Id,
                        });
                        await _context.RolePermissions.AddAsync(new RolePermission
                        {
                            CreateDate = DateTime.Now,
                            CreateUser = "System",
                            IsDeleted = false,
                            IsEnabled = false,
                            PermissionId = permission.Id,
                            RoleId = operatorRole.Id,
                        });
                        await _context.SaveChangesAsync();

                        foreach (User user in await _context.Users.ToListAsync())
                        {
                            List<RolePermission> rolePermissions = await _context.RolePermissions
                                .Where(rp => rp.RoleId == _context.UserRoles.Where(ur => ur.UserId == user.Id).FirstOrDefault().RoleId)
                                .ToListAsync();

                            foreach (RolePermission rolePermission in rolePermissions)
                            {
                                UserPermission? userPermission = await _context.UserPermissions
                                    .Where(up => up.UserId == user.Id && up.PermissionId == rolePermission.PermissionId)
                                    .FirstOrDefaultAsync();
                                if (userPermission == null)
                                    await _context.UserPermissions.AddAsync(new UserPermission
                                    {
                                        CreateDate = DateTime.Now,
                                        CreateUser = "System",
                                        IsDeleted = false,
                                        IsEnabled = rolePermission.IsEnabled,
                                        UserId = user.Id,
                                        PermissionId = rolePermission.PermissionId,
                                    });
                            }
                            await _context.SaveChangesAsync();
                        }
                    }
                }

                StaticCommon.ContextInUse = false;
                result.Success = true;
                return result;
            }
            catch (Exception ex)
            {
                StaticCommon.ContextInUse = false;
                result.Message = ex.Message;
                return result;
            }

        }

        private static async Task<GeneralResponse> CreateTaxesAsync(AppDbContext _context)
        {
            GeneralResponse result = new GeneralResponse { Success = false };
            try
            {
                while (StaticCommon.ContextInUse)
                    await Task.Delay(100);
                StaticCommon.ContextInUse = true;
                List<Tax> taxes =
                [
                    new Tax
                    {
                        CreateDate = DateTime.Now,
                        CreateUser = "System",
                        IsEnabled = true,
                        IsDeleted = false,
                        Description = "0%".ToUpper(),
                        AfipId = "3",
                        Rate = 0m,
                    },
                    new Tax
                    {
                        CreateDate = DateTime.Now,
                        CreateUser = "System",
                        IsEnabled = true,
                        IsDeleted = false,
                        Description = "10.5%".ToUpper(),
                        AfipId = "4",
                        Rate = 10.5m,
                    },
                    new Tax
                    {
                        CreateDate = DateTime.Now,
                        CreateUser = "System",
                        IsEnabled = true,
                        IsDeleted = false,
                        Description = "21%".ToUpper(),
                        AfipId = "5",
                        Rate = 21m,
                    },
                    new Tax
                    {
                        CreateDate = DateTime.Now,
                        CreateUser = "System",
                        IsEnabled = true,
                        IsDeleted = false,
                        Description = "27%".ToUpper(),
                        AfipId = "6",
                        Rate = 27m,
                    },
                    new Tax
                    {
                        CreateDate = DateTime.Now,
                        CreateUser = "System",
                        IsEnabled = true,
                        IsDeleted = false,
                        Description = "5%".ToUpper(),
                        AfipId = "8",
                        Rate = 5m,
                    },
                    new Tax
                    {
                        CreateDate = DateTime.Now,
                        CreateUser = "System",
                        IsEnabled = true,
                        IsDeleted = false,
                        Description = "2.5%".ToUpper(),
                        AfipId = "9",
                        Rate = 2.5m,
                    },
                ];

                _context.Taxes.AddRange(taxes);

                StaticCommon.ContextInUse = false;
                await _context.SaveChangesAsync();
                result.Success = true;
                return result;
            }
            catch (Exception ex)
            {
                StaticCommon.ContextInUse = false;
                result.Message = ex.Message;
                return result;
            }
        }

        private static async Task<GeneralResponse> CreateMeasuresAsync(AppDbContext _context)
        {
            GeneralResponse result = new GeneralResponse { Success = false };
            try
            {
                while (StaticCommon.ContextInUse)
                    await Task.Delay(100);

                List<Measure> measures =
                [
                    new Measure
                    {
                        CreateDate = DateTime.Now,
                        CreateUser = "System",
                        IsEnabled = true,
                        IsDeleted = false,
                        Description = "Unidad".ToUpper(),
                    },
                    new Measure
                    {
                        CreateDate = DateTime.Now,
                        CreateUser = "System",
                        IsEnabled = true,
                        IsDeleted = false,
                        Description = "Docena".ToUpper(),
                    },
                    new Measure
                    {
                        CreateDate = DateTime.Now,
                        CreateUser = "System",
                        IsEnabled = true,
                        IsDeleted = false,
                        Description = "Kilogramo".ToUpper(),
                    },
                    new Measure
                    {
                        CreateDate = DateTime.Now,
                        CreateUser = "System",
                        IsEnabled = true,
                        IsDeleted = false,
                        Description = "Hora".ToUpper(),
                    },
                    new Measure
                    {
                        CreateDate = DateTime.Now,
                        CreateUser = "System",
                        IsEnabled = true,
                        IsDeleted = false,
                        Description = "Litro".ToUpper(),
                    },
                ];

                _context.Measures.AddRange(measures);

                StaticCommon.ContextInUse = false;
                await _context.SaveChangesAsync();

                result.Success = true;
                return result;
            }
            catch (Exception ex)
            {
                StaticCommon.ContextInUse = false;
                result.Message = ex.Message;
                return result;
            }
        }

        private static async Task<GeneralResponse> CreateCategoriesAsync(AppDbContext _context)
        {
            GeneralResponse result = new GeneralResponse { Success = false };
            try
            {
                while (StaticCommon.ContextInUse)
                    await Task.Delay(100);

                List<Category> categories =
                [
                    new Category
                    {
                        CreateDate = DateTime.Now,
                        CreateUser = "System",
                        IsEnabled = true,
                        IsDeleted = false,
                        Description = "Categoria 1",
                        Color = "#FF5733",
                    },
                    new Category
                    {
                        CreateDate = DateTime.Now,
                        CreateUser = "System",
                        IsEnabled = true,
                        IsDeleted = false,
                        Description = "Categoria 2",
                        Color = "#FFC773",
                    },
                    new Category
                    {
                        CreateDate = DateTime.Now,
                        CreateUser = "System",
                        IsEnabled = true,
                        IsDeleted = false,
                        Description = "Categoria 3",
                        Color = "#4A7781",
                    },
                ];

                _context.Categories.AddRange(categories);

                StaticCommon.ContextInUse = false;
                await _context.SaveChangesAsync();

                result.Success = true;
                return result;
            }
            catch (Exception ex)
            {
                StaticCommon.ContextInUse = false;
                result.Message = ex.Message;
                return result;
            }
        }

        private static async Task<GeneralResponse> CreatePriceListAsync(AppDbContext _context)
        {
            GeneralResponse result = new GeneralResponse { Success = false };
            try
            {
                while (StaticCommon.ContextInUse)
                    await Task.Delay(100);

                List<PriceList> priceLists =
                [
                    new PriceList
                    {
                        CreateDate = DateTime.Now,
                        CreateUser = "System",
                        IsEnabled = true,
                        IsDeleted = false,
                        Description = "5 %",
                        Utility = 5,
                    },
                    new PriceList
                    {
                        CreateDate = DateTime.Now,
                        CreateUser = "System",
                        IsEnabled = true,
                        IsDeleted = false,
                        Description = "10 %",
                        Utility = 10,
                    },
                    new PriceList
                    {
                        CreateDate = DateTime.Now,
                        CreateUser = "System",
                        IsEnabled = true,
                        IsDeleted = false,
                        Description = "20 %",
                        Utility = 20,
                    },
                ];

                _context.PriceLists.AddRange(priceLists);

                StaticCommon.ContextInUse = false;
                await _context.SaveChangesAsync();

                result.Success = true;
                return result;
            }
            catch (Exception ex)
            {
                StaticCommon.ContextInUse = false;
                result.Message = ex.Message;
                return result;
            }
        }

        private static async Task<GeneralResponse> UploadStatesAsync(AppDbContext _context)
        {
            GeneralResponse result = new GeneralResponse { Success = false };
            try
            {
                while (StaticCommon.ContextInUse)
                    await Task.Delay(100);
                StaticCommon.ContextInUse = true;
                string pathStates = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "Pre", "provincias.xlsx");
                string pathCities = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "Pre", "localidades.xlsx");

                IExcelDataReader readerStates = null;
                IExcelDataReader readerCities = null;
                FileStream streamStates = new FileStream(pathStates, FileMode.Open);
                FileStream streamCities = new FileStream(pathCities, FileMode.Open);

                readerStates = ExcelReaderFactory.CreateOpenXmlReader(streamStates);
                readerCities = ExcelReaderFactory.CreateOpenXmlReader(streamCities);

                DataSet resultStates = readerStates.AsDataSet(new ExcelDataSetConfiguration()
                {
                    ConfigureDataTable = (_) => new ExcelDataTableConfiguration()
                    {
                        UseHeaderRow = true
                    }
                });
                DataSet resultCities = readerCities.AsDataSet(new ExcelDataSetConfiguration()
                {
                    ConfigureDataTable = (_) => new ExcelDataTableConfiguration()
                    {
                        UseHeaderRow = true
                    }
                });

                readerStates.Close();
                readerCities.Close();
                foreach (DataRow itemStates in resultStates.Tables[0].Rows)
                {
                    StaticCommon.ContextInUse = true;
                    string stateName = itemStates[1].ToString().ToUpper();
                    int stateAfipId = int.Parse(itemStates[0].ToString());

                    List<City> cities = new List<City>();
                    State state = new State
                    {
                        Name = stateName,
                        CreateDate = DateTime.Now,
                        CreateUser = "System",
                        IsEnabled = true,
                        AfipId = stateAfipId,
                    };

                    _context.States.Add(state);
                    StaticCommon.ContextInUse = false;
                    await _context.SaveChangesAsync();
                    foreach (DataRow itemCities in resultCities.Tables[0].Rows)
                    {
                        StaticCommon.ContextInUse = true;
                        if (int.Parse(itemCities[2].ToString()) == stateAfipId)
                        {
                            string cityName = itemCities[1].ToString().ToUpper();
                            int cityAfipId = int.Parse(itemCities[0].ToString());
                            cities.Add(new City
                            {
                                AfipId = cityAfipId,
                                CreateDate = DateTime.Now,
                                CreateUser = "System",
                                IsEnabled = true,
                                Name = cityName,
                                StateId = state.Id,
                            });
                        }
                    }
                    _context.Cities.AddRange(cities);
                    StaticCommon.ContextInUse = false;
                    await _context.SaveChangesAsync();

                }
                StaticCommon.ContextInUse = false;
                result.Success = true;
                return result;
            }
            catch (Exception ex)
            {
                StaticCommon.ContextInUse = false;
                result.Message = ex.Message;
                return result;
            }
        }
    }
}
