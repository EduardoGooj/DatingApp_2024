namespace API.UnitTests.Tests;

using System.Security.Claims;
using API.Controllers;
using API.Data;
using API.DataEntities;
using API.DTOs;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

public class UsersControllerTests
{
    private readonly Mock<IUserRepository> _repositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly UsersController _controller;

    public UsersControllerTests()
    {
        _repositoryMock = new Mock<IUserRepository>();
        _mapperMock = new Mock<IMapper>();
        _controller = new UsersController(_repositoryMock.Object, _mapperMock.Object);

        // Mock user context
        var user = new ClaimsPrincipal(new ClaimsIdentity(new[]
        {
            new Claim(ClaimTypes.NameIdentifier, "arenita")
        }, "TestAuth"));

        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = user }
        };
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnOkWithMembers()
    {
        // Arrange
        var members = new List<MemberResponse>
        {
            new MemberResponse { UserName = "arenita" },
            new MemberResponse { UserName = "calamardo" }
        };

        _repositoryMock.Setup(r => r.GetMembersAsync()).ReturnsAsync(members);

        // Act
        var result = await _controller.GetAllAsync();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnedMembers = Assert.IsType<List<MemberResponse>>(okResult.Value);
        Assert.Equal(2, returnedMembers.Count);
    }

    [Fact]
    public async Task GetByUsernameAsync_ValidUsername_ShouldReturnMember()
    {
        // Arrange
        var username = "arenita";
        var member = new MemberResponse { UserName = username };

        _repositoryMock.Setup(r => r.GetMemberAsync(username)).ReturnsAsync(member);

        // Act
        var result = await _controller.GetByUsernameAsync(username);

        // Assert
        var returnedMember = Assert.IsType<MemberResponse>(result.Value);
        Assert.Equal(username, returnedMember.UserName);
    }

    [Fact]
    public async Task GetByUsernameAsync_InvalidUsername_ShouldReturnNotFound()
    {
        // Arrange
        var username = "unknown";
        _repositoryMock.Setup(r => r.GetMemberAsync(username)).ReturnsAsync((MemberResponse)null);

        // Act
        var result = await _controller.GetByUsernameAsync(username);

        // Assert
        Assert.IsType<NotFoundResult>(result.Result);
    }

    [Fact]
    public async Task UpdateUser_ValidRequest_ShouldReturnNoContent()
    {
        // Arrange
        var updateRequest = new MemberUpdateRequest { Introduction = "Updated Bio" };
        var user = new AppUser
        {
            UserName = "arenita",
            KnownAs = "Arenita",
            Gender = "Female",
            City = "Bikini Bottom",
            Country = "Pacific Ocean"
        };

        _repositoryMock.Setup(r => r.GetByUsernameAsync("arenita")).ReturnsAsync(user);
        _repositoryMock.Setup(r => r.SaveAllAsync()).ReturnsAsync(true);

        // Act
        var result = await _controller.UpdateUser(updateRequest);

        // Assert
        Assert.IsType<NoContentResult>(result);
        _mapperMock.Verify(m => m.Map(updateRequest, user), Times.Once);
        _repositoryMock.Verify(r => r.Update(user), Times.Once);
    }

    [Fact]
    public async Task UpdateUser_UserNotFound_ShouldReturnBadRequest()
    {
        // Arrange
        var updateRequest = new MemberUpdateRequest { Introduction = "Updated Bio" };

        _repositoryMock.Setup(r => r.GetByUsernameAsync("arenita")).ReturnsAsync((AppUser)null);

        // Act
        var result = await _controller.UpdateUser(updateRequest);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("Could not find user", badRequestResult.Value);
    }

    [Fact]
    public async Task UpdateUser_SaveFails_ShouldReturnBadRequest()
    {
        // Arrange
        var updateRequest = new MemberUpdateRequest { Introduction = "Updated Bio" };
        var user = new AppUser
        {
            UserName = "arenita",
            KnownAs = "Arenita",
            Gender = "Female",
            City = "Bikini Bottom",
            Country = "Pacific Ocean"
        };

        _repositoryMock.Setup(r => r.GetByUsernameAsync("arenita")).ReturnsAsync(user);
        _repositoryMock.Setup(r => r.SaveAllAsync()).ReturnsAsync(false);

        // Act
        var result = await _controller.UpdateUser(updateRequest);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("Update user failed!", badRequestResult.Value);
    }
}
