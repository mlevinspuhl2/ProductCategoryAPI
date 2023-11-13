using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using MongoDB.Bson;
using MongoDB.Driver;
using ProductCategoryAPI.models;
using ProductCategoryAPI.Services;
using System.Reflection;
using Xunit;

namespace TestProductCategory
{
    public class CategoryServiceTest
    {
        private IHostBuilder GetWorkerService()
        {
            return Host.CreateDefaultBuilder()
                        .ConfigureAppConfiguration((hostContext, config) =>
                        {
                            config.AddJsonFile("appsettings_test.json", false, false);
                        })
                        .ConfigureServices((hostContext, services) =>
                        {
                            services.AddAutoMapper(typeof(Program));
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
        [Fact]
        public async void Get_Test()
        {
            // Arrange
            var service = GetWorkerService().Build().Services.GetService <ICategoryService>();

 
            session.StartTransaction(new TransactionOptions(
                                         readConcern: ReadConcern.Snapshot,
                                         writeConcern: WriteConcern.WMajority));



            // Act
            var categories = await service.Get();

            // Assert
            Assert.NotNull(categories);
        }
    }
}