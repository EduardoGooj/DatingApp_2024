namespace API.UnitTests.Tests;

using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using API.DTOs;
using API.UnitTests.Helpers;
using Xunit;

public class AccountControllerTests
{
    private readonly string apiRoute = "api/account";
    private readonly HttpClient _client;
    private HttpResponseMessage httpResponse;
    private string requestUrl;
    private HttpContent httpContent;

    public AccountControllerTests()
    {
        _client = TestHelper.Instance.Client;
    }

    [Fact]
    public async Task RegisterShouldReturnBadRequestWhenUsernameExists()
    {
        // Arrange
        requestUrl = $"{apiRoute}/register";
        var registerRequest = new RegisterRequest
        {
            Username = "arenita",
            Password = "123456"
        };

        httpContent = GetHttpContent(registerRequest);

        // Act
        httpResponse = await _client.PostAsync(requestUrl, httpContent);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, httpResponse.StatusCode);
        var responseMessage = await httpResponse.Content.ReadAsStringAsync();
        Assert.Contains("Username already in use", responseMessage);
    }

    [Fact]
    public async Task LoginShouldReturnUnauthorizedWhenUserDoesNotExist()
    {
        // Arrange
        requestUrl = $"{apiRoute}/login";
        var loginRequest = new LoginRequest
        {
            Username = "juan",
            Password = "Password123"
        };

        httpContent = GetHttpContent(loginRequest);

        // Act
        httpResponse = await _client.PostAsync(requestUrl, httpContent);

        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, httpResponse.StatusCode);
        var responseMessage = await httpResponse.Content.ReadAsStringAsync();
        Assert.Contains("Invalid username or password", responseMessage);
    }

    [Fact]
    public async Task LoginShouldReturnOkWhenCredentialsAreValid()
    {
        // Arrange
        requestUrl = $"{apiRoute}/login";
        var loginRequest = new LoginRequest
        {
            Username = "arenita",
            Password = "123456"
        };

        httpContent = GetHttpContent(loginRequest);

        // Act
        httpResponse = await _client.PostAsync(requestUrl, httpContent);

        // Assert
        Assert.Equal(HttpStatusCode.OK, httpResponse.StatusCode);
        var responseContent = await httpResponse.Content.ReadAsStringAsync();
        var userResponse = JsonSerializer.Deserialize<UserResponse>(responseContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        Assert.NotNull(userResponse);
        Assert.NotEmpty(userResponse.Token);
    }

    [Fact]
    public async Task RegisterShouldReturnOkWhenUsernameIsAvailable()
    {
        // Arrange
        requestUrl = $"{apiRoute}/register";
        var registerRequest = new RegisterRequest
        {
            Username = "eduardo",
            Password = "123456"
        };

        httpContent = GetHttpContent(registerRequest);

        // Act
        httpResponse = await _client.PostAsync(requestUrl, httpContent);

        // Assert
        Assert.Equal(HttpStatusCode.OK, httpResponse.StatusCode);
    }

    #region Private Helpers

    private static StringContent GetHttpContent(object requestObject) =>
        new(JsonSerializer.Serialize(requestObject), Encoding.UTF8, "application/json");

    #endregion
}
