using System;
namespace StaffApplication.Services.Products;

public class ProductDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Brand { get; set; }
    public string Description { get; set; }
    public double Price { get; set; }
    public int StockLevel { get; set; }   
}
