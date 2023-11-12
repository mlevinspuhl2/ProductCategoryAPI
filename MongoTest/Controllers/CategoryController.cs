using Microsoft.AspNetCore.Mvc;
using MongoTest.DTO;
using MongoTest.models;
using MongoTest.Services;

namespace MongoTest.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;
        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }
        [HttpGet]
        [Route("api/[controller]")]
        public async Task<IEnumerable<Category>> Get()
        {
            return await _categoryService.Get();
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
