using Microsoft.AspNetCore.Cors;
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
        private readonly ILogger<ProductController> _logger;
        public ProductController(IProductService productService, ICategoryService categoryService, ILogger<ProductController> logger)
        {
            _productService = productService;
            _categoryService = categoryService;
            _logger = logger;
        }
        [HttpGet]
        [Route("api/[controller]")]
        public async Task<ActionResult> Get()
        {
            return Ok(await _productService.Get());
        }

        [HttpGet()]
        [Route("api/[controller]/{id}")]
        public async Task<ActionResult> Get(string id)
        {
            try
            {
                var product = await _productService.Get(id);

                if (product == null)
                {
                    return NotFound();
                }
                return Ok(product);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [HttpPost]
        [Route("api/[controller]")]
        public async Task<ActionResult> Create([FromBody] ProductDTO productDto)
        {
            try
            {
                Product product;
                Category category = null;
                if (productDto != null)
                {
                    if (!string.IsNullOrEmpty(productDto.CategoryId))
                    {
                        category = await _categoryService.Get(productDto.CategoryId);
                        if (category == null)
                        {
                            return NotFound();
                        }

                    }
                    product = await _productService.Create(productDto, category);
                    return Ok(product);
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [HttpPut()]
        [Route("api/[controller]/{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] ProductDTO productDto)
        {
            try
            {
                Category category = null;
                var product = await _productService.Get(id);

                if (product == null)
                {
                    return NotFound();
                }
                if (productDto.CategoryId != null)
                {
                    category = await _categoryService.Get(productDto.CategoryId);
                    if (category == null)
                    {
                        return NotFound();
                    }
                }

                productDto = await _productService.Update(id, productDto, category);
                return Ok(productDto);
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