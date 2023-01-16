using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NPOI.SS.Formula.Functions;
using StaffApplication.Controllers;
using StaffApplication.Models;
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
            var result = await controller.Index(null, false);

            //Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task GetIndex_WithnullSubject()
        {
            //Arange
            var mockLogger = new Mock<ILogger<ProductsController>>();
            var mockProducts = new Mock<IProductsRepository>();
            var mockReviews = new Mock<IReviewsService>();
            var expected = GetTestProducts();
            mockProducts.Setup(p => p.GetProductsAsync(null, false))
                                .ReturnsAsync(expected)
                                .Verifiable();

            var controller = new ProductsController(mockLogger.Object,
                                                    mockProducts.Object,
                                                    mockReviews.Object);

            //Act
            var result = await controller.Index(null, false);

            //Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<ProductDto>>(
                viewResult.ViewData.Model);
            Assert.Equal(expected.Length, model.Count());

            mockProducts.Verify(p => p.GetProductsAsync(null, false), Times.Once);
                
        }

        [Fact]
        public async Task GetIndex_WithSubject()
        {
            //Arange
            var mockLogger = new Mock<ILogger<ProductsController>>();
            var mockProducts = new Mock<IProductsRepository>();
            var mockReviews = new Mock<IReviewsService>();
            var expected = GetTestProducts();
            mockProducts.Setup(p => p.GetProductsAsync("TestName1", false))
                                .ReturnsAsync(expected)
                                .Verifiable();

            var controller = new ProductsController(mockLogger.Object,
                                                    mockProducts.Object,
                                                    mockReviews.Object);

            //Act
            var result = await controller.Index("TestName1", false);

            //Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<ProductDto>>(
                viewResult.ViewData.Model);
            Assert.Equal(expected.Length, model.Count());

            mockProducts.Verify(p => p.GetProductsAsync("TestName1", false), Times.Once);

        }

        [Fact]
        public async Task GetIndex_BadServiceCall()
        {
            //Arange
            var mockLogger = new Mock<ILogger<ProductsController>>();
            var mockProducts = new Mock<IProductsRepository>();
            var mockReviews = new Mock<IReviewsService>();
            var expected = GetTestProducts();
            mockProducts.Setup(p => p.GetProductsAsync(null, false))
                                .ThrowsAsync(new Exception())
                                .Verifiable();

            var controller = new ProductsController(mockLogger.Object,
                                                    mockProducts.Object,
                                                    mockReviews.Object);

            //Act
            var result = await controller.Index(null, false);

            //Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<ProductDto>>(
                viewResult.ViewData.Model);
            Assert.Empty(model);

            mockProducts.Verify(p => p.GetProductsAsync(null, false), Times.Once);

        }

        [Fact]
        public async Task GetDetails_InvalidModel()
        {
            //Arange
            var mockLogger = new Mock<ILogger<ProductsController>>();
            var mockProducts = new Mock<IProductsRepository>();
            var mockReviews = new Mock<IReviewsService>();
            var controller = new ProductsController(mockLogger.Object,
                                                    mockProducts.Object,
                                                    mockReviews.Object);
            controller.ModelState.AddModelError("test", "test");

            //Act
            var result = await controller.Details(null);

            //Assert
            Assert.IsType<BadRequestResult>(result);

        }

        [Fact]
        public async Task GetDetails_NullId()
        {
            //Arange
            var mockLogger = new Mock<ILogger<ProductsController>>();
            var mockProducts = new Mock<IProductsRepository>();
            var mockReviews = new Mock<IReviewsService>();
            var controller = new ProductsController(mockLogger.Object,
                                                    mockProducts.Object,
                                                    mockReviews.Object);

            //Act
            var result = await controller.Details(null);

            //Assert
            Assert.IsType<BadRequestResult>(result);

        }

        [Fact]
        public async Task GetDetails_WithId()
        {
            //Arange
            var mockLogger = new Mock<ILogger<ProductsController>>();
            var mockProducts = new Mock<IProductsRepository>();
            var mockReviews = new Mock<IReviewsService>();
            var expected = GetTestProducts().First();

            mockProducts.Setup(p => p.GetProductAsync(expected.Id))
                                .ReturnsAsync(expected)
                                .Verifiable();

            var controller = new ProductsController(mockLogger.Object,
                                                    mockProducts.Object,
                                                    mockReviews.Object);

            //Act
            var result = await controller.Details(expected.Id);

            //Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<ProductDetailsViewModel>(viewResult.ViewData.Model);
            Assert.Equal(expected.Id, model.product.Id);

            mockProducts.Verify(p => p.GetProductAsync(expected.Id), Times.Once);

        }
    }
}