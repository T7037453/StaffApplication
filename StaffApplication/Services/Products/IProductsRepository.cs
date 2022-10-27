using System;
namespace StaffApplication.Services.Products
{
    public interface IProductsRepository
    {
        Task<IEnumerable<ProductDto>> GetProductsAsync(string brand);

        Task<ProductDto> GetProductAsync(int id);
    }
}
