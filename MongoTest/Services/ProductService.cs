using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using MongoTest.DTO;
using MongoTest.models;

namespace MongoTest.Services
{
    public class ProductService : BasicService, IProductService
    {
        public ProductService(MongoDBContext context, IMapper mapper) : base(context, mapper)
        {
        }
        public async Task<IEnumerable<Product>> Get()
        {
            return await _context.Products.Find(_ => true).ToListAsync();
        }
        public async Task<Product> Get(string id)
        {
            return await _context.Products.Find(p => p.Id == id).FirstOrDefaultAsync();
        }
        public async Task<Product> Create(ProductDTO productDto, Category category)
        {
            var product = _mapper.Map<Product>(productDto);
            product.Category = category;
            await _context.Products.InsertOneAsync(product);
            return product;
        }
        public async Task Update(string id, ProductDTO productDto, Category category)
        {
            var productReplace = _mapper.Map<Product>(productDto);
            productReplace.Category = category;
            productReplace.Id = id;
            await _context.Products.ReplaceOneAsync(p => p.Id == id, productReplace);
        }
        public async Task Delete(string id)
        {
            var filter = Builders<Product>.Filter.Eq(category => category.Id, id);
            await _context.Products.DeleteOneAsync(filter);
        }
    }
}
