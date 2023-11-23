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
    public class ProductControllerTest
    {
        private IHost _host;
        private IProductService? _productService;
        private ICategoryService? _categoryService;
        private readonly MongoDBContext? _dbcontext;
        private Product _productData;
        private readonly ProductController controller;
        public ProductControllerTest()
        {
            _host = GetWorkerService().Build();
            _productService = _host.Services.GetService<IProductService>();
            _categoryService = _host.Services.GetService<ICategoryService>();
            _dbcontext = _host.Services.GetService<MongoDBContext>();
            controller = GetControler(_productService, _categoryService);
        }
        [Fact]
        public async void Get_Test()
        {
            // Arrange
            var count = 1;
            //DeleteAll();
            _productData = await GetProductData(_productService);
            // Act
            var actionResult = await controller.Get();
            // Assert
            var okResult = actionResult as OkObjectResult;
            Assert.NotNull(okResult);
            Assert.Equal((int)HttpStatusCode.OK, (int)okResult.StatusCode);
            var products = okResult.Value as List<Product>;
            Assert.NotNull(products);
            DeleteAll();
        }
        [Fact]
        public async void GetbyId_Test()
        {
            // Arrange
            //DeleteAll();
            _productData = await GetProductData(_productService);
            var id = _productData.Id;
            // Act
            var actionResult = await controller.Get(id);
            // Assert
            var okResult = actionResult as OkObjectResult;
            Assert.NotNull(okResult);
            Assert.Equal((int)HttpStatusCode.OK, (int)okResult.StatusCode);
            var product = okResult.Value as Product;
            Assert.Equal(id, product.Id);
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
        public async void GetbyId_Should_Return_InternalErrorTest()
        {
            // Arrange
            //DeleteAll();
            var id = "123456";
            // Act
            var actionResult = await controller.Get(id);
            // Assert
            var objectResult = actionResult as ObjectResult;
            Assert.NotNull(objectResult);
            Assert.Equal((int)HttpStatusCode.InternalServerError, (int)objectResult.StatusCode);
            DeleteAll();
        }
        [Fact]
        public async void Create_Test()
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
            var actionResult = await controller.Create(dto);
            // Assert
            var okResult = actionResult as OkObjectResult;
            Assert.NotNull(okResult);
            Assert.Equal((int)HttpStatusCode.OK, (int)okResult.StatusCode);
            var product = okResult.Value as Product;
            Assert.Equal(dto.Description, product.Description);
            DeleteAll();
        }
        [Fact]
        public async void Create_WithCategoryTest()
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
            var actionResult = await controller.Create(dto);
            // Assert
            var okResult = actionResult as OkObjectResult;
            Assert.NotNull(okResult);
            Assert.Equal((int)HttpStatusCode.OK, (int)okResult.StatusCode);
            var product = okResult.Value as Product;
            Assert.Equal(dto.Description, product.Description);
            DeleteAll();
        }
        [Fact]
        public async void Create_Should_Return_NotFoundTest()
        {
            // Arrange
            //DeleteAll();
            // Act
            var actionResult = await controller.Create(null);
            // Assert
            var notFoundResult = actionResult as NotFoundResult;
            Assert.NotNull(notFoundResult);
            Assert.Equal((int)HttpStatusCode.NotFound, (int)notFoundResult.StatusCode);
            DeleteAll();
        }
        [Fact]
        public async void Update_Test()
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
            var actionResult = await controller.Update(_productData.Id, dto);
            // Assert
            var noResult = actionResult as NoContentResult;
            Assert.NotNull(noResult);
            Assert.Equal((int)HttpStatusCode.NoContent, noResult.StatusCode);
            DeleteAll();
        }
        [Fact]
        public async void Update_WithCategoryTest()
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
            var actionResult = await controller.Update(_productData.Id, dto);
            // Assert
            var noResult = actionResult as NoContentResult;
            Assert.NotNull(noResult);
            Assert.Equal((int)HttpStatusCode.NoContent, noResult.StatusCode);
            DeleteAll();
        }
        [Fact]
        public async void Update_Should_Return_NotFoundTest()
        {
            // Arrange
            //DeleteAll();
            var id = "6553a3bde159e09ee8bc1b47";
            var dto = new ProductDTO
            {
                Name = "Name Test",
                Description = "Description Test"
            };
            // Act
            var actionResult = await controller.Update(id, dto);
            // Assert
            var notFoundResult = actionResult as NotFoundResult;
            Assert.NotNull(notFoundResult);
            Assert.Equal((int)HttpStatusCode.NotFound, (int)notFoundResult.StatusCode);
            DeleteAll();
        }
        [Fact]
        public async void Update_Should_Return_InternalErrorTest()
        {
            // Arrange
            //DeleteAll();
            var id = "123456";
            var dto = new ProductDTO
            {
                Name = "Name Test",
                Description = "Description Test"
            };
            // Act
            var actionResult = await controller.Update(id, dto);
            // Assert
            var objectResult = actionResult as ObjectResult;
            Assert.NotNull(objectResult);
            Assert.Equal((int)HttpStatusCode.InternalServerError, (int)objectResult.StatusCode);
            DeleteAll();
        }
        [Fact]
        public async void Delete_Test()
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
            var actionResult = await controller.Delete(_productData.Id);
            _productData = _productService.Create(dto).Result;
            // Assert
            var noResult = actionResult as NoContentResult;
            Assert.NotNull(noResult);
            Assert.Equal((int)HttpStatusCode.NoContent, noResult.StatusCode);
            DeleteAll();
        }
        [Fact]
        public async void Delete_Should_Return_NotFoundTest()
        {
            // Arrange
            //DeleteAll();
            var id = "6553a3bde159e09ee8bc1b47";
            // Act
            var actionResult = await controller.Delete(id);
            // Assert
            var notFoundResult = actionResult as NotFoundResult;
            Assert.NotNull(notFoundResult);
            Assert.Equal((int)HttpStatusCode.NotFound, (int)notFoundResult.StatusCode);
            DeleteAll();
        }
        [Fact]
        public async void Delete_Should_Return_InternalErrorTest()
        {
            // Arrange
            //DeleteAll();
            var id = "123456";
            // Act
            var actionResult = await controller.Delete(id);
            // Assert
            var objectResult = actionResult as ObjectResult;
            Assert.NotNull(objectResult);
            Assert.Equal((int)HttpStatusCode.InternalServerError, (int)objectResult.StatusCode);
            DeleteAll();
        }
        private async Task<Product> GetProductData(IProductService? _productService)
        {
            var dto = new ProductDTO
            {
                Name = "Name Test",
                Description = "Description Test"
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
        private ProductController? GetControler(IProductService? productService, ICategoryService? categoryService)
        {
            using var loggerFactory = LoggerFactory.Create(loggingBuilder => loggingBuilder
            .SetMinimumLevel(LogLevel.Trace)
            .AddConsole());

            ILogger<ProductController> logger = loggerFactory.CreateLogger<ProductController>();

            return new ProductController(productService, categoryService, logger);
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
                    services.AddAutoMapper(typeof(ProductControllerTest));
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
