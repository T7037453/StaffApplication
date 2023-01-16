using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using StaffApplication.Controllers;
using StaffApplication.Services.Products;
using StaffApplication.Services.Reviews;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StaffApplication.Tests
{
    public class ReviewsControllerTest
    {
        private ReviewDto[] GetTestReviews() => new ReviewDto[]
    {
        new ReviewDto {Id = 1, Title = "TestReview1", productId = 1, productReviewContent = "TestReview1", productReviewRating = 1, displayReview = true, anonymized = false, firstName = "TestReviewUser1"},
        new ReviewDto {Id = 2, Title = "TestReview2", productId = 1, productReviewContent = "TestReview2", productReviewRating = 1, displayReview = true, anonymized = false, firstName = "TestReviewUser2"},
        new ReviewDto {Id = 3, Title = "TestReview3", productId = 2, productReviewContent = "TestReview3", productReviewRating = 3, displayReview = true, anonymized = false, firstName = "TestReviewUser3"},
        new ReviewDto {Id = 4, Title = "TestReview4", productId = 2, productReviewContent = "TestReview4", productReviewRating = 3, displayReview = true, anonymized = false, firstName = "TestReviewUser4"},
        new ReviewDto {Id = 5, Title = "TestReview5", productId = 3, productReviewContent = "TestReview5", productReviewRating = 5, displayReview = true, anonymized = false, firstName = "TestReviewUser5"},
        new ReviewDto {Id = 6, Title = "TestReview6", productId = 3, productReviewContent = "TestReview6", productReviewRating = 5, displayReview = true, anonymized = false, firstName = "TestReviewUser6"},
    };

        [Fact]
        public async Task Index_WithInvalidModelState_BadResult()
        {
            //Arrange
            var mockLogger = new Mock<ILogger<ReviewsController>>();
            var mockReviews = new Mock<IReviewsService>();
            var controller = new ReviewsController(mockLogger.Object,
                                                    mockReviews.Object);
            controller.ModelState.AddModelError("Test", "Test2");

            //Act
            var result = await controller.Index(0);

            //Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task GetIndex_WithnullSubject()
        {
            //Arange
            var mockLogger = new Mock<ILogger<ReviewsController>>();
            var mockReviews = new Mock<IReviewsService>();
            var expected = GetTestReviews();
            mockReviews.Setup(r => r.GetReviewsAsync(0))
                                .ReturnsAsync(expected)
                                .Verifiable();

            var controller = new ReviewsController(mockLogger.Object,
                                                    mockReviews.Object);

            //Act
            var result = await controller.Index(0);

            //Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<List<ReviewDto>>(
                viewResult.ViewData.Model);
            Assert.Equal(expected.Length, model.Count());

            mockReviews.Verify(p => p.GetReviewsAsync(0), Times.Once);

        }

        [Fact]
        public async Task GetIndex_WithSubject()
        {
            //Arange
            var mockLogger = new Mock<ILogger<ReviewsController>>();
            var mockReviews = new Mock<IReviewsService>();
            var expected = GetTestReviews();
            mockReviews.Setup(r => r.GetReviewsAsync(1))
                                .ReturnsAsync(expected)
                                .Verifiable();

            var controller = new ReviewsController(mockLogger.Object,
                                                    mockReviews.Object);

            //Act
            var result = await controller.Index(1);

            //Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<List<ReviewDto>>(
                viewResult.ViewData.Model);
            Assert.Equal(expected.Length, model.Count());

            mockReviews.Verify(p => p.GetReviewsAsync(1), Times.Once);

        }

        [Fact]
        public async Task GetIndex_BadServiceCall()
        {
            //Arange
            var mockLogger = new Mock<ILogger<ReviewsController>>();
            var mockReviews = new Mock<IReviewsService>();
            var expected = GetTestReviews();
            mockReviews.Setup(r => r.GetReviewsAsync(0))
                                .ThrowsAsync(new Exception())
                                .Verifiable();

            var controller = new ReviewsController(mockLogger.Object,
                                                    mockReviews.Object);

            //Act
            var result = await controller.Index(0);

            //Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<List<ReviewDto>>(
                viewResult.ViewData.Model);
            Assert.Empty(model);

            mockReviews.Verify(p => p.GetReviewsAsync(0), Times.Once);

        }

        [Fact]
        public async Task GetDetails_ValidViewResult()
        {
            //Arange
            var mockLogger = new Mock<ILogger<ReviewsController>>();
            var mockReviews = new Mock<IReviewsService>();
            var controller = new ReviewsController(mockLogger.Object,
                                                    mockReviews.Object);
            controller.ModelState.AddModelError("test", "test");

            //Act
            var result = await controller.Details(0);

            //Assert
            Assert.IsType<ViewResult>(result);

        }

        [Fact]
        public async Task GetDetails_WithId()
        {
            //Arange
            var mockLogger = new Mock<ILogger<ReviewsController>>();
            var mockReviews = new Mock<IReviewsService>();
            var expected = GetTestReviews().First();
            mockReviews.Setup(r => r.GetReviewAsync(expected.Id))
                                .ReturnsAsync(expected)
                                .Verifiable();

            var controller = new ReviewsController(mockLogger.Object,
                                                    mockReviews.Object);

            //Act
            var result = await controller.Details(expected.Id);

            //Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<ReviewDto>(viewResult.ViewData.Model);
            Assert.Equal(expected.Id, model.Id);

            mockReviews.Verify(r => r.GetReviewAsync(expected.Id), Times.Once);

        }

    }
}
