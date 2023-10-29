using Application.Abstractions;
using Application.Users.Create;
using Domain;
using Domain.Users;
using FluentValidation;
using FluentValidation.Results;
using Moq;

namespace Application.UnitTests.Users.Create;

public class CreateUserCommandHandlerTests
{
    private readonly CancellationToken _cancellationToken;
    private readonly CreateUserCommandHandler _handler;
    private readonly Mock<IEventBus> _eventBusMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<IValidator<CreateUserCommand>> _validatorMock;

    public CreateUserCommandHandlerTests()
    {
        _cancellationToken = new();
        _eventBusMock = new();
        _unitOfWorkMock = new();
        _userRepositoryMock = new();
        _validatorMock = new();

        _handler = new CreateUserCommandHandler(_eventBusMock.Object, _unitOfWorkMock.Object, _userRepositoryMock.Object, _validatorMock.Object);
    }

    [Fact]
    public async Task Handle_ValidCreateUserCommand_CreatesUserAndPublishesEventAsync()
    {
        // Arrange
        var createUserCommand = new CreateUserCommand("test", "test@gmail.com", "test");
        _eventBusMock.Setup(mock => mock.PublishAsync(It.IsAny<CreatedUserEvent>(), _cancellationToken)).Verifiable();
        _unitOfWorkMock.Setup(mock => mock.SaveChangesAsync(_cancellationToken)).Verifiable();
        _userRepositoryMock.Setup(mock => mock.CreateAsync(It.IsAny<User>(), _cancellationToken)).Verifiable();
        _validatorMock.Setup(mock => mock.ValidateAsync(createUserCommand, _cancellationToken)).ReturnsAsync(new ValidationResult()).Verifiable();

        // Act
        await _handler.Handle(createUserCommand, _cancellationToken);

        // Assert
        _eventBusMock.Verify();
        _unitOfWorkMock.Verify();
        _userRepositoryMock.Verify();
        _validatorMock.Verify();
    }
}
