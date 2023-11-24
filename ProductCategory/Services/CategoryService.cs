using AutoMapper;
using MongoDB.Bson;
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
        public async Task<List<Category>> GetByName(string name)
        {
            var filter = Builders<Category>.Filter.Regex("Name", new BsonRegularExpression($"/{name}/i"));

            return await _context.Categories.Find(filter).ToListAsync();
        }
        public async Task<CategoryDTO> Create(CategoryDTO categoryDto)
        {
            var category = _mapper.Map<Category>(categoryDto);
            await _context.Categories.InsertOneAsync(category);
            return _mapper.Map<CategoryDTO>(category); ;
        }
        public async Task<CategoryDTO> Update(string id, CategoryDTO categoryDto)
        {
            var categoryReplace = _mapper.Map<Category>(categoryDto);
            categoryReplace.Id = id;


            await _context.Categories.ReplaceOneAsync(p => p.Id == id, categoryReplace);
            var filter = Builders<Product>.Filter.Eq(prod => prod.Category.Id, id);
            var products = _context.Products.Find(filter).ToListAsync().Result;
            products.ForEach(prod => prod.Category = categoryReplace);
            foreach(var prod in products)
            {
                prod.Category = categoryReplace;
                _context.Products.ReplaceOneAsync(p => p.Id == prod.Id, prod);
            }
            return _mapper.Map<CategoryDTO>(categoryReplace);
        }
        public async Task<CategoryDTO> Delete(string id)
        {
            var filter = Builders<Category>.Filter.Eq(category => category.Id, id);
            await _context.Categories.DeleteOneAsync(filter);
            return new CategoryDTO { Message = "Category removed!" };

        }
        public async Task<CategoryDTO> Delete()
        {
            var filter = Builders<Category>.Filter.Empty;
            await _context.Categories.DeleteManyAsync(filter);
            return new CategoryDTO { Message = "Categories removed!" };
        }

    }
}
