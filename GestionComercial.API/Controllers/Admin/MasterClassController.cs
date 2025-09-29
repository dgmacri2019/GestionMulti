using GestionComercial.API.Security;
using GestionComercial.Applications.Interfaces;
using GestionComercial.Applications.Notifications;
using GestionComercial.Domain.DTOs.Master.Configurations.Commerce;
using GestionComercial.Domain.DTOs.PriceLists;
using GestionComercial.Domain.Entities.Afip;
using GestionComercial.Domain.Entities.Masters;
using GestionComercial.Domain.Entities.Stock;
using GestionComercial.Domain.Helpers;
using GestionComercial.Domain.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static GestionComercial.Domain.Constant.Enumeration;

namespace GestionComercial.API.Controllers.Admin
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    [AuthorizePermission]
    public class MasterClassController : ControllerBase
    {
        private readonly IMasterClassService _masterClassService;
        private readonly IMasterService _masterService;
        private readonly IMasterClassNotifier _notifier;
        private readonly IArticlesNotifier _articlesNotifier;

        public MasterClassController(IMasterClassService masterClassService, IMasterService masterService, IMasterClassNotifier notifier,
            IArticlesNotifier articlesNotifier)
        {
            _masterClassService = masterClassService;
            _masterService = masterService;
            _notifier = notifier;
            _articlesNotifier = articlesNotifier;
        }



        #region Category

        [HttpPost("AddCategoryAsync")]
        public async Task<IActionResult> AddCategoryAsync([FromBody] Category category)
        {
            GeneralResponse resultAdd = await _masterService.AddAsync(category);
            if (resultAdd.Success)
            {
                await _notifier.NotifyAsync(category.Id, "Rubro creado", ChangeType.Created, ChangeClass.Category);

                return
                    Ok("Rubro creado correctamente");
            }
            else return BadRequest(resultAdd.Message);
        }

        [HttpPost("UpdateCategoryAsync")]
        public async Task<IActionResult> UpdateCategoryAsync([FromBody] Category category)
        {
            GeneralResponse resultAdd = await _masterService.UpdateAsync(category);
            if (resultAdd.Success)
            {
                List<int> articlesId = [];

                await _notifier.NotifyAsync(category.Id, "Rubro actualizado", ChangeType.Updated, ChangeClass.Category);
                Category categoryUpdated = await _masterClassService.GetCategoryByIdAsync(category.Id);
                foreach (Article article in categoryUpdated.Articles)
                    articlesId.Add(article.Id);
                if (articlesId.Count > 0)
                    await _articlesNotifier.NotifyAsync(articlesId, "Rubro Actualizado", ChangeType.Updated);

                return
                    Ok("Rubro actualizado correctamente");
            }
            else return BadRequest(resultAdd.Message);
        }


        [HttpPost("GetAllCategoriesAsync")]
        public async Task<IActionResult> GetAllCategoriesAsync([FromBody] PriceListFilterDto filter)
        {
            return Ok(await _masterClassService.GetAllCategoriesAsync());
        }

        [HttpPost("GetCategoryByIdAsync")]
        public async Task<IActionResult> GetCategoryByIdAsync([FromBody] PriceListFilterDto filter)
        {
            return Ok(await _masterClassService.GetCategoryByIdAsync(filter.Id));
        }

        #endregion

        #region PriceList

        [HttpPost("AddPriceListAsync")]
        public async Task<IActionResult> AddPriceList([FromBody] PriceList priceList)
        {
            GeneralResponse resultAdd = await _masterService.AddAsync(priceList);
            if (resultAdd.Success)
            {
                await _notifier.NotifyAsync(priceList.Id, "Lista de precios creada", ChangeType.Created, ChangeClass.PriceList);

                List<int> articlesId = await _masterClassService.GetAllArticlesId();
                await _articlesNotifier.NotifyAsync(articlesId, "Lista de precios creada", ChangeType.Updated);

                return
                    Ok("Lista de precios creada correctamente");
            }
            else return BadRequest(resultAdd.Message);
        }


        [HttpPost("UpdatePriceListAsync")]
        public async Task<IActionResult> UpdatePriceListAsync([FromBody] PriceList priceList)
        {
            GeneralResponse resultAdd = await _masterService.UpdateAsync(priceList);
            if (resultAdd.Success)
            {
                await _notifier.NotifyAsync(priceList.Id, "Lista de precios actualizada", ChangeType.Updated, ChangeClass.PriceList);
                List<int> articlesId = await _masterClassService.GetAllArticlesId();
                await _articlesNotifier.NotifyAsync(articlesId, "Lista de precios actualizada", ChangeType.Updated);
                return
                    Ok("Lista de precios actualizada correctamente");
            }
            else return BadRequest(resultAdd.Message);
        }

        [HttpPost("GetAllPriceListAsync")]
        public async Task<IActionResult> GetAllPriceListAsync([FromBody] PriceListFilterDto filter)
        {
            return Ok(await _masterClassService.GetAllPriceListAsync());
        }


        [HttpPost("GetPriceListByIdAsync")]
        public async Task<IActionResult> GetByIdAsync([FromBody] PriceListFilterDto filter)
        {
            return Ok(await _masterClassService.GetPriceListByIdAsync(filter.Id));
        }

        #endregion

        #region CommerceData

        [HttpPost("AddOrUpdateCommerceDataAsync")]
        public async Task<IActionResult> AddOrUpdateCommerceDataAsync([FromBody] CommerceData commerceData)
        {
            CommerceData? commerceDataCheck = await _masterClassService.GetCommerceDataAsync();

            if (commerceDataCheck != null && commerceDataCheck.LogoByteArray != null && commerceData.LogoByteArray == null)
                commerceData.LogoByteArray = commerceDataCheck.LogoByteArray;
            //if (commerceData.LogoByteArray != null)
            //{
            //    if (!Directory.Exists(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Archivos", "Imagenes", "Logo")))
            //        Directory.CreateDirectory(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Archivos", "Imagenes", "Logo"));

            //    string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Archivos", "Imagenes", "Logo", "logo.png");

            //    if (System.IO.File.Exists(path))
            //        System.IO.File.Delete(path);

            //    if (System.IO.File.Exists(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Archivos", "Imagenes", "Logo", "logo.png")))
            //        System.IO.File.Delete(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Archivos", "Imagenes", "Logo", "logo.png"));


            //    GeneralResponse resultAddFile = FileHelper.SaveByteArrayToFile(commerceData.LogoByteArray, path);
            //    if (!resultAddFile.Success)
            //        return BadRequest(resultAddFile.Message);
            //    commerceData.LogoPath = path;
            //}

            GeneralResponse resultAdd = commerceDataCheck == null ?
                await _masterService.AddAsync(commerceData)
                :
                await _masterService.UpdateAsync(commerceData);

            if (resultAdd.Success)
                await _notifier.NotifyAsync(commerceData.Id, "Datos Comerciales guardados", ChangeType.Updated, ChangeClass.CommerceData);

            return resultAdd.Success ?
                     Ok("Datos Comerciales guardados correctamente")
                     :
                     BadRequest(resultAdd.Message);
        }

        [HttpPost("GetCommerceDataAsync")]
        public async Task<IActionResult> GetCommerceDataAsync([FromBody] PriceListFilterDto filter)
        {
            CommerceData? commerceData = await _masterClassService.GetCommerceDataAsync();
            return commerceData == null ? Ok(new CommerceData { Id = -1 }) : Ok(commerceData);
        }


        [HttpPost("AddOrUpdateBillingAsync")]
        //public async Task<IActionResult> AddOrUpdateBillingAsync([FromForm] BillingViewModel billingViewModel,
        //    [FromForm] IFormFile? file)
        public async Task<IActionResult> AddOrUpdateBillingAsync([FromBody] BillingViewModel billingViewModel)
        {
            try
            {
                if (billingViewModel == null)
                    return BadRequest("No se pudieron recibir correctamente los datos. Por favor reintente nuevamente");
                CommerceData commerceData = await _masterClassService.GetCommerceDataAsync();
                if (commerceData == null)
                    return BadRequest("Debe cargar primero los datos comerciales de la empresa");

                BillingViewModel? billingCheck = await _masterClassService.GetBillingAsync();

                if (billingViewModel.CertificateByteArray != null)
                {
                    if (!Directory.Exists(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Archivos", "Certificados")))
                        Directory.CreateDirectory(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Archivos", "Certificados"));

                    string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Archivos", "Certificados", string.Format("{0}.pfx", commerceData.CUIT));

                    if (System.IO.File.Exists(path))
                        System.IO.File.Delete(path);

                    if (System.IO.File.Exists(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Archivos", "Certificados", "0.pdf")))
                        System.IO.File.Delete(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Archivos", "Certificados", "0.pdf"));


                    GeneralResponse resultAddFile = FileHelper.SaveByteArrayToFile(billingViewModel.CertificateByteArray, path);
                    if (!resultAddFile.Success)
                        return BadRequest(resultAddFile.Message);
                }
                Billing billing = ConverterHelper.ToBilling(billingViewModel, billingViewModel.Id == 0);
                billing.CommerceDataId = commerceData.Id;

                GeneralResponse resultAdd = billingCheck == null ?
                    await _masterService.AddAsync(billing)
                    :
                    await _masterService.UpdateAsync(billing);

                if (resultAdd.Success)
                    await _notifier.NotifyAsync(billingViewModel.Id, "Datos fiscales guardados", ChangeType.Updated, ChangeClass.CommerceData);

                return resultAdd.Success ?
                         Ok("Datos fiscales guardados correctamente")
                         :
                         BadRequest(resultAdd.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }



        [HttpPost("GetBillingAsync")]
        public async Task<IActionResult> GetBillingAsync([FromBody] PriceListFilterDto filter)
        {
            BillingViewModel? billing = await _masterClassService.GetBillingAsync();
            return billing == null ? Ok(new BillingViewModel { Id = -1 }) : Ok(billing);
        }



        #endregion







        [HttpPost("AddStateAsync")]
        public async Task<IActionResult> AddStateAsync([FromBody] State state)
        {
            GeneralResponse resultAdd = await _masterService.AddAsync(state);
            if (resultAdd.Success)
            {
                await _notifier.NotifyAsync(state.Id, "Provencia Creada", ChangeType.Created, ChangeClass.State);

                return
                    Ok("Provincia creada correctamente");
            }
            else return BadRequest(resultAdd.Message);
        }

        [HttpPost("UpdateStateAsync")]
        public async Task<IActionResult> UpdateStateAsync([FromBody] State state)
        {
            GeneralResponse resultAdd = await _masterService.UpdateAsync(state);
            if (resultAdd.Success)
            {
                await _notifier.NotifyAsync(state.Id, "Pronvincia Actualizada", ChangeType.Updated, ChangeClass.State);

                return
                    Ok("Provincia actualizada correctamente");
            }
            else return BadRequest(resultAdd.Message);
        }





        [HttpPost("AddDocumentTypeAsync")]
        public async Task<IActionResult> AddDocumentTypeAsync([FromBody] DocumentType documentType)
        {
            GeneralResponse resultAdd = await _masterService.AddAsync(documentType);
            if (resultAdd.Success)
            {
                await _notifier.NotifyAsync(documentType.Id, "Tipo de documento", ChangeType.Created, ChangeClass.DocumentType);

                return
                    Ok("Tipo de decumento creado correctamente");
            }
            else return BadRequest(resultAdd.Message);
        }

        [HttpPost("UpdateDocumentTypeAsync")]
        public async Task<IActionResult> UpdateDocumentTypeAsync([FromBody] DocumentType documentType)
        {
            GeneralResponse resultAdd = await _masterService.UpdateAsync(documentType);
            if (resultAdd.Success)
            {
                await _notifier.NotifyAsync(documentType.Id, "Tipo de documento actualizado", ChangeType.Updated, ChangeClass.DocumentType);

                return
                    Ok("Tipo de documento actualizado correctamente");
            }
            else return BadRequest(resultAdd.Message);
        }



        [HttpPost("AddIvaConditionAsync")]
        public async Task<IActionResult> AddIvaConditionAsync([FromBody] IvaCondition ivaCondition)
        {
            GeneralResponse resultAdd = await _masterService.AddAsync(ivaCondition);
            if (resultAdd.Success)
            {
                await _notifier.NotifyAsync(ivaCondition.Id, "Tipo de iva crado", ChangeType.Created, ChangeClass.IvaCondition);

                return
                    Ok("Tipo de iva creado correctamente");
            }
            else return BadRequest(resultAdd.Message);
        }

        [HttpPost("UpdateIvaConditionAsync")]
        public async Task<IActionResult> UpdateIvaConditionAsync([FromBody] IvaCondition ivaCondition)
        {
            GeneralResponse resultAdd = await _masterService.UpdateAsync(ivaCondition);
            if (resultAdd.Success)
            {
                await _notifier.NotifyAsync(ivaCondition.Id, "Condición de venta actualizada", ChangeType.Updated, ChangeClass.IvaCondition);

                return
                    Ok("Condición de venta actualizada correctamente");
            }
            else return BadRequest(resultAdd.Message);
        }



        [HttpPost("AddSaleConditionAsync")]
        public async Task<IActionResult> AddSaleConditionAsync([FromBody] SaleCondition saleCondition)
        {
            GeneralResponse resultAdd = await _masterService.AddAsync(saleCondition);
            if (resultAdd.Success)
            {
                await _notifier.NotifyAsync(saleCondition.Id, "Condición de venta creada", ChangeType.Created, ChangeClass.SaleCondition);

                return
                    Ok("Condición de venta creada correctamente");
            }
            else return BadRequest(resultAdd.Message);
        }

        [HttpPost("UpdateSaleConditionAsync")]
        public async Task<IActionResult> UpdateSaleConditionAsync([FromBody] SaleCondition saleCondition)
        {
            GeneralResponse resultAdd = await _masterService.UpdateAsync(saleCondition);
            if (resultAdd.Success)
            {
                await _notifier.NotifyAsync(saleCondition.Id, "Condición de venta actualizada", ChangeType.Updated, ChangeClass.SaleCondition);

                return
                    Ok("Condición de venta actualizada correctamente");
            }
            else return BadRequest(resultAdd.Message);
        }


        [HttpPost("AddMeasureAsync")]
        public async Task<IActionResult> AddMeasureAsync([FromBody] Measure measure)
        {
            GeneralResponse resultAdd = await _masterService.AddAsync(measure);
            if (resultAdd.Success)
            {
                await _notifier.NotifyAsync(measure.Id, "Unidad de medida creada", ChangeType.Created, ChangeClass.Measure);

                return
                    Ok("Unidad de medida creada correctamente");
            }
            else return BadRequest(resultAdd.Message);
        }

        [HttpPost("UpdateMeasureAsync")]
        public async Task<IActionResult> UpdateMeasureAsync([FromBody] Measure measure)
        {
            GeneralResponse resultAdd = await _masterService.UpdateAsync(measure);
            if (resultAdd.Success)
            {
                await _notifier.NotifyAsync(measure.Id, "Unidad de medida actualizada", ChangeType.Updated, ChangeClass.Measure);

                return
                    Ok("Unidad de medida actualizada correctamente");
            }
            else return BadRequest(resultAdd.Message);
        }


        [HttpPost("AddTaxAsync")]
        public async Task<IActionResult> AddTaxAsync([FromBody] Tax tax)
        {
            GeneralResponse resultAdd = await _masterService.AddAsync(tax);
            if (resultAdd.Success)
            {
                await _notifier.NotifyAsync(tax.Id, "Tipo de IVA creado", ChangeType.Created, ChangeClass.Tax);

                return
                    Ok("Tipo de IVA creado correctamente");
            }
            else return BadRequest(resultAdd.Message);
        }

        [HttpPost("UpdateTaxAsync")]
        public async Task<IActionResult> UpdateTaxAsync([FromBody] Tax tax)
        {
            GeneralResponse resultAdd = await _masterService.UpdateAsync(tax);
            if (resultAdd.Success)
            {
                await _notifier.NotifyAsync(tax.Id, "Tipo de IVA actualizado", ChangeType.Updated, ChangeClass.Tax);

                return
                    Ok("Tipo de IVA actualizado correctamente");
            }
            else return BadRequest(resultAdd.Message);
        }






        [HttpPost("GetAllStatesAsync")]
        public async Task<IActionResult> GetAllStatesAsync([FromBody] PriceListFilterDto filter)
        {
            return Ok(await _masterClassService.GetAllStatesAsync(filter.IsEnabled, filter.IsDeleted));
        }

        [HttpPost("GetAllDocumentTypesAsync")]
        public async Task<IActionResult> GetAllDocumentTypesAsync([FromBody] PriceListFilterDto filter)
        {
            return Ok(await _masterClassService.GetAllDocumentTypesAsync(filter.IsEnabled, filter.IsDeleted));
        }

        [HttpPost("GetAllIvaConditionsAsync")]
        public async Task<IActionResult> GetAllIvaConditionsAsync([FromBody] PriceListFilterDto filter)
        {
            return Ok(await _masterClassService.GetAllIvaConditionsAsync(filter.IsEnabled, filter.IsDeleted));
        }





        [HttpPost("GetAllMeasuresAsync")]
        public async Task<IActionResult> GetAllMeasuresAsync([FromBody] PriceListFilterDto filter)
        {
            return Ok(await _masterClassService.GetAllMeasuresAsync(filter.IsEnabled, filter.IsDeleted));
        }

        [HttpPost("GetAllTaxesAsync")]
        public async Task<IActionResult> GetAllTaxesAsync([FromBody] PriceListFilterDto filter)
        {
            return Ok(await _masterClassService.GetAllTaxesAsync(filter.IsEnabled, filter.IsDeleted));
        }

        [HttpPost("GetAllSaleConditionsAsync")]
        public async Task<IActionResult> GetAllSaleConditionsAsync([FromBody] PriceListFilterDto filter)
        {
            return Ok(await _masterClassService.GetAllSaleConditionsAsync(filter.IsEnabled, filter.IsDeleted));
        }




    }
}
