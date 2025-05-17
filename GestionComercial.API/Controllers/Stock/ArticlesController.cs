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
        private readonly IMasterService _masterService;


        public ArticlesController(IArticleService articleService, IMasterService masterService)
        {
            _artcicleService = articleService;
            _masterService = masterService;
        }



        [HttpPost("AddAsync")]
        public async Task<IActionResult> AddAsync([FromBody] Article article)
        {
            GeneralResponse resultAdd = await _masterService.AddAsync(article);
            return resultAdd.Success ?
                Ok("Articulo creado correctamente")
                :
                BadRequest(resultAdd.Message);
        }

        [HttpPost("UpdateAsync")]
        public async Task<IActionResult> UpdateAsync([FromBody] Article article)
        {
            GeneralResponse resultAdd = await _masterService.UpdateAsync(article);
            return resultAdd.Success ?
                Ok("Articulo actualizado correctamente")
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




        [HttpPost("GetAllAsync")]
        public async Task<IActionResult> GetAllAsync([FromBody] ArticleFilterDto filter)
        {
            IEnumerable<ArticleWithPricesDto> articles = await _artcicleService.GetAllAsync(filter.IsEnabled, filter.IsDeleted);
            return Ok(articles);
        }

        [HttpPost("GetByIdAsync")]
        public async Task<IActionResult> GetByIdAsync([FromBody] ArticleFilterDto filter)
        {
            ArticleViewModel? article = await _artcicleService.GetByIdAsync(filter.Id);
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

        [HttpPost("FindByBarCodeAsync")]
        public async Task<IActionResult> FindByBarCodeAsync([FromBody] ArticleFilterDto filter)
        {
            ArticleWithPricesDto? article = await _artcicleService.FindByBarCodeAsync(filter.BarCode);
            if (article == null)
                return NotFound();

            return Ok(article);
        }

        [HttpPost("SearchToListAsync")]
        public async Task<IActionResult> SearchToListAsync([FromBody] ArticleFilterDto filter)
        {
            IEnumerable<ArticleWithPricesDto> articles = await _artcicleService.SearchToListAsync(filter.Description, filter.IsEnabled, filter.IsDeleted);
            return Ok(articles);
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
