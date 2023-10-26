using Domain;
using Domain.Users;
using MediatR;

namespace Application.Users.Create;

internal sealed class CreateUserCommandHandler : IRequestHandler<CreateUserCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserRepository _userRepository;

    public CreateUserCommandHandler(IUnitOfWork unitOfWork, IUserRepository userRepository)
    {
        _unitOfWork = unitOfWork;
        _userRepository = userRepository;
    }

    public async Task Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        await _userRepository.CreateAsync(new User(new Guid(), request.Name, request.Email, request.Password));

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
