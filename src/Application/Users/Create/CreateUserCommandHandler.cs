using Application.Abstractions;
using Domain.Users;
using Domain;
using MediatR;
using FluentValidation;

namespace Application.Users.Create;

internal sealed class CreateUserCommandHandler : IRequestHandler<CreateUserCommand>
{
    private readonly IEventBus _eventBus;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserRepository _userRepository;

    public CreateUserCommandHandler(IEventBus eventBus, IUnitOfWork unitOfWork, IUserRepository userRepository)
    {
        _eventBus = eventBus;
        _unitOfWork = unitOfWork;
        _userRepository = userRepository;
    }

    public async Task Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var validator = new CreateUserCommandValidator();
        await validator.ValidateAndThrowAsync(request, cancellationToken);

        var newUser = new User(new Guid(), request.Name, request.Email, request.Password);

        await _userRepository.CreateAsync(newUser, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        await _eventBus.PublishAsync(new CreatedUserEvent(newUser.Id, newUser.Name, newUser.Email), cancellationToken);
    }
}
