using Microsoft.AspNetCore.Mvc;
using ProductCategoryAPI.DTO;
using ProductCategoryAPI.models;
using ProductCategoryAPI.Services;

namespace ProductCategoryAPI.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;
        private readonly ILogger<CategoryController> _logger;
        public CategoryController(ICategoryService categoryService, ILogger<CategoryController> logger)
        {
            _categoryService = categoryService;
            _logger = logger;
        }
        [HttpGet]
        [Route("api/[controller]")]
        public async Task<ActionResult<IEnumerable<Category>>> Get()
        {
            try
            {
                _logger.LogInformation("Get");
                return Ok(await _categoryService.Get());
            }
            catch(Exception ex)
            {
                _logger.LogError("Get", ex);
                return StatusCode(500, ex.Message);
            }
            
        }
        [HttpGet()]
        [Route("api/[controller]/{id}")]
        public async Task<ActionResult<Category>> Get(string id)
        {
            try
            {
                var category = await _categoryService.Get(id);

                if (category == null)
                {
                    return NotFound();
                }
                return category;
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [HttpPost]
        [Route("api/[controller]/Create")]
        public async Task<ActionResult<Category>> Create(CategoryDTO categoryDto)
        {
            if (categoryDto != null)
            {
                var category = await _categoryService.Create(categoryDto);
                return CreatedAtRoute(new { id = category.Id }, category);
            }
            return NotFound("Category can not be null");

        }
        [HttpPut()]
        [Route("api/[controller]/Update/{id}")]
        public async Task<IActionResult> Update(string id, CategoryDTO categoryDto)
        {
            try
            {
                var category = await _categoryService.Get(id);
                if (category == null)
                {
                    return NotFound();
                }
                await _categoryService.Update(id, categoryDto);

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [HttpDelete()]
        [Route("api/[controller]/Delete/{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                var category = await _categoryService.Get(id);

                if (category == null)
                {
                    return NotFound();
                }
                await _categoryService.Delete(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
