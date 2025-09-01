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
                if ((ex.InnerException != null && ex.InnerException.Message.Contains("Tax_Description_Index")) ||
                   ex.InnerException != null && ex.InnerException.InnerException != null &&
                    ex.InnerException.InnerException.Message.Contains("Tax_Description_Index"))
                {
                    response.Message = "Ese nombre de impuesto ya se encuentra registrado";
                }
                else if ((ex.InnerException != null && ex.InnerException.Message.Contains("CommerceData_Cuit_Index")) ||
                   ex.InnerException != null && ex.InnerException.InnerException != null &&
                    ex.InnerException.InnerException.Message.Contains("CommerceData_Cuit_Index"))
                {
                    response.Message = "Ese número de CUIT ya se encuentra registrado";
                }
                else if ((ex.InnerException != null && ex.InnerException.Message.Contains("Role_Name_Index")) ||
                   ex.InnerException != null && ex.InnerException.InnerException != null &&
                    ex.InnerException.InnerException.Message.Contains("Role_Name_Index"))
                {
                    response.Message = "Ese nombre de rol ya se encuentra registrado";
                }
                else if ((ex.InnerException != null && ex.InnerException.Message.Contains("User_UserName_Index")) ||
                   ex.InnerException != null && ex.InnerException.InnerException != null &&
                    ex.InnerException.InnerException.Message.Contains("User_UserName_Index"))
                {
                    response.Message = "Ese nombre de usuario ya se encuentra registrado";
                }
                else if ((ex.InnerException != null && ex.InnerException.Message.Contains("User_Email_Index")) ||
                   ex.InnerException != null && ex.InnerException.InnerException != null &&
                    ex.InnerException.InnerException.Message.Contains("User_Email_Index"))
                {
                    response.Message = "Esa cuanta de Email ya se encuentra registrada";
                }
                else if ((ex.InnerException != null && ex.InnerException.Message.Contains("Product_Code_Index")) ||
                   ex.InnerException != null && ex.InnerException.InnerException != null &&
                    ex.InnerException.InnerException.Message.Contains("Product_Code_Index"))
                {
                    response.Message = "Ese código de producto ya se encuentra registrado";
                }
                else if ((ex.InnerException != null && ex.InnerException.Message.Contains("Product_BarCode_Index")) ||
                   ex.InnerException != null && ex.InnerException.InnerException != null &&
                    ex.InnerException.InnerException.Message.Contains("Product_BarCode_Index"))
                {
                    response.Message = "Ese código de barras ya se encuentra registrado";
                }
                else if ((ex.InnerException != null && ex.InnerException.Message.Contains("Sale_SalePoint-Number_Index")) ||
                   ex.InnerException != null && ex.InnerException.InnerException != null &&
                    ex.InnerException.InnerException.Message.Contains("Sale_SalePoint-Number_Index"))
                {
                    response.Message = "Ese punto de venta y número ya se ecuentra registrado";
                }
                else if ((ex.InnerException != null && ex.InnerException.Message.Contains("State_Name_Index")) ||
                   ex.InnerException != null && ex.InnerException.InnerException != null &&
                    ex.InnerException.InnerException.Message.Contains("State_Name_Index"))
                {
                    response.Message = "Esa provincia ya se encuentra registrada";
                }
                else if ((ex.InnerException != null && ex.InnerException.Message.Contains("City_Name_Index")) ||
                   ex.InnerException != null && ex.InnerException.InnerException != null &&
                    ex.InnerException.InnerException.Message.Contains("City_Name_Index"))
                {
                    response.Message = "Esa localidad ya se encuentra registrada";
                }
                else if ((ex.InnerException != null && ex.InnerException.Message.Contains("Permision_Name_Index")) ||
                   ex.InnerException != null && ex.InnerException.InnerException != null &&
                    ex.InnerException.InnerException.Message.Contains("Permision_Name_Index"))
                {
                    response.Message = "Ese nombre de permiso ya se encuentra registrado";
                }
                else if ((ex.InnerException != null && ex.InnerException.Message.Contains("Account_AccountNumber_Index")) ||
                   ex.InnerException != null && ex.InnerException.InnerException != null &&
                    ex.InnerException.InnerException.Message.Contains("Account_AccountNumber_Index"))
                {
                    response.Message = "Esa cuanta ya se encuentra registrada";
                }
                else if ((ex.InnerException != null && ex.InnerException.Message.Contains("BillNumber_Provider_Index")) ||
                  ex.InnerException != null && ex.InnerException.InnerException != null &&
                   ex.InnerException.InnerException.Message.Contains("BillNumber_Provider_Index"))
                {
                    response.Message = "Ese número de factura ya se encuentra registrada para el mismo proveedor";
                }
                else if ((ex.InnerException != null && ex.InnerException.Message.Contains("Accounting_SeatNumber_Index")) ||
                  ex.InnerException != null && ex.InnerException.InnerException != null &&
                   ex.InnerException.InnerException.Message.Contains("Accounting_SeatNumber_Index"))
                {
                    response.Message = "Ese número de asiento contable ya se encuentra registrado para el año ingresado";
                }
                else if ((ex.InnerException != null && ex.InnerException.Message.Contains("AccountVinculation_AccountId_ViculatedAccountId_Index")) ||
                  ex.InnerException != null && ex.InnerException.InnerException != null &&
                   ex.InnerException.InnerException.Message.Contains("AccountVinculation_AccountId_ViculatedAccountId_Index"))
                {
                    response.Message = "Esa vinculación de cuentas ya se encuentra realizada";
                }
                else if ((ex.InnerException != null && ex.InnerException.Message.Contains("Bank_SaleCondition_Index")) ||
                   ex.InnerException != null && ex.InnerException.InnerException != null &&
                    ex.InnerException.InnerException.Message.Contains("Bank_SaleCondition_Index"))
                {
                    response.Message = "Ese Banco ya se encuentra vinculado a esa condición de venta";
                }
                else if ((ex.InnerException != null && ex.InnerException.Message.Contains("_Index")) ||
                   ex.InnerException != null && ex.InnerException.InnerException != null &&
                   ex.InnerException.InnerException.Message.Contains("_Index"))
                {
                    response.Message = "Existe un valor duplicado";
                }
                else if ((ex.InnerException != null && ex.InnerException.Message.Contains("REFERENCE")) ||
                   ex.InnerException != null && ex.InnerException.InnerException != null &&
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
                if ((ex.InnerException != null && ex.InnerException.Message.Contains("Tax_Description_Index")) ||
                   ex.InnerException != null && ex.InnerException.InnerException != null &&
                    ex.InnerException.InnerException.Message.Contains("Tax_Description_Index"))
                {
                    response.Message = "Ese nombre de impuesto ya se encuentra registrado";
                }
                else if ((ex.InnerException != null && ex.InnerException.Message.Contains("CommerceData_Cuit_Index")) ||
                   ex.InnerException != null && ex.InnerException.InnerException != null &&
                    ex.InnerException.InnerException.Message.Contains("CommerceData_Cuit_Index"))
                {
                    response.Message = "Ese número de CUIT ya se encuentra registrado";
                }
                else if ((ex.InnerException != null && ex.InnerException.Message.Contains("Role_Name_Index")) ||
                   ex.InnerException != null && ex.InnerException.InnerException != null &&
                    ex.InnerException.InnerException.Message.Contains("Role_Name_Index"))
                {
                    response.Message = "Ese nombre de rol ya se encuentra registrado";
                }
                else if ((ex.InnerException != null && ex.InnerException.Message.Contains("User_UserName_Index")) ||
                   ex.InnerException != null && ex.InnerException.InnerException != null &&
                    ex.InnerException.InnerException.Message.Contains("User_UserName_Index"))
                {
                    response.Message = "Ese nombre de usuario ya se encuentra registrado";
                }
                else if ((ex.InnerException != null && ex.InnerException.Message.Contains("User_Email_Index")) ||
                   ex.InnerException != null && ex.InnerException.InnerException != null &&
                    ex.InnerException.InnerException.Message.Contains("User_Email_Index"))
                {
                    response.Message = "Esa cuanta de Email ya se encuentra registrada";
                }
                else if ((ex.InnerException != null && ex.InnerException.Message.Contains("Client_Code_Index")) ||
                   ex.InnerException != null && ex.InnerException.InnerException != null &&
                    ex.InnerException.InnerException.Message.Contains("Client_Code_Index"))
                {
                    response.Message = "Ese código de cliente ya se encuentra registrado";
                }
                else if ((ex.InnerException != null && ex.InnerException.Message.Contains("Client_Document_Index")) ||
                   ex.InnerException != null && ex.InnerException.InnerException != null &&
                    ex.InnerException.InnerException.Message.Contains("Client_Document_Index"))
                {
                    response.Message = "Ese tipo y número de documento ya se encuentra registrado";
                }
                else if ((ex.InnerException != null && ex.InnerException.Message.Contains("Provider_Code_Index")) ||
                   ex.InnerException != null && ex.InnerException.InnerException != null &&
                    ex.InnerException.InnerException.Message.Contains("Provider_Code_Index"))
                {
                    response.Message = "Ese código de proveedor ya se encuentra registrado";
                }
                else if ((ex.InnerException != null && ex.InnerException.Message.Contains("Provider_Document_Index")) ||
                   ex.InnerException != null && ex.InnerException.InnerException != null &&
                    ex.InnerException.InnerException.Message.Contains("Provider_Document_Index"))
                {
                    response.Message = "Ese tipo y número de documento ya se encuentra registrado";
                }
                else if ((ex.InnerException != null && ex.InnerException.Message.Contains("Product_Code_Index")) ||
                   ex.InnerException != null && ex.InnerException.InnerException != null &&
                    ex.InnerException.InnerException.Message.Contains("Product_Code_Index"))
                {
                    response.Message = "Ese código de artículo ya se encuentra registrado";
                }
                else if ((ex.InnerException != null && ex.InnerException.Message.Contains("Product_BarCode_Index")) ||
                   ex.InnerException != null && ex.InnerException.InnerException != null &&
                    ex.InnerException.InnerException.Message.Contains("Product_BarCode_Index"))
                {
                    response.Message = "Ese código de barras ya se encuentra registrado";
                }
                else if ((ex.InnerException != null && ex.InnerException.Message.Contains("State_Name_Index")) ||
                   ex.InnerException != null && ex.InnerException.InnerException != null &&
                    ex.InnerException.InnerException.Message.Contains("State_Name_Index"))
                {
                    response.Message = "Esa provincia ya se encuentra registrada";
                }
                else if ((ex.InnerException != null && ex.InnerException.Message.Contains("City_Name_Index")) ||
                   ex.InnerException != null && ex.InnerException.InnerException != null &&
                    ex.InnerException.InnerException.Message.Contains("City_Name_Index"))
                {
                    response.Message = "Esa localidad ya se encuentra registrada";
                }
                else if ((ex.InnerException != null && ex.InnerException.Message.Contains("Permision_Name_Index")) ||
                   ex.InnerException != null && ex.InnerException.InnerException != null &&
                    ex.InnerException.InnerException.Message.Contains("Permision_Name_Index"))
                {
                    response.Message = "Ese nombre de permiso ya se encuentra registrado";
                }
                else if ((ex.InnerException != null && ex.InnerException.Message.Contains("_Index")) ||
                   ex.InnerException != null && ex.InnerException.InnerException != null &&
                   ex.InnerException.InnerException.Message.Contains("_Index"))
                {
                    response.Message = "Existe un valor duplicado";
                }
                else if ((ex.InnerException != null && ex.InnerException.Message.Contains("REFERENCE")) ||
                   ex.InnerException != null && ex.InnerException.InnerException != null &&
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
