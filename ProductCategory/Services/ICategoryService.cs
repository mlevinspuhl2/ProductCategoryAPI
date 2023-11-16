using ProductCategoryAPI.DTO;
using ProductCategoryAPI.models;

namespace ProductCategoryAPI.Services
{
    public interface ICategoryService
    {
        Task<CategoryDTO> Create(CategoryDTO categoryDto);
        Task<CategoryDTO> Delete();
        Task<CategoryDTO> Delete(string id);
        Task<IEnumerable<Category>> Get();
        Task<Category> Get(string id);
        Task<List<Category>> GetByName(string name);
        Task<CategoryDTO> Update(string id, CategoryDTO categoryDto);
    }
}