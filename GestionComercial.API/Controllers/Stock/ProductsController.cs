using GestionComercial.Applications.Interfaces;
using GestionComercial.Domain.DTOs;
using GestionComercial.Domain.Entities.Stock;
using GestionComercial.Domain.Response;
using Microsoft.AspNetCore.Mvc;

namespace GestionComercial.API.Controllers.Stock
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }



        [HttpPost("Add")]
        public IActionResult Add([FromBody] Product product)
        {
            GeneralResponse resultAdd = _productService.Add(product);
            return resultAdd.Success ?
                 Ok("Articulo creado correctamente")
                :
                BadRequest(resultAdd.Message);
        }

        [HttpPost("AddAsync")]
        public async Task<IActionResult> AddAsync([FromBody] Product product)
        {
            GeneralResponse resultAdd = await _productService.AddAsync(product);
            return resultAdd.Success ?
                Ok("Articulo creado correctamente")
                :
                BadRequest(resultAdd.Message);
        }



        [HttpPost("Update")]
        public IActionResult Update([FromBody] Product product)
        {
            GeneralResponse resultAdd = _productService.Update(product);
            return resultAdd.Success ?
                 Ok("Articulo actualizado correctamente")
                :
                BadRequest(resultAdd.Message);
        }

        [HttpPost("UpdateAsync")]
        public async Task<IActionResult> UpdateAsync([FromBody] Product product)
        {
            GeneralResponse resultAdd = await _productService.UpdateAsync(product);
            return resultAdd.Success ?
                Ok("Articulo actualizado correctamente")
                :
                BadRequest(resultAdd.Message);
        }



        [HttpPost("Delete/{id}")]
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



        [HttpGet("WithPrice")]
        public IActionResult GetProductsWithPrices()
        {
            IEnumerable<ProductWithPricesDto> result = _productService.GetProductsWithPrices();
            return Ok(result);
        }

        [HttpGet("WithPriceAsync")]
        public async Task<IActionResult> GetProductsWithPricesAsync()
        {
            IEnumerable<ProductWithPricesDto> result = await _productService.GetProductsWithPricesAsync();
            return Ok(result);
        }



        [HttpGet("GetAll")]
        public IActionResult GetAll()
        {
            IEnumerable<Product> products = _productService.GetAll();
            return Ok(products);
        }

        [HttpGet("GetAllAsync")]
        public async Task<IActionResult> GetAllAsync()
        {
            IEnumerable<Product> products = await _productService.GetAllAsync();
            return Ok(products);
        }



        [HttpGet("GetById/{id}")]
        public IActionResult GetById(int id)
        {
            Product product = _productService.GetById(id);
            if (product == null)
                return NotFound();

            return Ok(product);
        }

        [HttpGet("GetByIdAsync/{id}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            Product product = await _productService.GetByIdAsync(id);
            if (product == null)
                return NotFound();

            return Ok(product);
        }

    }
}
