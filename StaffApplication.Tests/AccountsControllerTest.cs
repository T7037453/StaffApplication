using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Moq;
using StaffApplication.Controllers;
using StaffApplication.Models;
using StaffApplication.Services.Accounts;
using StaffApplication.Services.Products;
using StaffApplication.Services.Reviews;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StaffApplication.Tests
{
    public class AccountsControllerTest
    {
        private AccountDto[] GetTestAccounts() => new AccountDto[]
        {
           new AccountDto {user_id = "test1", name = "testuser1", nickname = "test1", last_login = ""},
           new AccountDto {user_id = "test2", name = "testuser2", nickname = "test2", last_login = ""},
           new AccountDto {user_id = "test3", name = "testuser3", nickname = "test3", last_login = ""},
           new AccountDto {user_id = "test4", name = "testuser4", nickname = "test4", last_login = ""},
           new AccountDto {user_id = "test5", name = "testuser5", nickname = "test5", last_login = ""}
        };

        [Fact]
        public async Task Index_WithInvalidModelState_BadResult()
        {
            //Arrange
            var mockLogger = new Mock<ILogger<AccountsController>>();
            var mockAccounts = new Mock<IAccountsService>();
            var controller = new AccountsController(mockLogger.Object,
                                                    mockAccounts.Object);
            controller.ModelState.AddModelError("Test", "Test2");

            //Act
            var result = await controller.Index();

            //Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task Index_WithNoSubject()
        {
            //Arrange
            var mockLogger = new Mock<ILogger<AccountsController>>();
            var mockAccounts = new Mock<IAccountsService>();
            var expected = GetTestAccounts();
            mockAccounts.Setup(a => a.GetAccountsAsync())
                                 .ReturnsAsync(expected)
                                 .Verifiable();


            var controller = new AccountsController(mockLogger.Object,
                                                    mockAccounts.Object);

            //Act
            var result = await controller.Index();

            //Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<AccountDto>>(
                viewResult.ViewData.Model);
            Assert.Equal(expected.Length, model.Count());

            mockAccounts.Verify(a => a.GetAccountsAsync(), Times.Once);
        }

        [Fact]
        public async Task Index_BadServiceCall()
        {
            //Arrange
            var mockLogger = new Mock<ILogger<AccountsController>>();
            var mockAccounts = new Mock<IAccountsService>();
            var expected = GetTestAccounts();
            mockAccounts.Setup(a => a.GetAccountsAsync())
                                 .ThrowsAsync(new Exception())
                                 .Verifiable();


            var controller = new AccountsController(mockLogger.Object,
                                                    mockAccounts.Object);

            //Act
            var result = await controller.Index();

            //Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<AccountDto>>(
                viewResult.ViewData.Model);
            Assert.Empty(model);

            mockAccounts.Verify(a => a.GetAccountsAsync(), Times.Once);
        }

        [Fact]
        public async Task Details_InvalidModel()
        {
            //Arrange
            var mockLogger = new Mock<ILogger<AccountsController>>();
            var mockAccounts = new Mock<IAccountsService>();
            var controller = new AccountsController(mockLogger.Object,
                                                    mockAccounts.Object);
            controller.ModelState.AddModelError("test", "test");
            //Act
            var result = await controller.Details(null);

            //Assert
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public async Task Details_NullId()
        {
            //Arrange
            var mockLogger = new Mock<ILogger<AccountsController>>();
            var mockAccounts = new Mock<IAccountsService>();
            var controller = new AccountsController(mockLogger.Object,
                                                    mockAccounts.Object);
            //Act
            var result = await controller.Details(null);

            //Assert
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public async Task Details_WithId()
        {
            //Arrange
            var mockLogger = new Mock<ILogger<AccountsController>>();
            var mockAccounts = new Mock<IAccountsService>();
            var expected = GetTestAccounts().First();

            mockAccounts.Setup(a => a.GetAccountAsync(expected.user_id))
                    .ReturnsAsync(expected)
                    .Verifiable();

            var controller = new AccountsController(mockLogger.Object,
                                                    mockAccounts.Object);
            //Act
            var result = await controller.Details(expected.user_id);

            //Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<AccountsDetailsViewModel>(viewResult.ViewData.Model);
            Assert.Equal(expected.user_id, model.accounts.user_id);

            mockAccounts.Verify(a => a.GetAccountAsync(expected.user_id), Times.Once);
        }
    }
}
