using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using ProductCategoryAPI.DTO;
using ProductCategoryAPI.Services;

namespace ProductCategoryAPI.Controllers
{
    [EnableCors("MyPolicy")]
    public class CategoryController : ControllerBase
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
        public async Task<ActionResult> Get()
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
        public async Task<ActionResult> Get(string id)
        {
            try
            {
                var category = await _categoryService.Get(id);

                if (category == null)
                {
                    return NotFound();
                }
                return Ok(category);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [HttpGet()]
        [Route("api/[controller]/GetByName")]
        public async Task<ActionResult> GetByName(string name)
        {
            try
            {
                if (string.IsNullOrEmpty(name))
                {
                    return Ok(await _categoryService.Get());
                }
                 var category = await _categoryService.GetByName(name);

                if (category == null)
                {
                    return NotFound();
                }
                return Ok(category);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [HttpPost]
        [Route("api/[controller]")]
        public async Task<ActionResult> Create([FromBody]CategoryDTO categoryDto)
        {
            if (categoryDto != null)
            {
                var category = await _categoryService.Create(categoryDto);
                return Ok(category);
            }
            return NotFound();

        }
        [HttpPut()]
        [Route("api/[controller]/{id}")]
        public async Task<IActionResult> Update(string id, [FromBody]CategoryDTO categoryDto)
        {
            try
            {
                var category = await _categoryService.Get(id);
                if (category == null)
                {
                    return NotFound();
                }
                categoryDto = await _categoryService.Update(id, categoryDto);
                categoryDto.Message = "Category updated!";
                return Ok(categoryDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [HttpDelete()]
        [Route("api/[controller]/{id}")]
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
        [HttpDelete()]
        [Route("api/[controller]")]
        public async Task<IActionResult> Delete()
        {
            try
            {
                var dto = await _categoryService.Delete();
                return Ok(dto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
