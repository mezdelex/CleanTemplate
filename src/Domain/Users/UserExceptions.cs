namespace Domain.Users;

public sealed class UserNotFoundException : Exception
{
    public UserNotFoundException(Guid id) : base($"The user with id {id} could not be found.") { }
}
