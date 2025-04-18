using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Application.Interfaces
{
    public interface IApplicationDbContext
    {
        DbSet<User> Users { get; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}