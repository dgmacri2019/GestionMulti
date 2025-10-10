using GestionComercial.Applications.Interfaces;
using GestionComercial.Domain.Response;
using GestionComercial.Domain.Statics;
using GestionComercial.Infrastructure.Persistence;

namespace GestionComercial.Applications.Services
{
    public class MasterService : IMasterService
    {
        private readonly AppDbContext _context;
        private readonly DBHelper _dBHelper;

        #region Constructor

        public MasterService(AppDbContext context)
        {
            _context = context;
            _dBHelper = new DBHelper();
        }

        #endregion


        public async Task<GeneralResponse> AddAsync<T>(T model)
        {
            while (StaticCommon.ContextInUse)
                await Task.Delay(100);

            // usar métodos async para transacciones
            await using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                await _context.AddAsync(model);

                GeneralResponse result = await _dBHelper.SaveChangesAsync(_context);
                if (result.Success)
                {
                    await transaction.CommitAsync();
                    return result;
                }
                else
                {
                    await transaction.RollbackAsync();
                    return result;
                }

            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return new GeneralResponse
                {
                    Message = ex.Message,
                    Success = false,
                };
            }
            finally
            {
                StaticCommon.ContextInUse = false;
            }
        }


        public async Task<GeneralResponse> DeleteAsync<T>(T model)
        {
            while (StaticCommon.ContextInUse)
                await Task.Delay(100);

            // usar métodos async para transacciones
            await using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                _context.Remove(model);

                GeneralResponse result = await _dBHelper.SaveChangesAsync(_context);
                if (result.Success)
                {
                    await transaction.CommitAsync();
                    return result;
                }
                else
                {
                    await transaction.RollbackAsync();
                    return result;
                }

            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return new GeneralResponse
                {
                    Message = ex.Message,
                    Success = false,
                };
            }
            finally
            {
                StaticCommon.ContextInUse = false;
            }
        }


        public async Task<GeneralResponse> UpdateAsync<T>(T model) where T : class
        {
            while (StaticCommon.ContextInUse)
                await Task.Delay(100);

            // usar métodos async para transacciones
            await using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var entityType = _context.Model.FindEntityType(typeof(T));
                var key = entityType.FindPrimaryKey();
                var keyValues = key.Properties.Select(p => p.PropertyInfo.GetValue(model)).ToArray();

                // 1) Buscar si ya está rastreada
                var trackedEntry = _context.ChangeTracker.Entries()
                    .FirstOrDefault(e => e.Entity.GetType() == typeof(T) &&
                        key.Properties.Select(p => p.PropertyInfo.GetValue(e.Entity))
                                      .SequenceEqual(keyValues));

                object dbEntity = null;

                if (trackedEntry != null)
                {
                    dbEntity = trackedEntry.Entity;
                }
                else
                {
                    // 2) Intentar cargarla desde la DB (la carga la pone en el ChangeTracker)
                    dbEntity = await _context.Set<T>().FindAsync(keyValues);
                }

                if (dbEntity != null)
                {
                    // 3) Actualizar SOLO propiedades escalar/columnas (NO colecciones navegacionales)
                    var dbEntry = _context.Entry(dbEntity);
                    var entityProperties = entityType.GetProperties() // EF Core metadata properties (scalars)
                                                .Where(p => !p.IsPrimaryKey()) // no tocar PK
                                                .ToList();

                    foreach (var p in entityProperties)
                    {
                        var propName = p.Name;
                        var value = p.PropertyInfo.GetValue(model);
                        // Asignar el valor y marcar la propiedad como modificada
                        dbEntry.Property(propName).CurrentValue = value;
                        dbEntry.Property(propName).IsModified = true;
                    }

                    // NOTA: No tocamos colecciones navegacionales (GetNavigations) aquí.
                }
                else
                {
                    // 4) Si no existe en DB -> agregar como nuevo (o cambiar a Attach si preferís)
                    _context.Set<T>().Add(model);
                }

                GeneralResponse result = await _dBHelper.SaveChangesAsync(_context);

                if (result.Success)
                {
                    await transaction.CommitAsync();
                    return result;
                }
                else
                {
                    await transaction.RollbackAsync();
                    return result;
                }
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return new GeneralResponse
                {
                    Message = ex.Message,
                    Success = false,
                };
            }
            finally
            {
                StaticCommon.ContextInUse = false;
            }
        }


    }
}
