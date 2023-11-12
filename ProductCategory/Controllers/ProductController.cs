using Microsoft.AspNetCore.Mvc;
using ProductCategoryAPI.DTO;
using ProductCategoryAPI.models;
using ProductCategoryAPI.Services;

namespace ProductCategoryAPI.Controllers
{
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
        public ProductController(IProductService productService, ICategoryService categoryService)
        {
            _productService = productService;
            _categoryService = categoryService;
        }
        [HttpGet]
        [Route("api/[controller]/Get")]
        public async Task<IEnumerable<Product>> Get()
        {
            return await _productService.Get();
        }

        [HttpGet()]
        [Route("api/[controller]/{id}")]
        public async Task<ActionResult<Product>> Get(string id)
        {
            try
            {
                var product = await _productService.Get(id);

                if (product == null)
                {
                    return NotFound($"Product not found by id:{id}!");
                }
                return Ok(product);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [HttpPost]
        [Route("api/[controller]/Create")]
        public async Task<ActionResult<Product>> Create(ProductDTO productDto)
        {
            if (productDto != null)
            {
                var category = await _categoryService.Get(productDto.CategoryId);
                if (category == null)
                {
                    return NotFound("Category not found");
                }
                var product = await _productService.Create(productDto, category);
                return CreatedAtRoute(new { id = product.Id }, product);
            }
            return NotFound("Product can not be null");
        }
        [HttpPut()]
        [Route("api/[controller]/Update/{id}")]
        public async Task<IActionResult> Update(string id, ProductDTO productDto)
        {
            try
            {
                var product = await _productService.Get(id);

                if (product == null)
                {
                    return NotFound($"Product not found by id:{id}");
                }
                var category = await _categoryService.Get(productDto.CategoryId);
                if (category == null)
                {
                    return NotFound($"Category not found by categoryId:{productDto.CategoryId}");
                }
                await _productService.Update(id, productDto, category);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                var product = await _productService.Get(id);
                if (product == null)
                {
                    return NotFound();
                }
                await _productService.Delete(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}