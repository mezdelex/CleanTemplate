using Application.Contexts;
using Application.Users.GetAll;
using Domain.Users;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace Application.UnitTests.Users.GetAll;

public class GetAllUsersQueryHandlerTests
{
    private readonly CancellationToken _cancellationToken;
    private readonly GetAllUsersQueryHandler _handler;
    private readonly Mock<DbSet<User>> _dbSet;
    private readonly Mock<IApplicationDbContext> _context;

    public GetAllUsersQueryHandlerTests()
    {
        _cancellationToken = new();
        _context = new();
        _dbSet = new();

        _handler = new GetAllUsersQueryHandler(_context.Object);
    }

    [Fact]
    public async Task Handle_GetAllUsersQuery_ReturnsGivenUsersAsListOfUserDTOAsync()
    {
        // Arrange
        var getAllUsersQuery = new GetAllUsersQuery();
        var users = new List<User> {
            new(Guid.NewGuid(), "test1", "test1@test.com", "test1"),
            new(Guid.NewGuid(), "test2", "test2@test.com", "test2")
        };
        _dbSet.As<IQueryable<User>>().Setup(mock => mock.Provider).Returns(users.AsQueryable().Provider);
        _dbSet.As<IQueryable<User>>().Setup(mock => mock.Expression).Returns(users.AsQueryable().Expression);
        _dbSet.As<IQueryable<User>>().Setup(mock => mock.ElementType).Returns(users.AsQueryable().ElementType);
        _dbSet.As<IQueryable<User>>().Setup(mock => mock.GetEnumerator()).Returns(users.AsQueryable().GetEnumerator());
        _context.Setup(mock => mock.Users).Returns(_dbSet.Object).Verifiable();

        // Act
        var result = await _handler.Handle(getAllUsersQuery, _cancellationToken);

        // Assert
        result[0].Id.Should().Be(users[0].Id);
        result[0].Name.Should().Be(users[0].Name);
        result[0].Email.Should().Be(users[0].Email);
        result[1].Id.Should().Be(users[1].Id);
        result[1].Name.Should().Be(users[1].Name);
        result[1].Email.Should().Be(users[1].Email);
        result.Count.Should().Be(users.Count);
        _context.Verify();
    }
}
