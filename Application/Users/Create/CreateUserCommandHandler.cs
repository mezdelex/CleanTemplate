using Domain;
using Domain.Users;
using MediatR;

namespace Application.Users.Create;

internal class CreateUserCommandHandler : IRequestHandler<CreateUserCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserRepository _userRepository;

    public CreateUserCommandHandler(IUnitOfWork unitOfWork, IUserRepository userRepository)
    {
        _unitOfWork = unitOfWork;
        _userRepository = userRepository;
    }

    public async Task Handle(CreateUserCommand command, CancellationToken cancellationToken)
    {
        await _userRepository.CreateAsync(new User(new Guid(), command.Name, command.Email, command.Password));

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
