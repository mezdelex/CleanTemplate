using Application.Messages;
using Application.Users.Create;
using FluentAssertions;

namespace Application.UnitTests.Users.Create;

public class CreateUserCommandValidatorTests
{
    [Fact]
    public async Task NewCreateUserCommand_Should_ContainGivenValidationMessage_WhenNameIsEmptyAsync()
    {
        // Arrange
        var name = string.Empty;
        var email = "test@test.com";
        var password = "testpassword";
        var createUserCommandValidator = new CreateUserCommandValidator();

        // Act
        var createUserCommand = new CreateUserCommand(name!, email, password);
        var results = await createUserCommandValidator.ValidateAsync(createUserCommand);

        // Assert
        results.ToString().Should().Contain(GenericValidatorMessages.ShouldNotBeEmptyMessage());
    }

    [Fact]
    public async Task NewCreateUserCommand_Should_ContainGivenValidationMessage_WhenNameIsTooLongAsync()
    {
        // Arrange
        var createUserCommandValidator = new CreateUserCommandValidator();
        var name = new string('a', createUserCommandValidator.NameMaxLength + 1);
        var email = "test@test.com";
        var password = "testpassword";

        // Act
        var createUserCommand = new CreateUserCommand(name!, email, password);
        var results = await createUserCommandValidator.ValidateAsync(createUserCommand);

        // Assert
        results.ToString().Should().Contain(GenericValidatorMessages.ShouldBeShorterThanMessage(createUserCommandValidator.NameMaxLength));
    }

    [Fact]
    public async Task NewCreateUserCommand_Should_ContainGivenValidationMessage_WhenEmailIsEmptyAsync()
    {
        // Arrange
        var createUserCommandValidator = new CreateUserCommandValidator();
        var name = "test";
        var email = string.Empty;
        var password = "testpassword";

        // Act
        var createUserCommand = new CreateUserCommand(name!, email, password);
        var results = await createUserCommandValidator.ValidateAsync(createUserCommand);

        // Assert
        results.ToString().Should().Contain(GenericValidatorMessages.ShouldNotBeEmptyMessage());
    }

    [Fact]
    public async Task NewCreateUserCommand_Should_ContainGivenValidationMessage_WhenEmailIsTooLongAsync()
    {
        // Arrange
        var createUserCommandValidator = new CreateUserCommandValidator();
        var name = "test";
        var email = new string('a', createUserCommandValidator.EmailMaxLength) + "@gmail.com";
        var password = "testpassword";

        // Act
        var createUserCommand = new CreateUserCommand(name!, email, password);
        var results = await createUserCommandValidator.ValidateAsync(createUserCommand);

        // Assert
        results.ToString().Should().Contain(GenericValidatorMessages.ShouldBeShorterThanMessage(createUserCommandValidator.EmailMaxLength));
    }

    [Fact]
    public async Task NewCreateUserCommand_Should_ContainGivenValidationMessage_WhenEmailIsMalformedAsync()
    {
        // Arrange
        var createUserCommandValidator = new CreateUserCommandValidator();
        var name = "test";
        var email = "aaa@.com";
        var password = "testpassword";

        // Act
        var createUserCommand = new CreateUserCommand(name!, email, password);
        var results = await createUserCommandValidator.ValidateAsync(createUserCommand);

        // Assert
        results.ToString().Should().Contain(GenericValidatorMessages.ShouldComplyWithRFC2822StandardsMessage());
    }

    [Fact]
    public async Task NewCreateUserCommand_Should_ContainGivenValidationMessage_WhenPasswordIsEmptyAsync()
    {
        // Arrange
        var createUserCommandValidator = new CreateUserCommandValidator();
        var name = "test";
        var email = "test@test.com";
        var password = "";

        // Act
        var createUserCommand = new CreateUserCommand(name!, email, password);
        var results = await createUserCommandValidator.ValidateAsync(createUserCommand);

        // Assert
        results.ToString().Should().Contain(GenericValidatorMessages.ShouldNotBeEmptyMessage());
    }

    [Fact]
    public async Task NewCreateUserCommand_Should_ContainGivenValidationMessage_WhenPasswordIsTooLongAsync()
    {
        // Arrange
        var createUserCommandValidator = new CreateUserCommandValidator();
        var name = "test";
        var email = "test@test.com";
        var password = new string('a', createUserCommandValidator.PasswordMaxLength + 1);

        // Act
        var createUserCommand = new CreateUserCommand(name!, email, password);
        var results = await createUserCommandValidator.ValidateAsync(createUserCommand);

        // Assert
        results.ToString().Should().Contain(GenericValidatorMessages.ShouldBeShorterThanMessage(createUserCommandValidator.PasswordMaxLength));
    }
}
