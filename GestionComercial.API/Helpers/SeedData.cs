using ExcelDataReader;
using GestionComercial.Domain.Entities.Afip;
using GestionComercial.Domain.Entities.Masters;
using GestionComercial.Domain.Entities.Stock;
using GestionComercial.Domain.Response;
using GestionComercial.Domain.Statics;
using GestionComercial.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Data;

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

                string adminEmail = "macri.diego@gmail.com";
                IdentityUser adminUser = await userManager.FindByEmailAsync(adminEmail);

                if (adminUser == null)
                {
                    User user = new User
                    {
                        UserName = "dgmacri",
                        Email = adminEmail,
                        EmailConfirmed = true,
                        LastName = "MACRI",
                        FirstName = "Diego Gaston",
                    };

                    IdentityResult result = await userManager.CreateAsync(user, "@Diego248");
                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(user, "DEVELOPER");
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
                resultResponse.Success = true;
                return resultResponse;

            }
            catch (Exception ex)
            {
                return new GeneralResponse { Success = false, Message = ex.Message };
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
                        Color = "FFFFFF",
                    },
                    new Category
                    {
                        CreateDate = DateTime.Now,
                        CreateUser = "System",
                        IsEnabled = true,
                        IsDeleted = false,
                        Description = "Categoria 2",
                        Color = "FFFFFF",
                    },
                    new Category
                    {
                        CreateDate = DateTime.Now,
                        CreateUser = "System",
                        IsEnabled = true,
                        IsDeleted = false,
                        Description = "Categoria 3",
                        Color = "FFFFFF",
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
