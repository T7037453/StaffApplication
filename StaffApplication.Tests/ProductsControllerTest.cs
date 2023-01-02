using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NPOI.SS.Formula.Functions;
using StaffApplication.Controllers;
using StaffApplication.Services;
using StaffApplication.Services.Products;
using StaffApplication.Services.Reviews;

namespace StaffApplication.Tests
{
    public class ProductsControllerTest
    {

        private ProductDto[] GetTestProducts() => new ProductDto[]
    {
        new ProductDto {Id = 1, Name = "TestName1", Brand = "TestBrand1", Description = "TestDesc", Price = 1.99, StockLevel = 1},
        new ProductDto {Id = 2, Name = "TestName2", Brand = "TestBrand2", Description = "TestDesc", Price = 2.99, StockLevel = 2},
        new ProductDto {Id = 3, Name = "TestName3", Brand = "TestBrand3", Description = "TestDesc", Price = 3.99, StockLevel = 3},
        new ProductDto {Id = 4, Name = "TestName4", Brand = "TestBrand4", Description = "TestDesc", Price = 4.99, StockLevel = 4},
        new ProductDto {Id = 5, Name = "TestName5", Brand = "TestBrand5", Description = "TestDesc", Price = 5.99, StockLevel = 5}
    };
        [Fact]
        public async Task Index_WithInvalidModelState_BadResult()
        {
            //Arrange
            var mockLogger = new Mock<ILogger<ProductsController>>();
            var mockProducts = new Mock<IProductsRepository>();
            var mockReviews = new Mock<IReviewsService>();
            var controller = new ProductsController(mockLogger.Object, 
                                                    mockProducts.Object,
                                                    mockReviews.Object);
            controller.ModelState.AddModelError("Test", "Test2");

            //Act
            var result = await controller.Index(null);

            //Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }
    }
}