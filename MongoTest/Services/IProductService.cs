﻿using MongoTest.DTO;
using MongoTest.models;

namespace MongoTest.Services
{
    public interface IProductService
    {
        Task<Product> Create(ProductDTO productDto, Category category);
        Task Delete(string id);
        Task<IEnumerable<Product>> Get();
        Task<Product> Get(string id);
        Task Update(string id, ProductDTO productDto, Category category);
    }
}