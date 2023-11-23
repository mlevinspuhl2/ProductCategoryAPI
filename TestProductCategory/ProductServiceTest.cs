using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using MongoDB.Driver;
using ProductCategoryAPI.DTO;
using ProductCategoryAPI.models;
using ProductCategoryAPI.Services;

namespace TestProductCategory
{
    [Collection("Our Test Collection #1")]
    public class ProductServiceTest
    {
        private IHost _host;
        private IProductService? _productService;
        private ICategoryService? _categoryService;
        private readonly MongoDBContext? _dbcontext;
        private Product _productData;

        public ProductServiceTest()
        {
            _host = GetWorkerService().Build();
            _productService = _host.Services.GetService<IProductService>();
            _categoryService = _host.Services.GetService<ICategoryService>();
            _dbcontext = _host.Services.GetService<MongoDBContext>();
            
        }
        private async Task<Product> GetProductData(IProductService? _productService)
        {
            var dto = new ProductDTO
            {
                Name = "Product Name",
                Description = "Product Description"
            };
            return await _productService.Create(dto);
        }
        private async Task<CategoryDTO> GetCategoryData(ICategoryService? _categoryService)
        {
            var dto = new CategoryDTO
            {
                Name = "Category Name",
                Description = "Category Description"
            };
            return await _categoryService.Create(dto);
        }
        [Fact]
        public async Task Get_Test()
        {
            // Arrange
            var count = 1;
            //DeleteAll();
            _productData = await GetProductData(_productService);
            // Act
            var result = await _productService.Get();
            // Assert
            Assert.NotNull(result);
            //Assert.Equal(count,result.Count());
            DeleteAll();
        }
        [Fact]
        public async Task GetbyId_Test()
        {
            // Arrange
            //DeleteAll();
            _productData = await GetProductData(_productService);
            var id = _productData.Id;
            // Act
            var result = await _productService.Get(id);
            // Assert
            Assert.NotNull(result);
            Assert.Equal(id, result.Id);
            DeleteAll();
        }
        [Fact]
        public async Task Create_Test()
        {
            // Arrange
            //DeleteAll();
            _productData = await GetProductData(_productService);
            var dto = new ProductDTO
            {
                Name = "Name Test",
                Description = "Description Test"
            };
            // Act
            var result = await _productService.Create(dto);
            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.Id);
            Assert.Equal(dto.Description, result.Description);
            DeleteAll();
        }
        [Fact]
        public async Task Create_WithCategoryTest()
        {
            // Arrange
            //DeleteAll();
            _productData = await GetProductData(_productService);
            var _categoryData = await GetCategoryData(_categoryService);
            var dto = new ProductDTO
            {
                Name = "Name Test",
                Description = "Description Test",
            };
            // Act
            var result = await _productService.Create(dto, null);
            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.Id);
            Assert.Equal(dto.Description, result.Description);
            DeleteAll();
        }
        [Fact]
        public async Task Update_Test()
        {
            // Arrange
            //DeleteAll();
            _productData = await GetProductData(_productService);
            var _categoryData = await GetCategoryData(_categoryService);
            var dto = new ProductDTO
            {
                Name = "Name Test",
                Description = "Description Test",
            };
            // Act
            await _productService.Update(_productData.Id, dto, null);
            var result = await _productService.Get(_productData.Id);
            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.Id);
            Assert.Equal(dto.Description, result.Description);
            DeleteAll();
        }
        [Fact]
        public async Task Update_WithCategoryTest()
        {
            // Arrange
            //DeleteAll();
            _productData = await GetProductData(_productService);
            var _categoryData = await GetCategoryData(_categoryService);
            var dto = new ProductDTO
            {
                Name = "Name Test",
                Description = "Description Test",
            };
            // Act
            await _productService.Update(_productData.Id, dto, null);
            var result = await _productService.Get(_productData.Id);
            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.Id);
            Assert.Equal(dto.Description, result.Description);
            DeleteAll();
        }
        [Fact]
        public async Task Delete_Test()
        {
            // Arrange
            //DeleteAll();
            _productData = await GetProductData(_productService);
            var dto = new ProductDTO
            {
                Name = "Name Test",
                Description = "Description Test"
            };
            // Act
            await _productService.Delete(_productData.Id);
            var result = await _productService.Get();
            _productData = await _productService.Create(dto);
            // Assert
            Assert.Empty(result);
            DeleteAll();
        }
        private IHostBuilder GetWorkerService()
        {
            var program = new Program();

            return Host.CreateDefaultBuilder()
                .ConfigureAppConfiguration((hostContext, config) =>
                {
                    config.AddJsonFile("appsettings_test.json", false, false);
                })
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddAutoMapper(typeof(ProductServiceTest));
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
            var filter = Builders<Product>.Filter.Empty;//.Where(x => x.Id != _productData.Id);
            
            await _dbcontext.Products.DeleteManyAsync(filter);
        }
    }
}
