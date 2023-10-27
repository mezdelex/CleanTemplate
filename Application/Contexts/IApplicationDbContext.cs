using Domain.Users;
using Microsoft.EntityFrameworkCore;

namespace Application.Contexts;

public interface IApplicationDbContext
{
    DbSet<User> Users { get; set; }
}
