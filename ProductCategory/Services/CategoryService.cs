using AutoMapper;
using MongoDB.Driver;
using ProductCategoryAPI.DTO;
using ProductCategoryAPI.models;

namespace ProductCategoryAPI.Services
{
    public class CategoryService : BasicService, ICategoryService
    {
        public CategoryService(MongoDBContext context, IMapper mapper) : base(context, mapper)
        {
        }
        public async Task<IEnumerable<Category>> Get()
        {
            return await _context.Categories.Find(_ => true).ToListAsync();
        }
        public async Task<Category> Get(string id)
        {
            return await _context.Categories.Find(p => p.Id == id).FirstOrDefaultAsync();
        }
        public async Task<Category> Create(CategoryDTO categoryDto)
        {
            var category = _mapper.Map<Category>(categoryDto);
            await _context.Categories.InsertOneAsync(category);
            return category;
        }
        public async Task Update(string id, CategoryDTO categoryDto)
        {
            var categoryReplace = _mapper.Map<Category>(categoryDto);
            categoryReplace.Id = id;
            await _context.Categories.ReplaceOneAsync(p => p.Id == id, categoryReplace);
        }
        public async Task Delete(string id)
        {
            var filter = Builders<Category>.Filter.Eq(category => category.Id, id);
            await _context.Categories.DeleteOneAsync(filter);
        }
    }
}
