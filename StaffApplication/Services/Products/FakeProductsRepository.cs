﻿namespace StaffApplication.Services.Products
{
    public class FakeProductsRepository : IProductsRepository
    {
        private readonly ProductDto[] _products =
        {
            new ProductDto {Id = 1, Name = "TestName1", Brand = "TestBrand1", Description = "TestDesc1", Price = 1.99, StockLevel = 1},
            new ProductDto {Id = 2, Name = "TestName2", Brand = "TestBrand2", Description = "TestDesc2", Price = 2.99, StockLevel = 2},
            new ProductDto {Id = 3, Name = "TestName3", Brand = "TestBrand3", Description = "TestDesc3", Price = 3.99, StockLevel = 3},
            new ProductDto {Id = 4, Name = "TestName4", Brand = "TestBrand4", Description = "TestDesc4", Price = 4.99, StockLevel = 4},
            new ProductDto {Id = 5, Name = "TestName5", Brand = "TestBrand5", Description = "TestDesc5", Price = 5.99, StockLevel = 5},
        };
        public Task<ProductDto> GetProductAsync(int id)
        {
            var product = _products.FirstOrDefault(p => p.Id == id);
            return Task.FromResult(product);
        }

        public Task<IEnumerable<ProductDto>> GetProductsAsync(string brand)
        {
            var products = _products.AsEnumerable();
            if (brand != null)
            {
                products = products.Where(p => p.Brand.Equals(brand, StringComparison.OrdinalIgnoreCase));
            }
            return Task.FromResult(products);
        }
    }
}
