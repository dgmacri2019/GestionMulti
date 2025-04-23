using GestionComercial.API.Security;
using GestionComercial.Applications.Interfaces;
using GestionComercial.Domain.DTOs.Stock;
using GestionComercial.Domain.Entities.Stock;
using GestionComercial.Domain.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GestionComercial.API.Controllers.Stock
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    [AuthorizePermission]
    public class ArticlesController : ControllerBase
    {
        private readonly IArticleService _artcicleService;
        private static readonly string ControllerName = "Articulos-";
        public ArticlesController(IArticleService articleService)
        {
            _artcicleService = articleService;
        }



        [HttpPost("Add")]
        public IActionResult Add([FromBody] Article article)
        {
            GeneralResponse resultAdd = _artcicleService.Add(article);
            return resultAdd.Success ?
                 Ok("Articulo creado correctamente")
                :
                BadRequest(resultAdd.Message);
        }

        [HttpPost("AddAsync")]
        public async Task<IActionResult> AddAsync([FromBody] Article article)
        {
            GeneralResponse resultAdd = await _artcicleService.AddAsync(article);
            return resultAdd.Success ?
                Ok("Articulo creado correctamente")
                :
                BadRequest(resultAdd.Message);
        }



        [HttpPost("Update")]
        public IActionResult Update([FromBody] Article article)
        {
            GeneralResponse resultAdd = _artcicleService.Update(article);
            return resultAdd.Success ?
                 Ok("Articulo actualizado correctamente")
                :
                BadRequest(resultAdd.Message);
        }

        [HttpPost("UpdateAsync")]
        public async Task<IActionResult> UpdateAsync([FromBody] Article article)
        {
            GeneralResponse resultAdd = await _artcicleService.UpdateAsync(article);
            return resultAdd.Success ?
                Ok("Articulo actualizado correctamente")
                :
                BadRequest(resultAdd.Message);
        }



        [HttpPost("Delete")]
        public IActionResult Delete([FromBody] ArticleFilterDto filter)
        {
            GeneralResponse resultAdd = _artcicleService.Delete(filter.Id);
            return resultAdd.Success ?
                 Ok("Articulo borrado correctamente")
                :
                BadRequest(resultAdd.Message);
        }

        [HttpPost("DeleteAsync")]
        public async Task<IActionResult> DeleteAsync([FromBody] ArticleFilterDto filter)
        {
            GeneralResponse resultAdd = await _artcicleService.DeleteAsync(filter.Id);
            return resultAdd.Success ?
                Ok("Articulo borrado correctamente")
                :
                BadRequest(resultAdd.Message);
        }




        [HttpPost("GetAll")]
        public IActionResult GetAll([FromBody] ArticleFilterDto filter)
        {
            IEnumerable<ArticleWithPricesDto> articles = _artcicleService.GetAll(filter.IsEnabled, filter.IsDeleted);
            return Ok(articles);
        }

        //[Authorize(Policy = "PERMISO:{0}lectura")]
       
        [HttpPost("GetAllAsync")]
        public async Task<IActionResult> GetAllAsync([FromBody] ArticleFilterDto filter)
        {
            IEnumerable<ArticleWithPricesDto> articles = await _artcicleService.GetAllAsync(filter.IsEnabled, filter.IsDeleted);
            return Ok(articles);
        }



        [HttpPost("GetById")]
        public IActionResult GetById([FromBody] ArticleFilterDto filter)
        {
            ArticleWithPricesDto? article = _artcicleService.GetById(filter.Id);
            if (article == null)
                return NotFound();

            return Ok(article);
        }

        [HttpPost("GetByIdAsync")]
        public async Task<IActionResult> GetByIdAsync([FromBody] ArticleFilterDto filter)
        {
            ArticleWithPricesDto? article = await _artcicleService.GetByIdAsync(filter.Id);
            if (article == null)
                return NotFound();

            return Ok(article);
        }



        [HttpPost("FindByCodeOrBarCode")]
        public IActionResult FindByCodeOrBarCode([FromBody] ArticleFilterDto filter)
        {
            ArticleWithPricesDto? article = _artcicleService.FindByCodeOrBarCode(filter.Code);
            if (article == null)
                return NotFound();

            return Ok(article);
        }

        [HttpPost("FindByCodeOrBarCodeAsync")]
        public async Task<IActionResult> FindByCodeOrBarCodeAsync([FromBody] ArticleFilterDto filter)
        {
            ArticleWithPricesDto? article = await _artcicleService.FindByCodeOrBarCodeAsync(filter.Code);
            if (article == null)
                return NotFound();

            return Ok(article);
        }



        [HttpPost("FindByBarCode")]
        public IActionResult FindByBarCode([FromBody] ArticleFilterDto filter)
        {
            ArticleWithPricesDto? article = _artcicleService.FindByBarCode(filter.BarCode);
            if (article == null)
                return NotFound();

            return Ok(article);
        }

        [HttpPost("FindByBarCodeAsync")]
        public async Task<IActionResult> FindByBarCodeAsync([FromBody] ArticleFilterDto filter)
        {
            ArticleWithPricesDto? article = await _artcicleService.FindByBarCodeAsync(filter.BarCode);
            if (article == null)
                return NotFound();

            return Ok(article);
        }



        [HttpPost("SearchToList")]
        public IActionResult SearchToList([FromBody] ArticleFilterDto filter)
        {
            IEnumerable<ArticleWithPricesDto> articles = _artcicleService.SearchToList(filter.Description, filter.IsEnabled, filter.IsDeleted);
            return Ok(articles);
        }

        [HttpPost("SearchToListAsync")]
        public async Task<IActionResult> SearchToListAsync([FromBody] ArticleFilterDto filter)
        {
            IEnumerable<ArticleWithPricesDto> articles = await _artcicleService.SearchToListAsync(filter.Description, filter.IsEnabled, filter.IsDeleted);
            return Ok(articles);
        }



        [HttpPost("UpdatePrices")]
        public IActionResult UpdatePrices([FromBody] ArticleFilterDto filter)
        {
            GeneralResponse result = _artcicleService.UpdatePrices(filter.Progress, filter.CategoryId, filter.Percentage);
            return result.Success ?
                Ok("Articulos actualizados correctamente")
               :
               BadRequest(result.Message);
        }

        [HttpPost("UpdatePricesAsync")]
        public async Task<IActionResult> UpdatePricesAsync([FromBody] ArticleFilterDto filter)
        {
            GeneralResponse result = await _artcicleService.UpdatePricesAsync(filter.Progress, filter.CategoryId, filter.Percentage);
            return result.Success ?
               Ok("Articulos actualizados correctamente")
              :
              BadRequest(result.Message);
        }



        [HttpPost("GenerateNewBarCode")]
        public IActionResult GenerateNewBarCode()
        {
            ArticleResponse result = _artcicleService.GenerateNewBarCode();
            return result.Success ?
                Ok(result)
               :
               BadRequest(result.Message);
        }

        [HttpPost("GenerateNewBarCodeAsync")]
        public async Task<IActionResult> GenerateNewBarCodeAsync()
        {
            ArticleResponse result = await _artcicleService.GenerateNewBarCodeAsync();
            return result.Success ?
               Ok(result)
              :
              BadRequest(result.Message);
        }
    }
}
