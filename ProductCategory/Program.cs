using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using NLog.Extensions.Logging;
using ProductCategoryAPI.models;
using ProductCategoryAPI.Services;
using System.Diagnostics.CodeAnalysis;

namespace ProductCategoryAPI
{
    [ExcludeFromCodeCoverage]
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args);
        }
        public static void CreateHostBuilder(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddAutoMapper(typeof(Program));
            builder.Services.AddScoped<ICategoryService, CategoryService>();
            builder.Services.AddScoped<IProductService, ProductService>();
            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Product Category API", Version = "v1" });
            });
            builder.Services.Configure<MongoDBSettings>(builder.Configuration.GetSection(nameof(MongoDBSettings)));

            builder.Services.AddSingleton<MongoDBContext>(serviceProvider =>
            {
                var settings = serviceProvider.GetRequiredService<IOptions<MongoDBSettings>>().Value;
                return new MongoDBContext(settings.ConnectionString, settings.DatabaseName);
            });

            builder.Services.AddLogging(loggingBuilder =>
            {
                loggingBuilder.AddNLog("nlog.config");
            });

            // Add controllers
            builder.Services.AddControllers();
            builder.Services.AddCors(c =>
            {
                c.AddPolicy("CorsPolicy", options =>
                {
                    options.AllowAnyOrigin()
                    .AllowAnyHeader()
                    .AllowAnyMethod();
                });
            });
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            //if (app.Environment.IsDevelopment())
            //{
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Product Category API v1"));
            //}
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseCors("CorsPolicy");
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            app.Run();

        }
    }
}
