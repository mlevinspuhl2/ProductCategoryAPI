using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Core.Configuration;
using ProductCategoryAPI.DTO;
using ProductCategoryAPI.models;
using ProductCategoryAPI.Services;
using System.Reflection;
using System.Runtime.InteropServices.JavaScript;
using System.Threading;
using Xunit;

namespace TestProductCategory
{
    [Collection("Sequential")]
    public class CategoryServiceTest : IClassFixture<TestsFixture>
    {
        TestsFixture _fixture;
        public CategoryServiceTest(TestsFixture fixture)
        {

            _fixture = fixture;

        }
        [Fact]
        public async void Get_Test()
        {
            // Arrange

            // Act
            var result = _fixture._categoryService.Get().Result;

            // Assert
            Assert.NotNull(result);

        }
        [Fact]
        public async void GetbyId_Test()
        {
            // Arrange
            var id = _fixture._categoryData.Id;


            // Act
            var result = _fixture._categoryService.Get(id).Result;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(id, result.Id);

        }
        [Fact]
        public async void Create_Test()
        {
            // Arrange
            var dto = new CategoryDTO
            {
                Name = "Name Test",
                Description = "Description Test"
            };


            // Act
            var result = _fixture._categoryService.Create(dto).Result;

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.Id);
            Assert.Equal(dto.Description, result.Description);

        }
        [Fact]
        public async void Update_Test()
        {
            // Arrange
            var dto = new CategoryDTO
            {
                Name = "Name Test",
                Description = "Description Test"
            };


            // Act
            _fixture._categoryService.Update(_fixture._categoryData.Id, dto);
            var result = _fixture._categoryService.Get(_fixture._categoryData.Id).Result;
            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.Id);
            Assert.Equal(dto.Description, result.Description);

        }
        [Fact]
        public async void Delete_Test()
        {
            // Arrange
            var dto = new CategoryDTO
            {
                Name = "Name Test",
                Description = "Description Test"
            };


            // Act
            _fixture._categoryService.Delete(_fixture._categoryData.Id);
            var result = _fixture._categoryService.Get(_fixture._categoryData.Id).Result;
            _fixture._categoryData = _fixture._categoryService.Create(dto).Result;
            // Assert
            Assert.Null(result);

        }
    }
    [Collection("Sequential")]
    public class TestsFixture : IDisposable
    {
        public IHost _host;
        public ICategoryService? _categoryService;
        public readonly MongoDBContext? _dbcontext;
        public Category _categoryData;
        public TestsFixture()
        {
            _host = GetWorkerService().Build();
            _categoryService = _host.Services.GetService<ICategoryService>();
            _dbcontext = _host.Services.GetService<MongoDBContext>();
            _categoryData = GetCategoryData(_categoryService);
        }
        private Category GetCategoryData(ICategoryService? _categoryService)
        {
            var dto = new CategoryDTO
            {
                Name = "Category Name",
                Description = "Category Description"
            };
            return _categoryService.Create(dto).Result;
        }

        public void Dispose()
        {
            DeleteAll();
        }
        private void DeleteAll()
        {
            var filter = Builders<Category>.Filter.Empty;//.Where(x => x.Id != _categoryData.Id);
            _dbcontext.Categories.DeleteMany(filter);
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

    }
}
