using Application.Messages;
using Application.Users.Create;
using FluentAssertions;

namespace Application.UnitTests.Users.Create;

public class CreateUserCommandValidatorTests
{
    private readonly CancellationToken _cancellationToken;
    private readonly CreateUserCommandValidator _validator;

    public CreateUserCommandValidatorTests()
    {
        _cancellationToken = new CancellationToken();
        _validator = new CreateUserCommandValidator();
    }

    [Fact]
    public async Task NewCreateUserCommand_Should_ContainGivenValidationMessage_WhenNameIsEmptyAsync()
    {
        // Arrange
        var name = string.Empty;
        var email = "test@test.com";
        var password = "testpassword";

        // Act
        var createUserCommand = new CreateUserCommand(name!, email, password);
        var results = await _validator.ValidateAsync(createUserCommand, _cancellationToken);

        // Assert
        results.ToString().Should()
            .Contain(GenericValidatorMessages.ShouldNotBeEmptyMessage(nameof(createUserCommand.Name)));
    }

    [Fact]
    public async Task NewCreateUserCommand_Should_ContainGivenValidationMessage_WhenNameIsTooLongAsync()
    {
        // Arrange
        var name = new string('a', _validator.NameMaxLength + 1);
        var email = "test@test.com";
        var password = "testpassword";

        // Act
        var createUserCommand = new CreateUserCommand(name!, email, password);
        var results = await _validator.ValidateAsync(createUserCommand, _cancellationToken);

        // Assert
        results.ToString().Should()
            .Contain(GenericValidatorMessages.ShouldBeShorterThanMessage(nameof(createUserCommand.Name), _validator.NameMaxLength));
    }

    [Fact]
    public async Task NewCreateUserCommand_Should_ContainGivenValidationMessage_WhenEmailIsEmptyAsync()
    {
        // Arrange
        var name = "test";
        var email = string.Empty;
        var password = "testpassword";

        // Act
        var createUserCommand = new CreateUserCommand(name!, email, password);
        var results = await _validator.ValidateAsync(createUserCommand, _cancellationToken);

        // Assert
        results.ToString().Should()
            .Contain(GenericValidatorMessages.ShouldNotBeEmptyMessage(nameof(createUserCommand.Email)));
    }

    [Fact]
    public async Task NewCreateUserCommand_Should_ContainGivenValidationMessage_WhenEmailIsTooLongAsync()
    {
        // Arrange
        var name = "test";
        var email = new string('a', _validator.EmailMaxLength) + "@gmail.com";
        var password = "testpassword";

        // Act
        var createUserCommand = new CreateUserCommand(name!, email, password);
        var results = await _validator.ValidateAsync(createUserCommand, _cancellationToken);

        // Assert
        results.ToString().Should()
            .Contain(GenericValidatorMessages.ShouldBeShorterThanMessage(nameof(createUserCommand.Email), _validator.EmailMaxLength));
    }

    [Fact]
    public async Task NewCreateUserCommand_Should_ContainGivenValidationMessage_WhenEmailIsMalformedAsync()
    {
        // Arrange
        var name = "test";
        var email = "aaa@.com";
        var password = "testpassword";

        // Act
        var createUserCommand = new CreateUserCommand(name!, email, password);
        var results = await _validator.ValidateAsync(createUserCommand, _cancellationToken);

        // Assert
        results.ToString().Should()
            .Contain(GenericValidatorMessages.ShouldComplyWithRFC2822StandardsMessage(nameof(createUserCommand.Email)));
    }

    [Fact]
    public async Task NewCreateUserCommand_Should_ContainGivenValidationMessage_WhenPasswordIsEmptyAsync()
    {
        // Arrange
        var name = "test";
        var email = "test@test.com";
        var password = "";

        // Act
        var createUserCommand = new CreateUserCommand(name!, email, password);
        var results = await _validator.ValidateAsync(createUserCommand, _cancellationToken);

        // Assert
        results.ToString().Should()
            .Contain(GenericValidatorMessages.ShouldNotBeEmptyMessage(nameof(createUserCommand.Password)));
    }

    [Fact]
    public async Task NewCreateUserCommand_Should_ContainGivenValidationMessage_WhenPasswordIsTooLongAsync()
    {
        // Arrange
        var name = "test";
        var email = "test@test.com";
        var password = new string('a', _validator.PasswordMaxLength + 1);

        // Act
        var createUserCommand = new CreateUserCommand(name!, email, password);
        var results = await _validator.ValidateAsync(createUserCommand, _cancellationToken);

        // Assert
        results.ToString().Should()
            .Contain(GenericValidatorMessages.ShouldBeShorterThanMessage(nameof(createUserCommand.Password), _validator.PasswordMaxLength));
    }
}
