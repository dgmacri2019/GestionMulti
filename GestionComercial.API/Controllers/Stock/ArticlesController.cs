using GestionComercial.Applications.Interfaces;
using GestionComercial.Domain.DTOs;
using GestionComercial.Domain.Entities.Stock;
using GestionComercial.Domain.Response;
using Microsoft.AspNetCore.Mvc;

namespace GestionComercial.API.Controllers.Stock
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArticlesController : ControllerBase
    {
        private readonly IArticleService _productService;

        public ArticlesController(IArticleService productService)
        {
            _productService = productService;
        }



        [HttpPost("Add")]
        public IActionResult Add([FromBody] Article product)
        {
            GeneralResponse resultAdd = _productService.Add(product);
            return resultAdd.Success ?
                 Ok("Articulo creado correctamente")
                :
                BadRequest(resultAdd.Message);
        }

        [HttpPost("AddAsync")]
        public async Task<IActionResult> AddAsync([FromBody] Article product)
        {
            GeneralResponse resultAdd = await _productService.AddAsync(product);
            return resultAdd.Success ?
                Ok("Articulo creado correctamente")
                :
                BadRequest(resultAdd.Message);
        }



        [HttpPost("Update")]
        public IActionResult Update([FromBody] Article product)
        {
            GeneralResponse resultAdd = _productService.Update(product);
            return resultAdd.Success ?
                 Ok("Articulo actualizado correctamente")
                :
                BadRequest(resultAdd.Message);
        }

        [HttpPost("UpdateAsync")]
        public async Task<IActionResult> UpdateAsync([FromBody] Article product)
        {
            GeneralResponse resultAdd = await _productService.UpdateAsync(product);
            return resultAdd.Success ?
                Ok("Articulo actualizado correctamente")
                :
                BadRequest(resultAdd.Message);
        }



        [HttpPost("Delete")]
        public IActionResult Delete([FromBody] int id)
        {
            GeneralResponse resultAdd = _productService.Delete(id);
            return resultAdd.Success ?
                 Ok("Articulo borrado correctamente")
                :
                BadRequest(resultAdd.Message);
        }

        [HttpPost("DeleteAsync")]
        public async Task<IActionResult> DeleteAsync([FromBody] int id)
        {
            GeneralResponse resultAdd = await _productService.DeleteAsync(id);
            return resultAdd.Success ?
                Ok("Articulo borrado correctamente")
                :
                BadRequest(resultAdd.Message);
        }




        [HttpPost("GetAll")]
        public IActionResult GetAll([FromBody] bool isEnabled, bool isDeleted)
        {
            IEnumerable<ProductWithPricesDto> products = _productService.GetAll(isEnabled, isDeleted);
            return Ok(products);
        }

        [HttpPost("GetAllAsync")]
        public async Task<IActionResult> GetAllAsync([FromBody] bool isEnabled, bool isDeleted)
        {
            IEnumerable<ProductWithPricesDto> products = await _productService.GetAllAsync(isEnabled, isDeleted);
            return Ok(products);
        }



        [HttpPost("GetById")]
        public IActionResult GetById([FromBody] int id)
        {
            ProductWithPricesDto product = _productService.GetById(id);
            if (product == null)
                return NotFound();

            return Ok(product);
        }

        [HttpPost("GetByIdAsync")]
        public async Task<IActionResult> GetByIdAsync([FromBody] int id)
        {
            ProductWithPricesDto product = await _productService.GetByIdAsync(id);
            if (product == null)
                return NotFound();

            return Ok(product);
        }



        [HttpPost("FindByCodeOrBarCode")]
        public IActionResult FindByCodeOrBarCode([FromBody] string code)
        {
            ProductWithPricesDto? product = _productService.FindByCodeOrBarCode(code);
            if (product == null)
                return NotFound();

            return Ok(product);
        }

        [HttpPost("FindByCodeOrBarCodeAsync")]
        public async Task<IActionResult> FindByCodeOrBarCodeAsync([FromBody] string code)
        {
            ProductWithPricesDto? product = await _productService.FindByCodeOrBarCodeAsync(code);
            if (product == null)
                return NotFound();

            return Ok(product);
        }



        [HttpPost("FindByBarCode")]
        public IActionResult FindByBarCode([FromBody] string barCode)
        {
            ProductWithPricesDto? product = _productService.FindByBarCode(barCode);
            if (product == null)
                return NotFound();

            return Ok(product);
        }

        [HttpPost("FindByBarCodeAsync")]
        public async Task<IActionResult> FindByBarCodeAsync([FromBody] string barCode)
        {
            ProductWithPricesDto? product = await _productService.FindByBarCodeAsync(barCode);
            if (product == null)
                return NotFound();

            return Ok(product);
        }



        [HttpPost("SearchToList")]
        public IActionResult SearchToList([FromBody] string description, bool isEnabled, bool isDeleted)
        {
            IEnumerable<ProductWithPricesDto> products = _productService.SearchToList(description, isEnabled, isDeleted);
            return Ok(products);
        }

        [HttpPost("SearchToListAsync")]
        public async Task<IActionResult> SearchToListAsync([FromBody] string description, bool isEnabled, bool isDeleted)
        {
            IEnumerable<ProductWithPricesDto> products = await _productService.SearchToListAsync(description, isEnabled, isDeleted);
            return Ok(products);
        }



        [HttpPost("UpdatePrices")]
        public IActionResult UpdatePrices([FromBody] IProgress<int> progress, int categoryId, int percentage)
        {
            GeneralResponse result = _productService.UpdatePrices(progress, categoryId, percentage);
            return result.Success ?
                Ok("Articulos actualizados correctamente")
               :
               BadRequest(result.Message);
        }

        [HttpPost("UpdatePricesAsync")]
        public async Task<IActionResult> UpdatePricesAsync([FromBody] IProgress<int> progress, int categoryId, int percentage)
        {
            GeneralResponse result = await _productService.UpdatePricesAsync(progress, categoryId, percentage);
            return result.Success ?
               Ok("Articulos actualizados correctamente")
              :
              BadRequest(result.Message);
        }



        [HttpPost("GenerateNewBarCode")]
        public IActionResult GenerateNewBarCode()
        {
            ProductResponse result = _productService.GenerateNewBarCode();
            return result.Success ?
                Ok(result)
               :
               BadRequest(result.Message);
        }

        [HttpPost("GenerateNewBarCodeAsync")]
        public async Task<IActionResult> GenerateNewBarCodeAsync()
        {
            ProductResponse result = await _productService.GenerateNewBarCodeAsync();
            return result.Success ?
               Ok(result)
              :
              BadRequest(result.Message);
        }
    }
}
