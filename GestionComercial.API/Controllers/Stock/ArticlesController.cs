using GestionComercial.API.Security;
using GestionComercial.Applications.Interfaces;
using GestionComercial.Applications.Notifications;
using GestionComercial.Domain.DTOs.Stock;
using GestionComercial.Domain.Entities.Stock;
using GestionComercial.Domain.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static GestionComercial.Domain.Constant.Enumeration;

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
        private readonly IArticlesNotifier _notifier;

        public ArticlesController(IArticleService articleService, IMasterService masterService, IArticlesNotifier notifier)
        {
            _artcicleService = articleService;
            _masterService = masterService;
            _notifier = notifier;
        }

        [HttpPost("{id:int}/notify")]
        public async Task<IActionResult> Notify(int id, [FromQuery] string nombre = "")
        {
            await _notifier.NotifyAsync(id, nombre, ChangeType.Updated);
            return Ok();
        }

        [HttpPost("AddAsync")]
        public async Task<IActionResult> AddAsync([FromBody] Article article)
        {
            GeneralResponse resultAdd = await _masterService.AddAsync(article);
            if (resultAdd.Success)
            {
                await _notifier.NotifyAsync(article.Id, article.Description, ChangeType.Created);

                return Ok("Artículo creado correctamente");
            }
            else return BadRequest(resultAdd.Message);
        }

        [HttpPost("UpdateAsync")]
        public async Task<IActionResult> UpdateAsync([FromBody] Article article)
        {
            GeneralResponse resultAdd = await _masterService.UpdateAsync(article);
            if (resultAdd.Success)
            {
                await _notifier.NotifyAsync(article.Id, article.Description, ChangeType.Updated);
                return Ok("Artículo actualizado correctamente");
            }
            else return BadRequest(resultAdd.Message);
        }

        [HttpPost("DeleteAsync")]
        public async Task<IActionResult> DeleteAsync([FromBody] ArticleFilterDto filter)
        {
            ArticleViewModel? article = await _artcicleService.GetByIdAsync(filter.Id);
            if (article == null)
                return BadRequest("No se reconoce el artículo");

            GeneralResponse resultAdd = await _artcicleService.DeleteAsync(filter.Id);
            if (resultAdd.Success)
            {
                await _notifier.NotifyAsync(article.Id, article.Description, ChangeType.Deleted);
                return Ok("Artículo borrado correctamente");
            }
            else return BadRequest(resultAdd.Message);
        }


        [HttpPost("GetAllAsync")]
        public async Task<IActionResult> GetAllAsync()
        {
            IEnumerable<ArticleViewModel> articles = await _artcicleService.GetAllAsync();
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
            ArticleViewModel? article = await _artcicleService.FindByCodeOrBarCodeAsync(filter.Code);
            if (article == null)
                return NotFound();

            return Ok(article);
        }

        [HttpPost("FindByBarCodeAsync")]
        public async Task<IActionResult> FindByBarCodeAsync([FromBody] ArticleFilterDto filter)
        {
            ArticleViewModel? article = await _artcicleService.FindByBarCodeAsync(filter.BarCode);
            if (article == null)
                return NotFound();

            return Ok(article);
        }

        [HttpPost("SearchToListAsync")]
        public async Task<IActionResult> SearchToListAsync([FromBody] ArticleFilterDto filter)
        {
            IEnumerable<ArticleViewModel> articles = await _artcicleService.SearchToListAsync(filter.Description);
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
