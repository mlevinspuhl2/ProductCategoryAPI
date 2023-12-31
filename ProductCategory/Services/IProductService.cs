﻿using ProductCategoryAPI.DTO;
using ProductCategoryAPI.models;

namespace ProductCategoryAPI.Services
{
    public interface IProductService
    {
        Task<Product> Create(ProductDTO productDto, Category category = null);
        Task Delete(string id);
        Task<IEnumerable<Product>> Get();
        Task<Product> Get(string id);
        Task<ProductDTO> Update(string id, ProductDTO productDto, Category category = null);
    }
}