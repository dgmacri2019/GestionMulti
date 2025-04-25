using ExcelDataReader;
using GestionComercial.Domain.Constant;
using GestionComercial.Domain.Entities.Afip;
using GestionComercial.Domain.Entities.Masters;
using GestionComercial.Domain.Entities.Masters.Security;
using GestionComercial.Domain.Entities.Stock;
using GestionComercial.Domain.Response;
using GestionComercial.Domain.Statics;
using GestionComercial.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Data;
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
