using Application.Users.Create;

namespace Application.UnitTests.Users.Create;

public class CreateUserCommandValidatorTests
{
    [Fact]
    public async Task NewCreateUserCommand_Should_NotBeValid_WhenNameIsEmptyAsync()
    {
        // Arrange
        string? name = string.Empty;
        var email = "test@test.com";
        var password = "testpassword";
        var createUserCommandValidator = new CreateUserCommandValidator();

        // Act
        var createUserCommand = new CreateUserCommand(name!, email, password);
        var results = await createUserCommandValidator.ValidateAsync(createUserCommand);

        // Assert
        Assert.False(results.IsValid);
    }
}
