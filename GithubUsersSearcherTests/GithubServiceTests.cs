using GithubUsersSearcher.Models;
using GithubUsersSearcher.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Moq.Protected;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

[TestClass]
public class GithubServiceTests
{
    private Mock<HttpMessageHandler> _mockHttpMessageHandler;
    private HttpClient _httpClient;
    private GithubService _githubService;

    [TestInitialize]
    public void Setup()
    {
        _mockHttpMessageHandler = new Mock<HttpMessageHandler>();

        // Create HttpClient using the mocked handler
        _httpClient = new HttpClient(_mockHttpMessageHandler.Object);
        _httpClient.DefaultRequestHeaders.Add("User-Agent", "GithubUsersSearcher");

        // Instantiate service with mocked HttpClient
        _githubService = new GithubService(_httpClient);
    }

    [TestMethod]
    public async Task GetUserAsync_ValidUsername_ReturnsGithubUser()
    {
        // Arrange
        var expectedUser = new GithubUser
        {
            Id = 12345,
            Username = "testuser",
            HtmlUrl = "https://test.user",
            AvatarUrl = "https://test.user/avatar.png",
            Location = "testville",
            ReposUrl = "https://test.user/repos",
            GithubRepositories = new List<GithubRepository>() { new GithubRepository() }
        };

        _mockHttpMessageHandler
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(JsonConvert.SerializeObject(expectedUser))
            });

        // Act
        var result = await _githubService.GetUserAsync("testuser");

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(expectedUser.Id, result.Id);
        Assert.AreEqual(expectedUser.Username, result.Username);
        Assert.AreEqual(expectedUser.HtmlUrl, result.HtmlUrl);
        Assert.AreEqual(expectedUser.AvatarUrl, result.AvatarUrl);
        Assert.AreEqual(expectedUser.Location, result.Location);
        Assert.AreEqual(expectedUser.ReposUrl, result.ReposUrl);
        Assert.AreEqual(expectedUser.GithubRepositories.Count, result.GithubRepositories.Count);

    }


    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public async Task GetUserAsync_ShouldThrowArgumentException_WhenUsernameIsNull()
    {
        await _githubService.GetUserAsync(null);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public async Task GetUserAsync_ShouldThrowArgumentException_WhenUsernameIsEmpty()
    {
        await _githubService.GetUserAsync("");
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public async Task GetUserAsync_ShouldThrowArgumentException_WhenUsernameIsWhitespace()
    {
        await _githubService.GetUserAsync("   ");
    }

    [TestMethod]
    public async Task GetUserAsync_ShouldReturnNull_WhenUserDoesNotExist()
    {
        // Arrange: Simulate 404 Not Found response
        _mockHttpMessageHandler
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.NotFound
            });

        // Act
        GithubUser result = await _githubService.GetUserAsync("nonexistentuser");

        // Assert
        Assert.IsNull(result);
    }
    
    [TestMethod]
    public async Task GetUserReposAsync_ValidResponse_ReturnsTop5Repos()
    {
        // Arrange
        var mockRepos = new List<GithubRepository>
        {
            new GithubRepository { Name = "Repo1", StargazersCount = 10, HtmlUrl = "url1" },
            new GithubRepository { Name = "Repo2", StargazersCount = 50, HtmlUrl = "url2" },
            new GithubRepository { Name = "Repo3", StargazersCount = 20, HtmlUrl = "url3" },
            new GithubRepository { Name = "Repo4", StargazersCount = 5, HtmlUrl = "url4" },
            new GithubRepository { Name = "Repo5", StargazersCount = 30, HtmlUrl = "url5" },
            new GithubRepository { Name = "Repo6", StargazersCount = 15, HtmlUrl = "url6" }
        };

        _mockHttpMessageHandler
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(JsonConvert.SerializeObject(mockRepos))
            });

        // Act
        var result = await _githubService.GetUserReposAsync("https://api.github.com/users/testuser/repos");

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(5, result.Count);
        Assert.AreEqual("Repo2", result[0].Name); // Repo with highest stars should be first
        Assert.AreEqual("Repo5", result[1].Name);
    }

    [TestMethod]
    public async Task GetUserReposAsync_ValidResponse_ReturnsTopXRepos_WhenLessThan5Repos()
    {
        // Arrange
        var mockRepos = new List<GithubRepository>
        {
            new GithubRepository { Name = "Repo1", StargazersCount = 10, HtmlUrl = "url1" },
            new GithubRepository { Name = "Repo2", StargazersCount = 50, HtmlUrl = "url2" },
            new GithubRepository { Name = "Repo3", StargazersCount = 20, HtmlUrl = "url3" }
        };

        _mockHttpMessageHandler
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(JsonConvert.SerializeObject(mockRepos))
            });

        // Act
        var result = await _githubService.GetUserReposAsync("https://api.github.com/users/testuser/repos");

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(3, result.Count);
        Assert.AreEqual("Repo2", result[0].Name);
        Assert.AreEqual("Repo3", result[1].Name);
        Assert.AreEqual("Repo1", result[2].Name);
    }

    [TestMethod]
    public async Task GetUserReposAsync_ValidResponse_ReturnsNoRepos_WhenThereAreNone()
    {
        // Arrange
        var mockRepos = new List<GithubRepository>{};

        _mockHttpMessageHandler
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(JsonConvert.SerializeObject(mockRepos))
            });

        // Act
        var result = await _githubService.GetUserReposAsync("https://api.github.com/users/testuser/repos");

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(0, result.Count);
    }
}
