using Microsoft.EntityFrameworkCore;
using TurneroAPI.Domain.Entities;

namespace TurneroAPI.Application.Interfaces;

/// <summary>
/// Interface for the application's database context.
/// This abstraction allows the Application layer to remain independent of the specific data access technology (like EF Core).
/// </summary>
public interface IApplicationDbContext
{
    /// <summary>
    /// Gets or sets the DbSet for Users.
    /// </summary>
    DbSet<User> Users { get; }

    /// <summary>
    /// Saves changes made to the context.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The number of state entries written to the database.</returns>
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
