using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using MongoTest.DTO;
using MongoTest.models;

namespace MongoTest.Controllers
{
    public class CategoryController : Controller
    {
        private readonly MongoDBContext _context;
        private readonly IMapper _mapper;

        public CategoryController(MongoDBContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        [HttpGet]
        [Route("api/[controller]")]
        public async Task<IEnumerable<Category>> Get()
        {
            return await _context.Categories.Find(_ => true).ToListAsync();
        }
        [HttpGet()]
        [Route("api/[controller]/{id}")]
        public async Task<ActionResult<Category>> Get(string id)
        {
            try
            {
                var category = await _context.Categories.Find(p => p.Id == id).FirstOrDefaultAsync();

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
                var category = _mapper.Map<Category>(categoryDto);
                await _context.Categories.InsertOneAsync(category);
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
                var category = await _context.Categories.Find(p => p.Id == id).FirstOrDefaultAsync();
                if (category == null)
                {
                    return NotFound();
                }
                var categoryReplace = _mapper.Map<Category>(categoryDto);
                categoryReplace.Id = id;
                await _context.Categories.ReplaceOneAsync(p => p.Id == id, categoryReplace);

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
                var category = await _context.Categories.Find(p => p.Id == id).FirstOrDefaultAsync();

                if (category == null)
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
