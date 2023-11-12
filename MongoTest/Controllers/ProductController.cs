using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using MongoTest.DTO;
using MongoTest.models;

namespace MongoTest.Controllers
{
    public class ProductController : ControllerBase
    {
        private readonly MongoDBContext _context;
        private readonly IMapper _mapper;

        public ProductController(MongoDBContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        [Route("api/[controller]/Get")]
        public async Task<IEnumerable<Product>> Get()
        {
            return await _context.Products.Find(_ => true).ToListAsync();
        }

        [HttpGet()]
        [Route("api/[controller]/{id}")]
        public async Task<ActionResult<Product>> Get(string id)
        {
            try
            {
                var product = await _context.Products.Find(p => p.Id == id).FirstOrDefaultAsync();

                if (product == null)
                {
                    return NotFound();
                }

                return product;
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
                var category = await _context.Categories.Find(p => p.Id == productDto.CategoryId).FirstOrDefaultAsync();
                if (category == null)
                {
                    return NotFound("Category not found");
                }
                var product = _mapper.Map<Product>(productDto);
                product.Category = category;
                await _context.Products.InsertOneAsync(product);
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
                var product = await _context.Products.Find(p => p.Id == id).FirstOrDefaultAsync();

                if (product == null)
                {
                    return NotFound("Product not found");
                }
                var category = await _context.Categories.Find(p => p.Id == productDto.CategoryId).FirstOrDefaultAsync();
                if (category == null)
                {
                    return NotFound("Category not found");
                }
                var productReplace = _mapper.Map<Product>(productDto);
                productReplace.Category = category;
                productReplace.Id = id;
                await _context.Products.ReplaceOneAsync(p => p.Id == id, productReplace);

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
                var product = await _context.Products.Find(p => p.Id == id).FirstOrDefaultAsync();

                if (product == null)
                {
                    return NotFound();
                }

                await _context.Products.DeleteOneAsync(p => p.Id == id);

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}