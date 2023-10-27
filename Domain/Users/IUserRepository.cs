namespace Domain.Users;

public interface IUserRepository
{
    Task CreateAsync(User user, CancellationToken cancellationToken = default);
}
