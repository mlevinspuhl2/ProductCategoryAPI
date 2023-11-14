using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using ProductCategoryAPI.Controllers;
using ProductCategoryAPI.DTO;
using ProductCategoryAPI.models;
using ProductCategoryAPI.Services;
using System.Net;

namespace TestProductCategory
{
    [Collection("Our Test Collection #1")]
    public class CategoryControllerTest
    {
        private IHost _host;
        private ICategoryService? _categoryService;
        private readonly MongoDBContext? _dbcontext;
        private Category _categoryData;
        private readonly CategoryController controller;
        public CategoryControllerTest()
        {
            _host = GetWorkerService().Build();
            _categoryService = _host.Services.GetService<ICategoryService>();
            _dbcontext = _host.Services.GetService<MongoDBContext>();
            controller = GetControler(_categoryService);
        }
        [Fact]
        public async void Get_Test()
        {
            // Arrange
            var count = 1;
            //DeleteAll();
            _categoryData = await GetCategoryData(_categoryService);
            // Act
            var actionResult = await controller.Get();
            // Assert
            var okResult = actionResult as OkObjectResult;
            Assert.NotNull(okResult);
            Assert.Equal((int)HttpStatusCode.OK, (int)okResult.StatusCode);
            var categories = okResult.Value as List<Category>;
            Assert.NotNull(categories);
            //Assert.Equal(count, categories.Count);
            DeleteAll();
        }
        [Fact]
        public async void GetbyId_Test()
        {
            // Arrange
            //DeleteAll();
            _categoryData = await GetCategoryData(_categoryService);
            var id = _categoryData.Id;
            // Act
            var actionResult = await controller.Get(id);
            // Assert
            var okResult = actionResult as OkObjectResult;
            if(okResult == null)
            {
                var act = actionResult;
            }
            Assert.NotNull(okResult);
            Assert.Equal((int)HttpStatusCode.OK, (int)okResult.StatusCode);
            var category = okResult.Value as Category;
            Assert.Equal(id, category.Id);
            DeleteAll();
        }
        [Fact]
        public async void GetbyId_Should_Return_NotFoundTest()
        {
            // Arrange
            //DeleteAll();
            var id = "6553a3bde159e09ee8bc1b47";
            // Act
            var actionResult = await controller.Get(id);
            // Assert
            var notFoundResult = actionResult as NotFoundResult;
            Assert.NotNull(notFoundResult);
            Assert.Equal((int)HttpStatusCode.NotFound, (int)notFoundResult.StatusCode);
            DeleteAll();
        }
        [Fact]
        public async void Create_Test()
        {
            // Arrange
            //DeleteAll();
            _categoryData = await GetCategoryData(_categoryService);
            var dto = new CategoryDTO
            {
                Name = "Name Test",
                Description = "Description Test"
            };
            // Act
            var actionResult = await controller.Create(dto);
            // Assert
            var okResult = actionResult as OkObjectResult;
            Assert.NotNull(okResult);
            Assert.Equal((int)HttpStatusCode.OK, (int)okResult.StatusCode);
            var category = okResult.Value as Category;
            Assert.Equal(dto.Description, category.Description);
            DeleteAll();
        }
        [Fact]
        public async void Update_Test()
        {
            // Arrange
            //DeleteAll();
            _categoryData = await GetCategoryData(_categoryService);
            var dto = new CategoryDTO
            {
                Name = "Name Test",
                Description = "Description Test"
            };
            // Act
            var actionResult = await controller.Update(_categoryData.Id, dto);
            // Assert
            var noResult = actionResult as NoContentResult;
            Assert.NotNull(noResult);
            Assert.Equal((int)HttpStatusCode.NoContent, noResult.StatusCode);
            DeleteAll();
        }
        [Fact]
        public async void Delete_Test()
        {
            // Arrange
            //DeleteAll();
            _categoryData = await GetCategoryData(_categoryService);
            var dto = new CategoryDTO
            {
                Name = "Name Test",
                Description = "Description Test"
            };
            // Act
            var actionResult = await controller.Delete(_categoryData.Id);
            _categoryData = _categoryService.Create(dto).Result;
            // Assert
            var noResult = actionResult as NoContentResult;
            Assert.NotNull(noResult);
            Assert.Equal((int)HttpStatusCode.NoContent, noResult.StatusCode);
            DeleteAll();
        }
        private async Task<Category> GetCategoryData(ICategoryService? _categoryService)
        {
            var dto = new CategoryDTO
            {
                Name = "Category Name",
                Description = "Category Description"
            };
            return await _categoryService.Create(dto);

        }
        private CategoryController? GetControler(ICategoryService? categoryService)
        {
            using var loggerFactory = LoggerFactory.Create(loggingBuilder => loggingBuilder
            .SetMinimumLevel(LogLevel.Trace)
            .AddConsole());

            ILogger<CategoryController> logger = loggerFactory.CreateLogger<CategoryController>();

            return new CategoryController(categoryService, logger);
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
