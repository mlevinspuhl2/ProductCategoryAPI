using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using ProductCategoryAPI.DTO;
using ProductCategoryAPI.models;
using ProductCategoryAPI.Services;

namespace TestProductCategory
{
    [Collection("Our Test Collection #1")]
    public class CategoryServiceTest
    {
        private IHost _host;
        private ICategoryService? _categoryService;
        private readonly MongoDBContext? _dbcontext;
        private Category _categoryData;

        public CategoryServiceTest()
        {
            _host = GetWorkerService().Build();
            _categoryService = _host.Services.GetService<ICategoryService>();
            _dbcontext = _host.Services.GetService<MongoDBContext>();
            
        }
        private async Task<CategoryDTO> GetCategoryData(ICategoryService? _categoryService)
        {
            var dto = new CategoryDTO
            {
                Name = "Category Name",
                Description = "Category Description"
            };
            return  await _categoryService.Create(dto);

        }
        [Fact]
        public async void Get_Test()
        {
            // Arrange
            var count = 1;
            //DeleteAll();
            var dto = await GetCategoryData(_categoryService);
            // Act
            var result = await _categoryService.Get();
            // Assert
            Assert.NotNull(result);
            //Assert.Equal(count,result.Count());
            DeleteAll();
        }
        [Fact]
        public async void GetbyId_Test()
        {
            // Arrange
           // DeleteAll();
            var dto = await GetCategoryData(_categoryService);
            var id = _categoryData.Id;
            // Act
            var result = await _categoryService.Get(id);
            // Assert
            Assert.NotNull(result);
            Assert.Equal(id, result.Id);
            DeleteAll();
        }
        [Fact]
        public async void Create_Test()
        {
            // Arrange
            //DeleteAll();
            var dto = await GetCategoryData(_categoryService);
            dto = new CategoryDTO
            {
                Name = "Name Test",
                Description = "Description Test"
            };
            // Act
            var result = await _categoryService.Create(dto);
            // Assert
            Assert.NotNull(result);
            Assert.Equal(dto.Description, result.Description);
            DeleteAll();
        }
        [Fact]
        public async void Update_Test()
        {
            // Arrange
            //DeleteAll();
            var dto = await GetCategoryData(_categoryService);
            dto = new CategoryDTO
            {
                Name = "Name Test",
                Description = "Description Test"
            };
            // Act
            await _categoryService.Update(_categoryData.Id, dto);
            var result = await _categoryService.Get(_categoryData.Id);
            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.Id);
            Assert.Equal(dto.Description, result.Description);
            DeleteAll();
        }
        [Fact]
        public async void Delete_Test()
        {
            // Arrange
            //DeleteAll();
            var dto = await GetCategoryData(_categoryService);
            dto = new CategoryDTO
            {
                Name = "Name Test",
                Description = "Description Test"
            };
            // Act
            await _categoryService.Delete(_categoryData.Id);
            var result = _categoryService.Get().Result;
            dto = _categoryService.Create(dto).Result;
            // Assert
            Assert.Empty(result);
            DeleteAll();
        }
        private IHostBuilder GetWorkerService()
        {
            return Host.CreateDefaultBuilder()
                .ConfigureAppConfiguration((hostContext, config) =>
                {
                    config.AddJsonFile("appsettings_test.json", false, false);
                })
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddAutoMapper(typeof(CategoryServiceTest));
                    services.AddScoped<ICategoryService, CategoryService>();
                    services.AddScoped<IProductService, ProductService>();
                    services.Configure<MongoDBSettings>(hostContext
                        .Configuration.GetSection(nameof(MongoDBSettings)));
                    services.AddSingleton<MongoDBContext>(serviceProvider =>
                    {
                        var settings = serviceProvider.GetRequiredService<IOptions<MongoDBSettings>>().Value;
                        return new MongoDBContext(settings.ConnectionString, settings.DatabaseName);
                    });
                });
        }
        private async Task DeleteAll()
        {
            var filter = Builders<Category>.Filter.Empty;//.Where(x => x.Id != _productData.Id);
            await _dbcontext.Categories.DeleteManyAsync(filter);
        }
    }
}
