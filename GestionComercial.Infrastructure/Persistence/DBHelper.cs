using GestionComercial.Domain.Response;
using GestionComercial.Domain.Statics;

namespace GestionComercial.Infrastructure.Persistence
{
    public class DBHelper
    {
        public DBHelper()
        {

        }


        public GeneralResponse SaveChanges(AppDbContext _context)
        {
            while (StaticCommon.ContextInUse)
                Thread.Sleep(100);

            StaticCommon.ContextInUse = true;
            try
            {
                _context.SaveChanges();
                StaticCommon.ContextInUse = false;
                return new GeneralResponse { Success = true, };
            }
            catch (AbandonedMutexException ee)
            {
                return new GeneralResponse
                {
                    Success = false,
                    Message = ee.Message,
                };
            }
            catch (AccessViolationException ee)
            {
                return new GeneralResponse
                {
                    Success = false,
                    Message = ee.Message,
                };
            }
            catch (AggregateException ee)
            {
                return new GeneralResponse
                {
                    Success = false,
                    Message = ee.Message,
                };
            }
            catch (Exception ex)
            {
                GeneralResponse response = new GeneralResponse { Success = false, };
                if (ex.InnerException != null &&
                    ex.InnerException.InnerException != null &&
                    ex.InnerException.InnerException.Message.Contains("Tax_Description_Index"))
                {
                    response.Message = "Ese nombre de impuesto ya se encuentra registrado";
                }
                else if (ex.InnerException != null &&
                    ex.InnerException.InnerException != null &&
                    ex.InnerException.InnerException.Message.Contains("CommerceData_Cuit_Index"))
                {
                    response.Message = "Ese número de CUIT ya se encuentra registrado";
                } 
                else if (ex.InnerException != null &&
                    ex.InnerException.InnerException != null &&
                    ex.InnerException.InnerException.Message.Contains("Role_Name_Index"))
                {
                    response.Message = "Ese nombre de rol ya se encuentra registrado";
                }
                else if (ex.InnerException != null &&
                    ex.InnerException.InnerException != null &&
                    ex.InnerException.InnerException.Message.Contains("User_UserName_Index"))
                {
                    response.Message = "Ese nombre de usuario ya se encuentra registrado";
                }
                else if (ex.InnerException != null &&
                    ex.InnerException.InnerException != null &&
                    ex.InnerException.InnerException.Message.Contains("User_Email_Index"))
                {
                    response.Message = "Esa cuanta de Email ya se encuentra registrada";
                }
                else if (ex.InnerException != null &&
                    ex.InnerException.InnerException != null &&
                    ex.InnerException.InnerException.Message.Contains("Product_Code_Index"))
                {
                    response.Message = "Ese código de producto ya se encuentra registrado";
                }
                else if (ex.InnerException != null &&
                    ex.InnerException.InnerException != null &&
                    ex.InnerException.InnerException.Message.Contains("Product_BarCode_Index"))
                {
                    response.Message = "Ese código de barras ya se encuentra registrado";
                }
                else if (ex.InnerException != null &&
                    ex.InnerException.InnerException != null &&
                    ex.InnerException.InnerException.Message.Contains("State_Name_Index"))
                {
                    response.Message = "Esa provincia ya se encuentra registrada";
                } 
                else if (ex.InnerException != null &&
                    ex.InnerException.InnerException != null &&
                    ex.InnerException.InnerException.Message.Contains("City_Name_Index"))
                {
                    response.Message = "Esa localidad ya se encuentra registrada";
                } 
                else if (ex.InnerException != null &&
                    ex.InnerException.InnerException != null &&
                    ex.InnerException.InnerException.Message.Contains("Permision_Name_Index"))
                {
                    response.Message = "Ese nombre de permiso ya se encuentra registrado";
                }
                else if (ex.InnerException != null &&
                   ex.InnerException.InnerException != null &&
                   ex.InnerException.InnerException.Message.Contains("_Index"))
                {
                    response.Message = "Existe un valor duplicado";
                }
                else if (ex.InnerException != null &&
                    ex.InnerException.InnerException != null &&
                    ex.InnerException.InnerException.Message.Contains("REFERENCE"))
                {
                    response.Message = "El registro no se puede borrar porque tiene registros relacionados";
                }
                else
                {
                    response.Message = ex.Message;
                }
                return response;
            }
            finally
            {
                StaticCommon.ContextInUse = false;
            }
        }


        public async Task<GeneralResponse> SaveChangesAsync(AppDbContext _context)
        {
            while (StaticCommon.ContextInUse)
                await Task.Delay(100);
            StaticCommon.ContextInUse = true;

            try
            {
                var xd = await _context.SaveChangesAsync();
                StaticCommon.ContextInUse = false;
                return new GeneralResponse { Success = true, };
            }
            catch (AbandonedMutexException ee)
            {
                return new GeneralResponse
                {
                    Success = false,
                    Message = ee.Message,
                };
            }
            catch (AccessViolationException ee)
            {
                return new GeneralResponse
                {
                    Success = false,
                    Message = ee.Message,
                };
            }
            catch (AggregateException ee)
            {
                return new GeneralResponse
                {
                    Success = false,
                    Message = ee.Message,
                };
            }
            catch (Exception ex)
            {
                GeneralResponse response = new GeneralResponse { Success = false, };
                if (ex.InnerException != null &&
                    ex.InnerException.InnerException != null &&
                    ex.InnerException.InnerException.Message.Contains("Tax_Description_Index"))
                {
                    response.Message = "Ese nombre de impuesto ya se encuentra registrado";
                }
                else if (ex.InnerException != null &&
                    ex.InnerException.InnerException != null &&
                    ex.InnerException.InnerException.Message.Contains("Role_Name_Index"))
                {
                    response.Message = "Ese nombre de rol ya se encuentra registrado";
                }
                else if (ex.InnerException != null &&
                   ex.InnerException.InnerException != null &&
                   ex.InnerException.InnerException.Message.Contains("CommerceData_Cuit_Index"))
                {
                    response.Message = "Ese número de CUIT ya se encuentra registrado";
                }
                else if (ex.InnerException != null &&
                    ex.InnerException.InnerException != null &&
                    ex.InnerException.InnerException.Message.Contains("User_UserName_Index"))
                {
                    response.Message = "Ese nombre de usuario ya se encuentra registrado";
                }
                else if (ex.InnerException != null &&
                    ex.InnerException.InnerException != null &&
                    ex.InnerException.InnerException.Message.Contains("User_Email_Index"))
                {
                    response.Message = "Esa cuanta de Email ya se encuentra registrada";
                }
                else if (ex.InnerException != null &&
                    ex.InnerException.InnerException != null &&
                    ex.InnerException.InnerException.Message.Contains("Product_Code_Index"))
                {
                    response.Message = "Ese código de producto ya se encuentra registrado";
                }
                else if (ex.InnerException != null &&
                    ex.InnerException.InnerException != null &&
                    ex.InnerException.InnerException.Message.Contains("Product_BarCode_Index"))
                {
                    response.Message = "Ese código de barras ya se encuentra registrado";
                }
                else if (ex.InnerException != null &&
                    ex.InnerException.InnerException != null &&
                    ex.InnerException.InnerException.Message.Contains("State_Name_Index"))
                {
                    response.Message = "Esa provincia ya se encuentra registrada";
                }
                else if (ex.InnerException != null &&
                    ex.InnerException.InnerException != null &&
                    ex.InnerException.InnerException.Message.Contains("Permision_Name_Index"))
                {
                    response.Message = "Ese nombre de permiso ya se encuentra registrado";
                }
                else if (ex.InnerException != null &&
                   ex.InnerException.InnerException != null &&
                   ex.InnerException.InnerException.Message.Contains("_Index"))
                {
                    response.Message = "Existe un valor duplicado";
                }
                else if (ex.InnerException != null &&
                    ex.InnerException.InnerException != null &&
                    ex.InnerException.InnerException.Message.Contains("REFERENCE"))
                {
                    response.Message = "El registro no se puede borrar porque tiene registros relacionados";
                }
                else
                {
                    response.Message = ex.Message;
                }
                return response;
            }
            finally
            {
                StaticCommon.ContextInUse = false;
            }
        }
    }
}
