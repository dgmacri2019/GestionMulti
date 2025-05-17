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

            using (var transacction = _context.Database.BeginTransaction())
            {
                try
                {
                    await _context.AddAsync(model);

                    GeneralResponse result = await _dBHelper.SaveChangesAsync(_context);
                    if (result.Success)
                    {

                        transacction.Commit();
                        return result;
                    }
                    else
                    {
                        transacction.Rollback();
                        return result;
                    }

                }
                catch (Exception ex)
                {
                    transacction.Rollback();
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

        public async Task<GeneralResponse> DeleteAsync<T>(T model)
        {
            while (StaticCommon.ContextInUse)
                await Task.Delay(100);

            using (var transacction = _context.Database.BeginTransaction())
            {
                try
                {
                    _context.Remove(model);

                    GeneralResponse result = await _dBHelper.SaveChangesAsync(_context);
                    if (result.Success)
                    {

                        transacction.Commit();
                        return result;
                    }
                    else
                    {
                        transacction.Rollback();
                        return result;
                    }

                }
                catch (Exception ex)
                {
                    transacction.Rollback();
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

        public async Task<GeneralResponse> UpdateAsync<T>(T model)
        {
            while (StaticCommon.ContextInUse)
                await Task.Delay(100);

            using (var transacction = _context.Database.BeginTransaction())
            {
                try
                {
                    _context.Update(model);

                    GeneralResponse result = await _dBHelper.SaveChangesAsync(_context);
                    if (result.Success)
                    {

                        transacction.Commit();
                        return result;
                    }
                    else
                    {
                        transacction.Rollback();
                        return result;
                    }

                }
                catch (Exception ex)
                {
                    transacction.Rollback();
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
}
