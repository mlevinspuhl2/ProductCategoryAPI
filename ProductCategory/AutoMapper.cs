using AutoMapper;
using Microsoft.Extensions.Logging;
using ProductCategoryAPI.DTO;
using ProductCategoryAPI.models;
using System.Diagnostics.CodeAnalysis;

namespace ProductCategoryAPI
{
    [ExcludeFromCodeCoverage]
    public class AutoMapper : Profile
    {
        public AutoMapper()
        {
            CreateMap<Product, ProductDTO>().ReverseMap();
            CreateMap<Category, CategoryDTO>().ReverseMap();
        }
    }
}