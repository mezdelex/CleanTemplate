using Domain.Users;
using Microsoft.EntityFrameworkCore;

namespace Application;

public interface IApplicationDbContext
{
    DbSet<User> Users { get; set; }
}
