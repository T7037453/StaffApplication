using System;
using Microsoft.AspNetCore.Mvc;
using StaffApplication.Models;
namespace StaffApplication.Services.Products;

public interface IProductsRepository
{
    Task<IEnumerable<ProductDto>> GetProductsAsync(string? name, bool update);

    Task<ProductDto> GetProductAsync(int id);

    Task<ProductDto> CreateProductAsync(ProductDto product);

    Task<ProductDto> DeleteProductAsync(int id);

    Task<Product> EditProductAsync(Product product, int id);

}
