using MongoTest.DTO;
using MongoTest.models;

namespace MongoTest.Services
{
    public interface ICategoryService
    {
        Task<Category> Create(CategoryDTO categoryDto);
        Task Delete(string id);
        Task<IEnumerable<Category>> Get();
        Task<Category> Get(string id);
        Task Update(string id, CategoryDTO categoryDto);
    }
}