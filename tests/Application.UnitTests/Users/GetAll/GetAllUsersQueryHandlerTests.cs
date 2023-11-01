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
    public async Task Handle_GetAllUsersQuery_ShouldReturnPagedListOfRequestedUsersAsListOfUserDTOAndMetadataAsync()
    {
        // Arrange
        var page = 1;
        var pageSize = 2;
        var getAllUsersQuery = new GetAllUsersQuery(page, pageSize);
        var users = new List<User> {
            new(Guid.NewGuid(), "test1", "test1@test.com", "test1"),
            new(Guid.NewGuid(), "test2", "test2@test.com", "test2")
        };
        _dbSet.As<IAsyncEnumerable<User>>().Setup(mock => mock.GetAsyncEnumerator(It.IsAny<CancellationToken>()))
            .Returns(new TestAsyncEnumerator<User>(users.AsQueryable().GetEnumerator()));
        _dbSet.As<IQueryable<User>>().Setup(mock => mock.Provider).Returns(new TestAsyncQueryProvider<User>(users.AsQueryable().Provider));
        _dbSet.As<IQueryable<User>>().Setup(mock => mock.Expression).Returns(users.AsQueryable().Expression);
        _dbSet.As<IQueryable<User>>().Setup(mock => mock.ElementType).Returns(users.AsQueryable().ElementType);
        _dbSet.As<IQueryable<User>>().Setup(mock => mock.GetEnumerator()).Returns(users.AsQueryable().GetEnumerator());
        _context.Setup(mock => mock.Users).Returns(_dbSet.Object).Verifiable();

        // Act
        var result = await _handler.Handle(getAllUsersQuery, _cancellationToken);

        // Assert
        result.Items[0].Id.Should().Be(users[0].Id);
        result.Items[0].Name.Should().Be(users[0].Name);
        result.Items[0].Email.Should().Be(users[0].Email);
        result.Items[1].Id.Should().Be(users[1].Id);
        result.Items[1].Name.Should().Be(users[1].Name);
        result.Items[1].Email.Should().Be(users[1].Email);
        result.TotalCount.Should().Be(users.Count);
        result.Page.Should().Be(page);
        result.PageSize.Should().Be(pageSize);
        result.HasPreviousPage.Should().Be(false);
        result.HasNextPage.Should().Be(false);
        _context.Verify();
    }
}
