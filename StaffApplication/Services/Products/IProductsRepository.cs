using System;
namespace StaffApplication.Services.Products;

public interface IProductsRepository
{
    Task<IEnumerable<ProductDto>> GetProductsAsync(string name);

    Task<ProductDto> GetProductAsync(int id);
}
