using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using MongoTest.models;

namespace MongoTest.Controllers
{
    public class ProductController : ControllerBase
    {
        private readonly MongoDBContext _context;

        public ProductController(MongoDBContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("api/[controller]/Get")]
        public async Task<IEnumerable<Product>> Get()
        {
            return await _context.Products.Find(_ => true).ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> Get(string id)
        {
            var product = await _context.Products.Find(p => p.Id == id).FirstOrDefaultAsync();

            if (product == null)
            {
                return NotFound();
            }

            return product;
        }

        [HttpPost]
        [Route("api/[controller]/Create")]
        public async Task<ActionResult<Product>> Create(Product product)
        {
            await _context.Categories.InsertOneAsync(product.Category);
            await _context.Products.InsertOneAsync(product);
            return CreatedAtRoute(new { id = product.Id }, product);
        }

        [HttpPut]
        [Route("api/[controller]/Update")]
        public async Task<IActionResult> Update(Product productIn)
        {
            var product = await _context.Products.Find(p => p.Id == productIn.Id).FirstOrDefaultAsync();

            if (product == null)
            {
                return NotFound();
            }
            productIn.Category.Id = product.Category.Id;
            await _context.Products.ReplaceOneAsync(p => p.Id == productIn.Id, productIn);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var product = await _context.Products.Find(p => p.Id == id).FirstOrDefaultAsync();

            if (product == null)
            {
                return NotFound();
            }

            await _context.Products.DeleteOneAsync(p => p.Id == id);

            return NoContent();
        }
    }
}