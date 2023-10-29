namespace Application.Users.Create;

public record CreatedUserEvent(Guid Id, string Name, string Email);
