using Application.Common;
using Application.DTOs;
using Application.Interfaces;
using Application.Services;
using Domain.Entities;
using FluentAssertions;
using Moq;

namespace Test;

public class AuthServiceTests
{
    private readonly Mock<IUserRepository> _userRepoMock;
    private readonly Mock<IJwtTokenService> _jwtMock;
    private readonly AuthService _service;

    public AuthServiceTests()
    {
        _userRepoMock = new Mock<IUserRepository>();
        _jwtMock = new Mock<IJwtTokenService>();
        var validator = new AuthValidator();

        _service = new AuthService(_userRepoMock.Object, _jwtMock.Object, validator);
    }

    [Fact]
    public async Task Register_ShouldReturnSuccess_WhenDataIsValid()
    {
        var dto = CreateValidRegisterDto();

        _userRepoMock
            .Setup(r => r.GetByEmailAsync(dto.Email))
            .ReturnsAsync((User?)null);

        _jwtMock
            .Setup(j => j.CreateToken(It.IsAny<User>()))
            .Returns("test-token");

        var result = await _service.Register(dto);

        result.Success.Should().BeTrue();
        result.Data.Should().Be("test-token");
        _userRepoMock.Verify(r => r.AddAsync(It.Is<User>(u => u.Email == dto.Email)), Times.Once);
        _userRepoMock.Verify(r => r.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task Register_ShouldReturnFail_WhenEmailAlreadyExists()
    {
        var dto = CreateValidRegisterDto();

        _userRepoMock
            .Setup(r => r.GetByEmailAsync(dto.Email))
            .ReturnsAsync(new User
            {
                Id = Guid.NewGuid(),
                Name = "Existing",
                Surename = "User",
                Email = dto.Email,
                Password = BCrypt.Net.BCrypt.HashPassword("password123")
            });

        var result = await _service.Register(dto);

        result.Success.Should().BeFalse();
        result.Error.Should().Be("User with same e-mail already exists");
        _userRepoMock.Verify(r => r.AddAsync(It.IsAny<User>()), Times.Never);
    }

    [Fact]
    public async Task Register_ShouldReturnFail_WhenPasswordsDoNotMatch()
    {
        var dto = CreateValidRegisterDto();
        dto.ConfirmPassword = "different-password";

        var result = await _service.Register(dto);

        result.Success.Should().BeFalse();
        result.Error.Should().Be("Passwords do not match");
        _userRepoMock.Verify(r => r.GetByEmailAsync(It.IsAny<string>()), Times.Never);
    }

    [Fact]
    public async Task Login_ShouldReturnSuccess_WhenCredentialsAreValid()
    {
        const string password = "password123";
        var dto = new LoginDTO
        {
            Email = "test@example.com",
            Password = password
        };

        var user = new User
        {
            Id = Guid.NewGuid(),
            Name = "Test",
            Surename = "User",
            Email = dto.Email,
            Password = BCrypt.Net.BCrypt.HashPassword(password)
        };

        _userRepoMock
            .Setup(r => r.GetByEmailAsync(dto.Email))
            .ReturnsAsync(user);

        _jwtMock
            .Setup(j => j.CreateToken(user))
            .Returns("login-token");

        var result = await _service.Login(dto);

        result.Success.Should().BeTrue();
        result.Data.Should().Be("login-token");
    }

    [Fact]
    public async Task Login_ShouldReturnFail_WhenUserDoesNotExist()
    {
        var dto = new LoginDTO
        {
            Email = "missing@example.com",
            Password = "password123"
        };

        _userRepoMock
            .Setup(r => r.GetByEmailAsync(dto.Email))
            .ReturnsAsync((User?)null);

        var result = await _service.Login(dto);

        result.Success.Should().BeFalse();
        result.Error.Should().Be("User doesn't exist");
    }

    [Fact]
    public async Task Login_ShouldReturnFail_WhenPasswordIsInvalid()
    {
        const string password = "password123";
        var dto = new LoginDTO
        {
            Email = "test@example.com",
            Password = "wrong-password"
        };

        var user = new User
        {
            Id = Guid.NewGuid(),
            Name = "Test",
            Surename = "User",
            Email = dto.Email,
            Password = BCrypt.Net.BCrypt.HashPassword(password)
        };

        _userRepoMock
            .Setup(r => r.GetByEmailAsync(dto.Email))
            .ReturnsAsync(user);

        var result = await _service.Login(dto);

        result.Success.Should().BeFalse();
        result.Error.Should().Be("Invalid credentials");
        _jwtMock.Verify(j => j.CreateToken(It.IsAny<User>()), Times.Never);
    }

    private static RegisterDTO CreateValidRegisterDto() =>
        new()
        {
            Name = "Test",
            Surename = "User",
            Email = "test@example.com",
            Password = "password123",
            ConfirmPassword = "password123"
        };
}
