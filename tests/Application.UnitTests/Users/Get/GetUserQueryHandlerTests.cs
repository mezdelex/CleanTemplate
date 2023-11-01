using Application.Contexts;
using Application.Users.Get;
using Domain.Users;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace Application.UnitTests.Users.Get;

public class GetUserQueryHandlerTests
{
    private readonly CancellationToken _cancellationToken;
    private readonly GetUserQueryHandler _handler;
    private readonly Mock<DbSet<User>> _dbSet;
    private readonly Mock<IApplicationDbContext> _context;

    public GetUserQueryHandlerTests()
    {
        _cancellationToken = new();
        _context = new();
        _dbSet = new();

        _handler = new GetUserQueryHandler(_context.Object);
    }

    [Fact]
    public async Task Handle_ValidIdGetUserQuery_ShouldReturnRequestedUserAsUserDTOAsync()
    {
        // Arrange
        var guid = Guid.NewGuid();
        var getUserQuery = new GetUserQuery(guid);
        var users = new List<User> { new(guid, "test1", "test1@test.com", "test1") };
        _dbSet.As<IAsyncEnumerable<User>>().Setup(mock => mock.GetAsyncEnumerator(It.IsAny<CancellationToken>()))
            .Returns(new TestAsyncEnumerator<User>(users.AsQueryable().GetEnumerator()));
        _dbSet.As<IQueryable<User>>().Setup(mock => mock.Provider).Returns(new TestAsyncQueryProvider<User>(users.AsQueryable().Provider));
        _dbSet.As<IQueryable<User>>().Setup(mock => mock.Expression).Returns(users.AsQueryable().Expression);
        _dbSet.As<IQueryable<User>>().Setup(mock => mock.ElementType).Returns(users.AsQueryable().ElementType);
        _dbSet.As<IQueryable<User>>().Setup(mock => mock.GetEnumerator()).Returns(users.AsQueryable().GetEnumerator());
        _context.Setup(mock => mock.Users).Returns(_dbSet.Object).Verifiable();

        // Act
        var result = await _handler.Handle(getUserQuery, _cancellationToken);

        // Assert
        result.Id.Should().Be(users[0].Id);
        result.Name.Should().Be(users[0].Name);
        result.Email.Should().Be(users[0].Email);
        _context.Verify();
    }
}
