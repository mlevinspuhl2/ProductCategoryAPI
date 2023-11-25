using Amazon.Auth.AccessControlPolicy;
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
            builder.Services.AddCors(options =>
            {
                //options.AddPolicy(name: "MyPolicy",
                //    policy =>
                //{
                //    policy.WithOrigins("http://localhost:83",
                //        "http://52.18.129.98:83",
                //        "http://localhost:4200",
                //        "http://52.18.129.98:4200",
                //        "http://172.31.22.34:83",
                //        "http://172.31.22.34:4200")
                //    .AllowAnyMethod()
                //    .AllowAnyHeader();
                //});
                options.AddDefaultPolicy(builder =>
                {
                    builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
                });
            });
            var app = builder.Build();
            app.UseCors();
            // Configure the HTTP request pipeline.
            //if (app.Environment.IsDevelopment())
            //{
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Product Category API v1"));
            //}
            if (!app.Environment.IsDevelopment())
            {
                app.UseHttpsRedirection();
            }
            app.UseRouting();
            app.UseCors();
            app.UseAuthorization();
            app.MapControllers();
            //app.UseEndpoints(endpoints =>
            //{
            //    endpoints.MapControllers();
            //});
            app.Run();

        }
    }
}
