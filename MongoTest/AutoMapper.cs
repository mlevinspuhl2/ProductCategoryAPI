using AutoMapper;
using Microsoft.Extensions.Logging;
using MongoTest.DTO;
using MongoTest.models;

namespace MongoTest
{
    public class AutoMapper : Profile
    {
        public AutoMapper()
        {
            CreateMap<Product, ProductDTO>().ReverseMap();
            CreateMap<Category, CategoryDTO>().ReverseMap();
        }
    }
}