using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using GithubUsersSearcher.Controllers;
using GithubUsersSearcher.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using GithubUsersSearcher.Models;
using System.Net.Http;

namespace GithubUsersSearcher.Tests.Controllers
{
    [TestClass]
    public class GithubControllerTests
    {
        private Mock<GithubService> _mockGithubService;
        private GithubController _controller;

        [TestInitialize]
        public void SetUp()
        {
            var mockHttpClient = new Mock<HttpClient>(); 
            _mockGithubService = new Mock<GithubService>(mockHttpClient.Object);
            _controller = new GithubController(_mockGithubService.Object);
        }


        [TestMethod]
        public async Task GetUser_ShouldReturnModelError_WhenUsernameIsEmpty()
        {
            // Arrange
            var username = string.Empty;

            // Act
            var result = await _controller.GetUser(username) as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Index", result.ViewName);
            Assert.IsFalse(_controller.ModelState.IsValid);
            Assert.AreEqual("Username cannot be empty.", _controller.ModelState["username"].Errors[0].ErrorMessage);
        }

        [TestMethod]
        public async Task GetUser_ShouldReturnModelError_WhenUserNotFound()
        {
            // Arrange
            var username = "nonexistentuser";
            _mockGithubService.Setup(s => s.GetUserAsync(username)).ReturnsAsync((GithubUser)null);

            // Act
            var result = await _controller.GetUser(username) as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Index", result.ViewName);
            Assert.IsFalse(_controller.ModelState.IsValid);
            Assert.AreEqual("User not found.", _controller.ModelState["username"].Errors[0].ErrorMessage);
        }

        [TestMethod]
        public async Task GetUser_ShouldReturnResultsView_WhenUserIsFound()
        {
            // Arrange
            var username = "validuser";
            var user = new GithubUser { Username = username, ReposUrl = "someurl" };
            _mockGithubService.Setup(s => s.GetUserAsync(username)).ReturnsAsync(user);
            _mockGithubService.Setup(s => s.GetUserReposAsync(user.ReposUrl)).ReturnsAsync(new List<GithubRepository>());

            // Act
            var result = await _controller.GetUser(username) as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Results", result.ViewName);
            Assert.AreEqual(user, result.Model);
        }
    }
}
